using System;
namespace vivo.profiles
{
	public class Profile
	{
		public Name Name { get; set; }
		public string Uri { get; set; }

		public Profile(string first, string middle, string last, string uri)
		{
			Name = new Name(first, middle, last);
			Uri = uri;
		}
	}
}
