using System;
using CommandLine;

namespace vivo
{
	class Matcher
	{
		public profiles.ProfileList Profiles { get; set; }
		public rdf.harvest.Pubmed IncomingHarvest { get; set; }
		public string OutputHarvestFilename { get; set; }

		public Matcher(profiles.ProfileList profiles, rdf.harvest.Pubmed harvest, string outputFilename)
		{
			Profiles = profiles;
			IncomingHarvest = harvest;
			OutputHarvestFilename = outputFilename;
		}

		public void Process()
		{
			// Go through the harvest
		}

		public class ProgramOption
		{
			[Option('p', @"profiles", Required = true, HelpText = @"Profile List CSV")]
			public string ProfileFilename { get; set; }

			[Option('h', @"harvest", Required = true, HelpText = @"Input Harvest RDF")]
			public string HarvestFilename { get; set; }

			[Option('o', @"output", Required = true, HelpText = @"Output Harvest RDF")]
			public string OutputFilename { get; set; }
		}

		public static void Main(string[] args)
		{
			string incomingFilename = "";
			string outputFilename = "";
			string profilesFilename = "";

			Parser.Default.ParseArguments<ProgramOption>(args).WithParsed<ProgramOption>(
				o =>
				{
					profilesFilename = o.ProfileFilename;
					incomingFilename = o.HarvestFilename;
					outputFilename = o.OutputFilename;
				}
			);

			if ((incomingFilename == "") || (profilesFilename == "") || (outputFilename == ""))
			{
				return;
			}

			// Load the RDF from the harvester
			var harvest = vivo.rdf.harvest.Pubmed.LoadFromXML(incomingFilename);

			var facultyProfiles = profiles.ProfileList.Load(profilesFilename);

			var m = new Matcher(facultyProfiles, harvest, outputFilename);

			m.Process();

		}
	}
}
