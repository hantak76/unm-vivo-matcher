using System;
using System.Collections.Generic;
using CommandLine;

using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

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



		// Process the triples created by the harvester
		// For each author, see if there is a match in our faculty
		// If there is, rewrite their triples so instead of making a new person, 
		//   the author is linked to the existing faculty
		// If there isn't a match, leave the author's triples
		// Finally, convert all the triples to the current format VIVO requires (ISF).  The harvester does not generate
		// the correct triples for VIVO 1.9.  For reference:
		//   https://wiki.lyrasis.org/display/VIVODOC19x/Publication+Model
		//   https://wiki.lyrasis.org/display/VIVODOC19x/Person+Model
		//   https://wiki.lyrasis.org/display/VTDA/VIVO-ISF+Ontology

		public void Process()
		{
			// Go through the harvest
			foreach (var document in IncomingHarvest.Documents) {
				foreach (var authorship in document.Authorships) {

					// If there isn't an author, skip
					if (authorship.Author == null) {
						Console.WriteLine("No author on authorship");
						continue;
					}

					if (!authorship.Author.IsPerson) {
						Console.WriteLine("Author is not a person - pmid: " + document.PMID);
						continue;
					}

					// At this point, we have a potential author to match
					// Look through the profiles (from vivo-pump) to see if there is a match
					var match = Profiles.FindMatch(authorship.Author.Name);

					// no match, skip, this means that autor
					if (match == null) {
						Console.WriteLine(@"Skipping author " + authorship.Author.Name.Last);
						continue;
					}

					Console.WriteLine(@"Updating " + authorship.Author.LastName + @" to " + match.TargetProfile.Uri);

					// Generates all the triples to remove (this is the new "person" it wants to create for the author)
					var triplesToRemove = new List<Triple>(authorship.Author.GetTriples());
					triplesToRemove.Add(authorship.LinkedAuthorTriple);

					// Adds the triple to link the author
					authorship.UpdateLinkedAuthor(match.TargetProfile.Uri);

					// Remove all the triples
					IncomingHarvest.Remove(triplesToRemove);
				}
			}

			// Convert triples to the ISF format
			IncomingHarvest.UpdateToISF();

			// Export the triples (currently this just pushes RDF)
			// TODO: Check the filename and output the correct format (RDF vs n3 triples)
			IncomingHarvest.ExportRdf(OutputHarvestFilename);
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

			// Load the faculty information from CSV file (comes from the vivo pump)
			var facultyProfiles = profiles.ProfileList.Load(profilesFilename);

			var m = new Matcher(facultyProfiles, harvest, outputFilename);

			// Run the matcher (will output to outputFilename)
			m.Process();

		}
	}
}
