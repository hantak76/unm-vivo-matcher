using System;
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

		public static Harvest LoadFromXML(string filename)
		{
			RdfXmlParser xmlParser = new RdfXmlParser();
			IGraph g = new Graph();

			xmlParser.Load(g, filename);

			return new Harvest(g);
		}
	}
}
