
namespace vivo
{
	public static class QualifiedNames
	{
		public static class Rdf
		{
			public const string NamespaceUri = @"http://www.w3.org/1999/02/22-rdf-syntax-ns#";
			public static System.Uri Type = new System.Uri(NamespaceUri + @"type");
			public static System.Uri Label = new System.Uri(NamespaceUri + @"label");
		}
		 
		public static class Foaf
		{
			public const string NamespaceUri = @"http://xmlns.com/foaf/0.1/";
			public static System.Uri Person = new System.Uri(NamespaceUri + @"Person");
			public static System.Uri FirstName = new System.Uri(NamespaceUri + @"firstName");
			public static System.Uri LastName = new System.Uri(NamespaceUri + @"lastName");
		}

		public static class Bibo
		{
			public const string NamespaceUri = @"http://purl.org/ontology/bibo/";
			public static System.Uri Document = new System.Uri(NamespaceUri + @"Document");
			public static System.Uri Pmid = new System.Uri(NamespaceUri + @"pmid");
		}

		public static class Arg
		{
			public const string NamespaceUri = @"http://purl.obolibrary.org/obo/";
			public static System.Uri HasContactInfo = new System.Uri(NamespaceUri + @"ARG_2000028");
			public static System.Uri ContactInfoFor = new System.Uri(NamespaceUri + @"ARG_2000029");
		}

		public static class VCard
		{
			public const string NamespaceUri = @"http://www.w3.org/2006/vcard/ns#";
			public static System.Uri Individual = new System.Uri(NamespaceUri + @"Individual");
			public static System.Uri Name = new System.Uri(NamespaceUri + @"Name");
			public static System.Uri HasName = new System.Uri(NamespaceUri + @"hasName");
			public static System.Uri FirstName = new System.Uri(NamespaceUri + @"giveName");
			public static System.Uri LastName = new System.Uri(NamespaceUri + @"familyName");
		}

		public static class Core
		{
			public const string NamespaceUri = @"http://vivoweb.org/ontology/core#";
			public static System.Uri MiddleName = new System.Uri(NamespaceUri + @"middleName");
			public static System.Uri RelatedBy = new System.Uri(NamespaceUri + @"relatedBy");
			public static System.Uri Relates = new System.Uri(NamespaceUri + @"relates");
			public static System.Uri Title = new System.Uri(NamespaceUri + @"title");
			public static System.Uri LinkedAuthor = new System.Uri(NamespaceUri + @"linkedAuthor");
			public static System.Uri AuthorInAuthorship = new System.Uri(NamespaceUri + @"authorInAuthorship");
			public static System.Uri LinkedInformationResource = new System.Uri(NamespaceUri + @"linkedInformationResource");
			public static System.Uri InformationResourceInAuthorship = new System.Uri(NamespaceUri + @"informationResourceInAuthorship");
		}
	}
}