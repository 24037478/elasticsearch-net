﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nest
{
	[DataContract]
	public class IndicesStats
	{
		[DataMember(Name ="primaries")]
		public IndexStats Primaries { get; internal set; }

		[DataMember(Name ="shards")]
		[JsonConverter(typeof(VerbatimDictionaryKeysJsonConverter<string, ShardStats[]>))]
		public IReadOnlyDictionary<string, ShardStats[]> Shards { get; internal set; } = EmptyReadOnly<string, ShardStats[]>.Dictionary;

		[DataMember(Name ="total")]
		public IndexStats Total { get; internal set; }

		/// <summary>
		/// Universal Unique Identifier for the index
		/// </summary>
		/// <remarks>
		/// Introduced in Elasticsearch 6.4.0
		/// </remarks>
		[DataMember(Name ="uuid")]
		public string UUID { get; }
	}
}
