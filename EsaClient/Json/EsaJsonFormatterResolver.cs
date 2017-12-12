using EsaClient.Responses;
using System;
using Utf8Json;

namespace EsaClient.Json
{
    public class EsaJsonFormatterResolver : IJsonFormatterResolver
    {
        public static readonly IJsonFormatterResolver Instance = new EsaJsonFormatterResolver();

        EsaJsonFormatterResolver()
        {
        }

        public IJsonFormatter<T> GetFormatter<T>()
        {
            return Cache<T>.formatter;
        }

        static object GetFormatter(Type type)
        {
            if (type == typeof(Pagination<Team>))
            {
                return new PaginationItemFormatter<Team>("teams");
            }
            else if (type == typeof(Pagination<Post>))
            {
                return new PaginationItemFormatter<Post>("posts");
            }
            return null;
        }

        static class Cache<T>
        {
            public static IJsonFormatter<T> formatter;

            static Cache()
            {
                formatter = (IJsonFormatter<T>)GetFormatter(typeof(T));
                if (formatter == null)
                {
                    // fallback
                    formatter = Utf8Json.Resolvers.StandardResolver.Default.GetFormatter<T>();
                }
            }
        }
    }
}