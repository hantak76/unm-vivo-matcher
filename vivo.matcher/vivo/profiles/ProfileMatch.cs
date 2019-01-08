using System;
namespace vivo.profiles
{
	public class ProfileMatch
	{
		public Name SourceName { get; set; }
		public Profile TargetProfile { get; set; }
		public int Score { get; set; }

		public ProfileMatch(Name source, Profile target, int score)
		{
			SourceName = source;
			TargetProfile = target;
			Score = score;
		}
	}
}
