using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace vivo.rdf.harvest
{
	public class GraphNode
	{
		protected INode Node { get; set; }

		protected INode LinkedAuthorPredicate {
			get {
				return CreateUriNode(QualifiedNames.Core.LinkedAuthor);
			}
		}

		protected GraphNode(INode node)
		{
			Node = node;
		}

		protected IUriNode CreateUriNode(System.Uri uri)
		{
			return Node.Graph.CreateUriNode(uri);
		}

		protected string GetObjLiteralValue(INode subj, INode pred)
		{
			return GetObjLiteralValue(Node.Graph.GetTriplesWithSubjectPredicate(subj, pred));
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

		public string Label {
			get {
				return GetObjLiteralValue(Node, CreateUriNode(QualifiedNames.Rdf.Label));
			}
		}
	}
}