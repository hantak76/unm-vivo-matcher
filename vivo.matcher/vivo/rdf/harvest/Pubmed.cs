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

			// Add the vcard
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

			foreach (var triple in Graph.GetTriplesWithPredicateObject(typePredicate,personObject))
			{
				Console.WriteLine("Person = " + triple.Subject);

				var newUri = vivo.utility.Individual.GenerateIndividual();

				Console.WriteLine("New URI = " + newUri);

				// Make a vcard

			}
			// Pull all the people a
			// Need to move person info in


			// Change vivo:linkedAuthor to vivo:relates
			// Change vivo:authorInAuthorship to vivo:relatedBy
			// Change vivo:linkedInfomrationResource to vivo:relates
			// Change vivo:informationResourceInAuthorship to vivo:relatedBy


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
