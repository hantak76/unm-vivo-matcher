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
				IUriNode authInAuthorship = CreateUriNode(@"j.3:authorInAuthorship");

				var nodes = Node.Graph.GetTriplesWithSubjectPredicate(Node, authInAuthorship);

				if (nodes.Count() == 0) return null;
				if (nodes.Count() > 1) return null;

				return nodes.ElementAt(0).Object;
			}
		}

		public Author Author {
			get {
				return new Author(LinkedAuthorNode);
			}
		}
	}
}