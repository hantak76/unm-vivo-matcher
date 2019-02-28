using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

namespace vivo.rdf.harvest
{
	public class Pubmed
	{
		protected string VCardPrefix = @"vcard";

		
		protected IGraph Graph { get; set; }

		protected Pubmed(IGraph g)
		{
			Graph = g;

			// Add the vcard namespace
			// Add the arg namespac
			Graph.NamespaceMap.AddNamespace(@"arg", new Uri(@"http://purl.obolibrary.org/obo/"));
			Graph.NamespaceMap.AddNamespace(@"vcard", new Uri(@"http://www.w3.org/2006/vcard/ns#"));
		}

		protected IEnumerable<Triple> DocumentTriples
		{	
			get {
				IUriNode rdfType = Graph.CreateUriNode(@"rdf:type");
				IUriNode document = Graph.CreateUriNode(@"j.2:Document");

				return Graph.GetTriplesWithPredicateObject(rdfType, document);
			}
		}

		public DocumentList Documents {
			get {
				var r = new DocumentList();
				foreach (var dTriple in DocumentTriples) {
					r.Add(new Document(dTriple.Subject));
				}
				return r;
			}
		}

		public void UpdateToISF()
		{
			List<Triple> additions = new List<Triple>();
			List<Triple> removals = new List<Triple>();

			var typePredicate = Graph.CreateUriNode(@"rdf:type");
			var personObject = Graph.CreateUriNode(@"j.4:Person");

			var argHasContanctInfoPredicate = Graph.CreateUriNode(@"arg:ARG_2000028");
			var argContactInfoForPredicate = Graph.CreateUriNode(@"arg:ARG_2000029");

			var vCardIndividualType = Graph.CreateUriNode(@"vcard:Individual");
			var vCardNameType = Graph.CreateUriNode(@"vcard:Name");

			var vCardHasName = Graph.CreateUriNode(@"vcard:hasName");

			var middleNamePredicate = Graph.CreateUriNode(@"j.3:middleName");
			var firstNamePredicate = Graph.CreateUriNode(@"vcard:givenName");
			var lastNamePredicate = Graph.CreateUriNode(@"vcard:familyName");

			var firstNameFoafPredicate = Graph.CreateUriNode(@"j.4:firstName");
			var lastNameFoafPredicate = Graph.CreateUriNode(@"j.4:lastName");

			foreach (var triple in Graph.GetTriplesWithPredicateObject(typePredicate,personObject))
			{
				Console.WriteLine("Base = " + Graph.BaseUri);

				var person = new Person(triple.Subject);

				var vCardUri = Graph.CreateUriNode(new Uri(vivo.utility.Individual.GenerateIndividual()));
				var nameUri = Graph.CreateUriNode(new Uri(vivo.utility.Individual.GenerateIndividual()));

				// Make a vcar
				additions.Add(new Triple(vCardUri, typePredicate, vCardIndividualType));

				// has contact info, has contact info for
				additions.Add(new Triple(triple.Subject, argHasContanctInfoPredicate, vCardUri));
				additions.Add(new Triple(vCardUri, argContactInfoForPredicate, triple.Subject));

				// Make the nam
				additions.Add(new Triple(nameUri, typePredicate, vCardNameType));
				additions.Add(new Triple(nameUri, firstNamePredicate, Graph.CreateLiteralNode(person.FirstName)));
				if (person.MiddleName != null) {
					additions.Add(new Triple(nameUri, middleNamePredicate, Graph.CreateLiteralNode(person.MiddleName)));
				}
				additions.Add(new Triple(nameUri, lastNamePredicate, Graph.CreateLiteralNode(person.LastName)));

				// Connect the name 
				additions.Add(new Triple(vCardUri, vCardHasName, nameUri));

				// Removal
				removals.AddRange(Graph.GetTriplesWithSubjectPredicate(triple.Subject, firstNameFoafPredicate));
				removals.AddRange(Graph.GetTriplesWithSubjectPredicate(triple.Subject, lastNameFoafPredicate));
				removals.AddRange(Graph.GetTriplesWithSubjectPredicate(triple.Subject, middleNamePredicate));

			}


			var relatedByPredicate = Graph.CreateUriNode(@"j.3:relatedBy");
			var relatesPredicate = Graph.CreateUriNode(@"j.3:relates");
			var linkedAuthorPredicate = Graph.CreateUriNode(@"j.3:linkedAuthor");
			var authorInAuthorshipPredicate = Graph.CreateUriNode("j.3:authorInAuthorship");
			var linkedInformationResourcePredicate = Graph.CreateUriNode("j.3:linkedInformationResource");
			var informationResourceinAuthorshipPredicate = Graph.CreateUriNode("j.3:informationResourceInAuthorshp");

			foreach (var triple in Graph.GetTriplesWithPredicate(linkedAuthorPredicate))
			{
				additions.Add(new Triple(triple.Subject, relatesPredicate, triple.Object));
				removals.Add(triple);
			}

			foreach (var triple in Graph.GetTriplesWithPredicate(authorInAuthorshipPredicate))
			{
				additions.Add(new Triple(triple.Subject, relatedByPredicate, triple.Object));
				removals.Add(triple);
			}

			foreach (var triple in Graph.GetTriplesWithPredicate(linkedInformationResourcePredicate))
			{
				additions.Add(new Triple(triple.Subject, relatesPredicate, triple.Object));
				removals.Add(triple);
			}

			foreach (var triple in Graph.GetTriplesWithPredicate(informationResourceinAuthorshipPredicate))
			{
				additions.Add(new Triple(triple.Subject, relatedByPredicate, triple.Object));
				removals.Add(triple);
			}

			System.Console.WriteLine("Adding " + additions.Count + ", Removing " + removals.Count);

			Graph.Assert(additions);
			Graph.Retract(removals);
    
		}

		public void ExportRdf(string filename)
		{
			RdfXmlWriter writer = new RdfXmlWriter();

			writer.Save(Graph, filename);
		}

		public void ExportNTriples(string filename)
		{
			var writer = new NTriplesWriter();
			writer.Save(Graph, filename);
		}


		public static Pubmed LoadFromXML(string filename)
		{
			RdfXmlParser xmlParser = new RdfXmlParser();
			IGraph g = new Graph();

			xmlParser.Load(g, filename);

			return new Pubmed(g);
		}

		public bool Remove(List<Triple> triplesToRemove)
		{
			return Graph.Retract(triplesToRemove);
		}
	}
}
