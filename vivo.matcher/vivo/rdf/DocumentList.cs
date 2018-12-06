using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace vivo.rdf
{
	public class DocumentList
	{
		public List<Document> Document { get; set; }

		public DocumentList()
		{
			Nodes = new List<INode>();
		}

		public void Add(INode Node)
		{
			Nodes.Add(Node);
		}
	}