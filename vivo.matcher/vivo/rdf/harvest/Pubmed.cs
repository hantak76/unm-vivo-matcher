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
		protected IGraph Graph { get; set; }

		protected Pubmed(IGraph g)
		{
			Graph = g;
		}

		/*
		protected IEnumerable<Triple> GetPeople()
		{
			IUriNode rdfType = Graph.CreateUriNode(@"rdf:type");
			IUriNode person = Graph.CreateUriNode(@"j.4:Person");

			return Graph.GetTriplesWithPredicateObject(rdfType, person);
		}

		protected IEnumerable<Triple> GetAuthorshipsForDocument(INode node)
		{
			IUriNode infoAuthorship = Graph.CreateUriNode(@"j.3:informationResourceInAuthorship");

			return Graph.GetTriplesWithSubjectPredicate(node, infoAuthorship);
		}
		*/

		public void Debug(string outFilename)
		{
			foreach (var doc in Documents) {
				var docTitle = doc.Title;

				Console.WriteLine("Document: " + docTitle);

				foreach (var authorship in doc.Authorships) {

					var author = authorship.Author;

					var authorLabel = author.Label;

					Console.WriteLine("\tAuthor: " + authorLabel);

					if (authorLabel == @"Larson, Richard S") {
						Console.WriteLine("\t\tReplacing " + authorLabel);
						var triplesToRemove = new List<Triple>(author.GetTriples());
						triplesToRemove.Add(authorship.LinkedAuthorTriple);

						authorship.UpdateLinkedAuthor(@"http://vivo.health.unm.edu/individual/n6221");

						Remove(triplesToRemove);

					}
				}
				Console.WriteLine();
			}

			ExportNTriples(outFilename);


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
