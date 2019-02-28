﻿using System;
using System.Runtime.Serialization;
using Elasticsearch.Net;

namespace Nest
{
	public partial interface IValidateQueryRequest
	{
		[DataMember(Name = "query")]
		QueryContainer Query { get; set; }
	}

	[InterfaceDataContract]
	public interface IValidateQueryRequest<T> : IValidateQueryRequest
		where T : class { }

	public partial class ValidateQueryRequest
	{
		public QueryContainer Query { get; set; }
	}

	public partial class ValidateQueryRequest<T>
		where T : class
	{
		public QueryContainer Query { get; set; }
	}

	[DescriptorFor("IndicesValidateQuery")]
	public partial class ValidateQueryDescriptor<T> where T : class
	{
		QueryContainer IValidateQueryRequest.Query { get; set; }

		public ValidateQueryDescriptor<T> Query(Func<QueryContainerDescriptor<T>, QueryContainer> querySelector) =>
			Assign(a => a.Query = querySelector?.Invoke(new QueryContainerDescriptor<T>()));
	}
}
