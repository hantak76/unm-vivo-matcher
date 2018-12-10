using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace vivo.rdf.harvest
{
	public class DocumentList : IEnumerable<Document>
	{
		protected List<Document> Documents { get; set; }

		public DocumentList()
		{
			Documents = new List<Document>();
		}

		public void Add(Document d)
		{
			Documents.Add(d);
		}

		public IEnumerator<Document> GetEnumerator()
		{
			return Documents.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
