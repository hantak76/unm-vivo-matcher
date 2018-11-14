using System;

namespace vivo
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// Load the RDF from the harvester
			var rdf = vivo.rdf.Harvest.LoadFromXML(@"C:\Users\CMHantak\Desktop\harvest.rdf.xml");

			// Get all the profiles
			rdf.Debug();
		}
	}
}
