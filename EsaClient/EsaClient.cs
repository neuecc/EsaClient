﻿using EsaClient.Json;
using EsaClient.Responses;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EsaClient
{
    public class EsaClient
    {
        const string EndPointBase = "https://api.esa.io/v1/";

        readonly HttpClient httpClient;

        public EsaClient(string token)
            : this(token, null)
        {
        }

        public EsaClient(string token, HttpMessageHandler innerHandler)
        {
            httpClient = new HttpClient(new BearerAuthenticationMessageHandler(token, innerHandler));
        }

        StringBuilder BuildStringBuilderWithTeamName(string teamName)
        {
            var sb = new StringBuilder();
            sb.Append(EndPointBase + "teams/" + teamName);
            return sb;
        }

        async Task<T> DeserializeAsync<T>(Task<byte[]> byteArray)
        {
            var x = await byteArray;
            return Utf8Json.JsonSerializer.Deserialize<T>(x, EsaJsonFormatterResolver.Instance);
        }

        Task<T> ReadMessage<T>(string url)
        {
            return ReadMessage<T>(httpClient.GetAsync(url));
        }

        async Task<T> ReadMessage<T>(Task<HttpResponseMessage> message)
        {
            var m = await message;
            if (!m.IsSuccessStatusCode)
            {
                var error = await DeserializeAsync<ErrorResponse>(m.Content.ReadAsByteArrayAsync()).ConfigureAwait(false);
                throw new EsaClientErrorException(error.error, error.message);
            }
            return await DeserializeAsync<T>(m.Content.ReadAsByteArrayAsync()).ConfigureAwait(false);
        }

        async Task<T[]> ReadPaginiationMesasge<T>(string url)
        {
            var p = await ReadMessage<Pagination<T>>(httpClient.GetAsync(url)).ConfigureAwait(false);

            var result = p.items;
            while (p.next_page != null)
            {
                p = await ReadMessage<Pagination<T>>(httpClient.GetAsync(url)).ConfigureAwait(false);
                ConcatArray(ref result, p.items);
            }

            return result;
        }

        void ConcatArray<T>(ref T[] array, T[] second)
        {
            Array.Resize(ref array, array.Length + second.Length);
            Array.Copy(array, array.Length, second, 0, second.Length);
        }

        void AppendInclude(StringBuilder sb, PostIncludes includes)
        {
            var requireComma = false;
            if (includes.HasFlag(PostIncludes.Comments))
            {
                if (requireComma)
                {
                    sb.Append(",");
                }
                sb.Append("comments");
                requireComma = true;
            }
            if (includes.HasFlag(PostIncludes.CommentsStargazers))
            {
                if (requireComma)
                {
                    sb.Append(",");
                }
                sb.Append("comments.stargazers");
                requireComma = true;
            }
            if (includes.HasFlag(PostIncludes.Stargazers))
            {
                if (requireComma)
                {
                    sb.Append(",");
                }
                sb.Append("stargazers ");
                requireComma = true;
            }
        }

        void AppendSort(StringBuilder sb, PostSort sort)
        {
            switch (sort)
            {
                case PostSort.Updated:
                    sb.Append("updated");
                    break;
                case PostSort.Created:
                    sb.Append("created");
                    break;
                case PostSort.Stars:
                    sb.Append("stars");
                    break;
                case PostSort.Watches:
                    sb.Append("watches");
                    break;
                case PostSort.Comments:
                    sb.Append("comments");
                    break;
                case PostSort.BestMatch:
                    sb.Append("best_match");
                    break;
                default:
                    break;
            }
        }

        void AppendOrder(StringBuilder sb, PostOrder order)
        {
            switch (order)
            {
                case PostOrder.Desc:
                    sb.Append("desc");
                    break;
                case PostOrder.Asc:
                    sb.Append("asc");
                    break;
                default:
                    break;
            }
        }

        // APIs

        public Task<Team[]> GetTeamsAsync()
        {
            var url = EndPointBase + "teams";
            return ReadPaginiationMesasge<Team>(url);
        }

        public Task<Team> GetTeamNameAsync(string teamName)
        {
            var url = EndPointBase + "teams/" + teamName;
            return ReadMessage<Team>(url);
        }

        public Task<Post[]> GetPostsAsync(string teamName, string query, PostIncludes include = PostIncludes.None, PostSort sort = PostSort.Updated, PostOrder order = PostOrder.Desc)
        {
            var url = BuildStringBuilderWithTeamName(teamName);
            url.Append("/posts");
            url.Append("?q="); url.Append(query);
            url.Append("&include="); AppendInclude(url, include);
            url.Append("&sort="); AppendSort(url, sort);
            url.Append("&order="); AppendOrder(url, order);

            return ReadPaginiationMesasge<Post>(url.ToString());
        }

        public Task<Post> GetPostAsync(string teamName, int postNumber, PostIncludes include = PostIncludes.None)
        {
            var url = BuildStringBuilderWithTeamName(teamName);
            url.Append("/posts/");
            url.Append(postNumber);
            AppendInclude(url, include);

            return ReadMessage<Post>(httpClient.GetAsync(url.ToString()));
        }
    }
}
