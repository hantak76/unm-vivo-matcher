using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace vivo.rdf.harvest
{
	public class AuthorshipList: IEnumerable<Authorship>
	{
		protected List<Authorship> Authorships { get; set; }

		public AuthorshipList()
		{
			Authorships = new List<Authorship>();
		}

		public void Add(Authorship a)
		{
			Authorships.Add(a);
		}

		public IEnumerator<Authorship> GetEnumerator()
		{
			return Authorships.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
