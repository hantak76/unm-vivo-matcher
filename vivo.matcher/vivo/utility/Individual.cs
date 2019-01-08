using System;
using System.Net;
namespace vivo.utility
{
	public class Individual
	{
		protected Individual()
		{
		}

		protected static long LongRandom(long min, long max, Random rand)
		{
			byte[] buf = new byte[8];
			rand.NextBytes(buf);
			long longRand = BitConverter.ToInt64(buf, 0);

			return (Math.Abs(longRand % (max - min)) + min);
		}

		protected static int IntRandom(int min, int max, Random rand)
		{
			return rand.Next(min, max);
		}

		protected static bool IsFreeIndividual(string uri)
		{
			// check with VIVO and see if the uri is available
			using (WebClient client = new WebClient())
			{
				string contents = "";
				try
				{
					contents = client.DownloadString(uri);
				}
				catch
				{
					return true;
				}
				return contents.Contains("Individual not found");
			}

		}

		public static string GenerateIndividual()
		{
			string uri;
			do
			{
				uri = @"http://vivo.health.unm.edu/individual/n" + IntRandom(0, int.MaxValue, new Random());

			} while (!IsFreeIndividual(uri));

			return uri;
		}
	}
}
