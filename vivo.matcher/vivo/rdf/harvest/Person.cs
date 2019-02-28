using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using vivo.profiles;

namespace vivo.rdf.harvest
{
	public class Person : Author
	{
		public Person(INode node) : base(node)
		{
		}

	}
}