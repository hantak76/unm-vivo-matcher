using System;

namespace vivo
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// Load the RDF from the harvester
			var harvest = vivo.rdf.Harvest.LoadFromXML(@"/Users/hantak/Downloads/harvest.rdf.xml");

			// Get all the profiles
			harvest.Debug();
		}
	}
}
