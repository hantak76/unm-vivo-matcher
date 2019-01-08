using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace vivo.rdf.harvest
{
	public class Authorship : GraphNode
	{
		public Authorship(INode node) : base(node)
		{
		}

		protected INode LinkedAuthorNode {
			get {
				if (LinkedAuthorTriple == null) return null;

				return LinkedAuthorTriple.Object;
			}
		}

		public Triple LinkedAuthorTriple {
			get {
				var nodes = Node.Graph.GetTriplesWithSubjectPredicate(Node, LinkedAuthorPredicate);
				if (nodes.Count() == 0) return null;
				if (nodes.Count() > 1) return null;
				return nodes.ElementAt(0);
			}
		}

		public Author Author {
			get {
				return new Author(LinkedAuthorNode);
			}
		}

		public void UpdateLinkedAuthor(string authorUri)
		{
			var t = new Triple(
				Node,
				LinkedAuthorPredicate,
				CreateUriNode(UriFactory.Create(authorUri))
			);

			Node.Graph.Assert(t);
		}
	}
}