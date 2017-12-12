using EsaClient.Responses;
using System;
using Utf8Json;
using Utf8Json.Internal;

namespace EsaClient.Json
{
    public class PaginationItemFormatter<T> : IJsonFormatter<Pagination<T>>
    {
        readonly AutomataDictionary dictionary;

        public PaginationItemFormatter(string itemFieldName)
        {
            dictionary = new Utf8Json.Internal.AutomataDictionary();
            dictionary.Add(JsonWriter.GetEncodedPropertyNameWithoutQuotation(itemFieldName), 0);
            dictionary.Add(JsonWriter.GetEncodedPropertyNameWithoutQuotation(nameof(Pagination<T>.prev_page)), 1);
            dictionary.Add(JsonWriter.GetEncodedPropertyNameWithoutQuotation(nameof(Pagination<T>.next_page)), 2);
            dictionary.Add(JsonWriter.GetEncodedPropertyNameWithoutQuotation(nameof(Pagination<T>.total_count)), 3);
            dictionary.Add(JsonWriter.GetEncodedPropertyNameWithoutQuotation(nameof(Pagination<T>.page)), 4);
            dictionary.Add(JsonWriter.GetEncodedPropertyNameWithoutQuotation(nameof(Pagination<T>.per_page)), 5);
            dictionary.Add(JsonWriter.GetEncodedPropertyNameWithoutQuotation(nameof(Pagination<T>.max_per_page)), 6);
        }

        public void Serialize(ref JsonWriter writer, Pagination<T> value, IJsonFormatterResolver formatterResolver)
        {
            throw new NotSupportedException();
        }

        public Pagination<T> Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            var count = 0;
            var item = new Pagination<T>();
            while (reader.ReadIsInObject(ref count))
            {
                var propName = reader.ReadPropertyNameSegmentRaw();
                var key = -1;
                dictionary.TryGetValue(propName, out key);
                switch (key)
                {
                    case 0:
                        item.items = formatterResolver.GetFormatterWithVerify<T[]>().Deserialize(ref reader, formatterResolver);
                        break;
                    case 1:
                        item.prev_page = formatterResolver.GetFormatterWithVerify<int?>().Deserialize(ref reader, formatterResolver);
                        break;
                    case 2:
                        item.next_page = formatterResolver.GetFormatterWithVerify<int?>().Deserialize(ref reader, formatterResolver);
                        break;
                    case 3:
                        item.total_count = reader.ReadInt32();
                        break;
                    case 4:
                        item.page = reader.ReadInt32();
                        break;
                    case 5:
                        item.per_page = reader.ReadInt32();
                        break;
                    case 6:
                        item.max_per_page = reader.ReadInt32();
                        break;
                    default:
                        break;
                }
            }

            return item;
        }
    }
}