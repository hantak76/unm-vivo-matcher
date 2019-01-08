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
				return GetObjLiteralValue(Node, CreateUriNode(@"j.4:firstName"));
			}
		}

		public string LastName {
			get {
				return GetObjLiteralValue(Node, CreateUriNode(@"j.4:lastName"));
			}
		}

		public string MiddleName {
			get {
				return GetObjLiteralValue(Node, CreateUriNode(@"j.3:middleName"));
			}
		}

		public Name Name {
			get {
				return new Name(FirstName, MiddleName, LastName);
			}
		}

	}
}