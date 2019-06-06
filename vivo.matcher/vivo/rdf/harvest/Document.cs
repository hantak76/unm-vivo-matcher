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

		public string PMID {
			get {
				return GetObjLiteralValue(Node, CreateUriNode(QualifiedNames.Bibo.Pmid));
			}
		}

		public string Title {
			get {
				return GetObjLiteralValue(Node, CreateUriNode(QualifiedNames.Core.Title));
			}
		}

		protected IEnumerable<Triple> AuthorshipTriples
		{
			get {
				IUriNode linkedResource = CreateUriNode(QualifiedNames.Core.LinkedInformationResource);
				return Node.Graph.GetTriplesWithPredicateObject(linkedResource, Node);
				//IUriNode infoAuthorship = CreateUriNode(QualifiedNames.Core.InformationResourceInAuthorship);
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