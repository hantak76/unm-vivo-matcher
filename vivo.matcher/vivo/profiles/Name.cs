using System;
using System.Linq;

namespace vivo.profiles
{
	public class Name
	{
		public string First { get; private set; }
		public string Middle { get; private set; } 
		public string Last { get; private set; }

		public string FirstCompareValue { get; private set; }
		public string MiddleCompareValue { get; private set; }
		public string LastCompareValue { get; private set; }


		public Name(string first, string middle, string last)
		{
			First = first;
			Middle = middle;
			Last = last;

			FirstCompareValue = First.ToLower();
		}

		protected string PrepareForCompare(string v)
		{
			return (new string(v.ToCharArray().Where(c => !char.IsPunctuation(c)).ToArray())).ToLower();

		}

		public int Compare(string a, string b)
		{
			return Fastenshtein.Levenshtein.Distance(a.ToLower(), b.ToLower());
		}

		public int FirstSimilarity(Name test) { return Compare(First, test.First); }
		public int MiddleSimilarity(Name test) { return Compare(Middle, test.Middle); }
		public int LastSimilarity(Name test) { return Compare(Last, test.Last); }

		public int similarity(Name test)
		{
			return FirstSimilarity(test) + MiddleSimilarity(test) + LastSimilarity(test);
		}
	}
}
