﻿using System.Collections.Generic;
using System.Text;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Nest
{
	internal class QueryContainerFormatter : IJsonFormatter<QueryContainer>
	{
		public QueryContainer Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
		{
			var token = reader.GetCurrentJsonToken();
			var queryFormatter = DynamicObjectResolver.ExcludeNullCamelCase.GetFormatter<QueryContainer>();

			if (token == JsonToken.BeginObject)
				return queryFormatter.Deserialize(ref reader, formatterResolver);

			if (token != JsonToken.String)
				return null;

			var escapedJson = reader.ReadNextBlockSegment();
			// add 1 to offset for escape value
			var escapedReader = new JsonReader(escapedJson.Array, escapedJson.Offset + 1);
			return queryFormatter.Deserialize(ref escapedReader, formatterResolver);
		}

		public void Serialize(ref JsonWriter writer, QueryContainer value, IJsonFormatterResolver formatterResolver)
		{
			var queryFormatter = formatterResolver.GetFormatter<IQueryContainer>();
			queryFormatter.Serialize(ref writer, value, formatterResolver);
		}
	}

	internal class QueryContainerInterfaceFormatter : IJsonFormatter<IQueryContainer>
	{
		public IQueryContainer Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
		{
			var queryFormatter = formatterResolver.GetFormatter<QueryContainer>();
			return queryFormatter.Deserialize(ref reader, formatterResolver);
		}

		public void Serialize(ref JsonWriter writer, IQueryContainer value, IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var rawQuery = value.RawQuery;
			if ((!rawQuery?.Raw.IsNullOrEmpty() ?? false) && rawQuery.IsWritable)
			{
				writer.WriteRaw(Encoding.UTF8.GetBytes(rawQuery.Raw));
				return;
			}

			var queryFormatter = DynamicObjectResolver.ExcludeNullCamelCase.GetFormatter<IQueryContainer>();
			queryFormatter.Serialize(ref writer, value, formatterResolver);
		}
	}

	// TODO: Unify with QueryContainerCollectionFormatter
	public class QueryContainerListFormatter : IJsonFormatter<List<QueryContainer>>
	{
		private static readonly QueryContainerFormatter QueryContainerFormatter =
			new QueryContainerFormatter();

		private static readonly QueryContainerInterfaceFormatter QueryContainerInterfaceFormatter =
			new QueryContainerInterfaceFormatter();

		public List<QueryContainer> Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
		{
			var token = reader.GetCurrentJsonToken();
			switch (token)
			{
				case JsonToken.BeginArray:
				{
					var count = 0;
					var queryContainers = new List<QueryContainer>();
					while (reader.ReadIsInArray(ref count))
						queryContainers.Add(QueryContainerFormatter.Deserialize(ref reader, formatterResolver));

					return queryContainers;
				}
				case JsonToken.BeginObject:
				{
					var queryContainers = new List<QueryContainer>
					{
						QueryContainerFormatter.Deserialize(ref reader, formatterResolver)
					};

					return queryContainers;
				}
				default:
					reader.ReadNextBlock();
					return null;
			}
		}

		public void Serialize(ref JsonWriter writer, List<QueryContainer> value, IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
				writer.WriteNull();
			else
			{
				writer.WriteBeginArray();
				var e = value.GetEnumerator();
				try
				{
					var written = false;
					while (e.MoveNext())
					{
						if (e.Current != null && e.Current.IsWritable)
						{
							if (written)
								writer.WriteValueSeparator();

							QueryContainerInterfaceFormatter.Serialize(ref writer, e.Current, formatterResolver);
							written = true;
						}
					}
				}
				finally
				{
					e.Dispose();
				}

				writer.WriteEndArray();
			}
		}
	}

	public class QueryContainerCollectionFormatter : IJsonFormatter<IEnumerable<QueryContainer>>
	{
		private static readonly QueryContainerFormatter QueryContainerFormatter =
			new QueryContainerFormatter();

		private static readonly QueryContainerInterfaceFormatter QueryContainerInterfaceFormatter =
			new QueryContainerInterfaceFormatter();

		public IEnumerable<QueryContainer> Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
		{
			var token = reader.GetCurrentJsonToken();
			switch (token)
			{
				case JsonToken.BeginArray:
				{
					var count = 0;
					var queryContainers = new List<QueryContainer>();
					while (reader.ReadIsInArray(ref count))
						queryContainers.Add(QueryContainerFormatter.Deserialize(ref reader, formatterResolver));

					return queryContainers;
				}
				case JsonToken.BeginObject:
				{
					var queryContainers = new List<QueryContainer>
					{
						QueryContainerFormatter.Deserialize(ref reader, formatterResolver)
					};

					return queryContainers;
				}
				default:
					reader.ReadNextBlock();
					return null;
			}
		}

		public void Serialize(ref JsonWriter writer, IEnumerable<QueryContainer> value, IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
				writer.WriteNull();
			else
			{
				writer.WriteBeginArray();
				var e = value.GetEnumerator();
				try
				{
					var written = false;
					while (e.MoveNext())
					{
						if (e.Current != null && e.Current.IsWritable)
						{
							if (written)
								writer.WriteValueSeparator();

							QueryContainerInterfaceFormatter.Serialize(ref writer, e.Current, formatterResolver);
							written = true;
						}
					}
				}
				finally
				{
					e.Dispose();
				}

				writer.WriteEndArray();
			}
		}
	}
}