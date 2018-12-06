using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace vivo.rdf.harvest
{
	public class Pubmed
	{
		protected IGraph Graph { get; set; }

		protected Pubmed(IGraph g)
		{
			Graph = g;
		}

		protected string GetObjLiteralValue(INode subj, INode pred)
		{
			return GetObjLiteralValue(Graph.GetTriplesWithSubjectPredicate(subj, pred));
		}

		protected string GetObjLiteralValue(IEnumerable<Triple> list)
		{
			if (list.Count() != 1) return null;

			return GetObjLiteralValue(list.ElementAt(0));
		}

		protected string GetObjLiteralValue(Triple triple)
		{
			if (triple == null) return @"";

			if (triple.Object.NodeType != NodeType.Literal) return null;
			var literal = triple.Object as LiteralNode;

			if (literal == null) return null;

			return literal.Value;
		}

		public string GetFirstName(INode node)
		{
			return GetObjLiteralValue(node, Graph.CreateUriNode(@"j.4:firstName"));
		}

		public string GetLastName(INode node)
		{
			return GetObjLiteralValue(node, Graph.CreateUriNode(@"j.4:lastName"));
		}

		public string GetMiddleName(INode node)
		{
			return GetObjLiteralValue(node, Graph.CreateUriNode(@"j.3:middleName"));
		}

		public string GetLabel(INode node)
		{
			return GetObjLiteralValue(node, Graph.CreateUriNode(@"rdfs:label"));
		}

		public string GetDocumentTitle(INode node)
		{
			return GetObjLiteralValue(node, Graph.CreateUriNode(@"j.3:Title"));
		}

		public INode GetLinkedAuthor(INode node)
		{
			IUriNode authInAuthorship = Graph.CreateUriNode(@"j.3:authorInAuthorship");

			var nodes = Graph.GetTriplesWithSubjectPredicate(node, authInAuthorship);

			if (nodes.Count() == 0) return null;

			return nodes.ElementAt(0).Object;
		}

		protected IEnumerable<Triple> GetPeople()
		{
			IUriNode rdfType = Graph.CreateUriNode(@"rdf:type");
			IUriNode person = Graph.CreateUriNode(@"j.4:Person");

			return Graph.GetTriplesWithPredicateObject(rdfType, person);
		}

		protected IEnumerable<Triple> GetDocuments()
		{
			IUriNode rdfType = Graph.CreateUriNode(@"rdf:type");
			IUriNode document = Graph.CreateUriNode(@"j.2:Document");

			return Graph.GetTriplesWithPredicateObject(rdfType, document);
		}

		protected IEnumerable<Triple> GetAuthorshipsForDocument(INode node)
		{
			IUriNode infoAuthorship = Graph.CreateUriNode(@"j.3:informationResourceInAuthorship");

			return Graph.GetTriplesWithSubjectPredicate(node, infoAuthorship);
		}

		public void Debug()
		{
			foreach (Triple docTriple in GetDocuments()) {
				var documentTitle = GetDocumentTitle(docTriple.Subject);

				Console.WriteLine("Document: " + documentTitle);

				foreach (Triple authTriple in GetAuthorshipsForDocument(docTriple.Subject)) {

					var author = GetLinkedAuthor(authTriple.Object);

					var authorLabel = GetLabel(author);

					Console.WriteLine("\tAuthor: " + authorLabel);

					if (authorLabel == @"Larson, Richard S") {
						Console.WriteLine("\t\tReplacing " + authorLabel);
					}
				}
				Console.WriteLine();
			}
		}

		public static Harvest LoadFromXML(string filename)
		{
			RdfXmlParser xmlParser = new RdfXmlParser();
			IGraph g = new Graph();

			xmlParser.Load(g, filename);

			return new Harvest(g);
		}
	}
}
