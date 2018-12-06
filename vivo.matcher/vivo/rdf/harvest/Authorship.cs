using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace vivo.rdf.harvest
{
	public class Authorship
	{
		protected INode Node { get; set; }

		public Authorship(INode node)
		{
			Node = node;
		}


	}
}