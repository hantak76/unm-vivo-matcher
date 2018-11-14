using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace vivo.rdf
{
	public class Harvest
	{
		protected IGraph Graph { get; set; }
		
		protected Harvest(IGraph g)
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
			return GetObjLiteralValue(node, Graph.CreateUriNode(@"j.4:lastName"));		}

		public string GetMiddleName(INode node)
		{
			return GetObjLiteralValue(node, Graph.CreateUriNode(@"j.3:middleName"));
		}

		public string GetLabel(INode node)
		{
			return GetObjLiteralValue(node, Graph.CreateUriNode(@"rdfs:label")); 
		}

		protected IEnumerable<Triple> GetPeople()
		{
			IUriNode rdfType = Graph.CreateUriNode(@"rdf:type");
			IUriNode person = Graph.CreateUriNode(@"j.4:Person");

			return Graph.GetTriplesWithPredicateObject(rdfType, person);
		}

		public void Debug()
		{
			foreach (Triple t in GetPeople())
			{
				Console.WriteLine(t.ToString());
				Console.WriteLine(GetFirstName(t.Subject) + @" " + GetMiddleName(t.Subject) + @" " + GetLastName(t.Subject) + @" (" + GetLabel(t.Subject) + @") ");
			}

			var a = Console.ReadLine();
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
