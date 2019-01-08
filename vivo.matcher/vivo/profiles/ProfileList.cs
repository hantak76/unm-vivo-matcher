using System.Collections.Generic;

namespace vivo.profiles
{
	public class ProfileList : IEnumerable<Profile>
	{
		protected List<Profile> Data { get; set; }

		public int MatchValue {
			get {
				return 5;
			}
		}

		public ProfileList()
		{
			Data = new List<Profile>();
		}

		public void Add(Profile p)
		{
			Data.Add(p);
		}

		public static ProfileList Load(string filename)
		{
			var r = new ProfileList();

			var csv = System.IO.File.ReadAllText(filename);
			foreach (var line in Csv.CsvReader.ReadFromText(csv))
			{
				r.Add(new Profile(line["first_name"], line["middle_name"], line["last_name"], line["uri"]));
			}

			return r;
		}

		public ProfileMatch FindMatch(string first, string middle, string last)
		{
			return FindMatch(new Name(first, middle, last));
		}

		public ProfileMatch FindMatch(Name name)
		{
			Profile targetProfile = null;

			var score = int.MaxValue;
			foreach (var p in this) {
				var testScore = p.Name.Similarity(name);
				if ( (testScore < score) && (testScore < MatchValue) ) {
					targetProfile = p;
					score = testScore;
				}
			}

			if (targetProfile == null) return null;

			return new ProfileMatch(name, targetProfile, score);
		}

		public IEnumerator<Profile> GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
