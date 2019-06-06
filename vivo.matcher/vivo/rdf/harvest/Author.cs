using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using vivo.profiles;

namespace vivo.rdf.harvest
{
	public class Author : GraphNode
	{
		public Author(INode node) : base(node)
		{
		}

		public List<Triple> GetTriples()
		{
			return Node.Graph.GetTriplesWithSubject(Node).ToList();
		}

		public string FirstName {
			get {
				return GetObjLiteralValue(Node, CreateUriNode(QualifiedNames.Foaf.FirstName));
			}
		}

		public string LastName {
			get {
				return GetObjLiteralValue(Node, CreateUriNode(QualifiedNames.Foaf.LastName));
			}
		}

		public string MiddleName {
			get {
				return GetObjLiteralValue(Node, CreateUriNode(QualifiedNames.Core.MiddleName));
			}
		}

		public bool IsPerson {
			get {
				return Node.Graph.ContainsTriple(new Triple(Node, CreateUriNode(QualifiedNames.Rdf.Type), CreateUriNode(QualifiedNames.Foaf.Person)));
			}
		}


		public Name Name {
			get {
				return new Name(FirstName, MiddleName, LastName);
			}
		}

	}
}