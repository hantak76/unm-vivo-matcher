using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace vivo.rdf.harvest
{
	public class Document : GraphNode
	{
		public Document(INode node) : base(node)
		{
		}

		public string Title {
			get {
				return GetObjLiteralValue(Node, CreateUriNode(@"j.3:Title"));
			}
		}

		protected IEnumerable<Triple> AuthorshipTriples
		{
			get {
				//IUriNode infoAuthorship = CreateUriNode(@"j.3:linkedInformationResource");
				IUriNode linkedResource = CreateUriNode(@"j.3:linkedInformationResource");
				return Node.Graph.GetTriplesWithPredicateObject(linkedResource, Node);
				//return Node.Graph.GetTriplesWithSubjectPredicate(Node, infoAuthorship);
			}
		}

		public AuthorshipList Authorships {
			get {
				var r = new AuthorshipList();

				foreach (var aTriple in AuthorshipTriples) {
					r.Add(new Authorship(aTriple.Subject));
				}
				return r;
			}
		}
	}
}