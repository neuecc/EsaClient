using EsaClient.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EsaClient.Toolkit
{
    public static class EsaClientExtensions
    {
        static readonly Regex postIdRegex = new Regex(@"https://(.+)\.esa\.io/posts/(.+)", RegexOptions.Compiled);

        class From
        {
            public string TeamName { get; set; }
            public int PostNumber { get; set; }
        }

        public static async Task PostExportAsync(this EsaClient client, string[] fromUrls, string toTeamName,string toCategoryPre = "", bool userAsEsaBot = false)
        {
            var list = new List<From>();
            foreach (var item in fromUrls)
            {
                var match = postIdRegex.Match(item);
                var teamName = match.Groups[1].Value;
                var number = int.Parse(match.Groups[2].Value);
                list.Add(new From { TeamName = teamName, PostNumber = number });
            }

            if (client.Logger.IsEnabled(LogLevel.Debug))
            {
                client.Logger.LogDebug("Start Export, Count:" + list.Count);
            }

            foreach (var item in list)
            {
                var post = await client.GetPostAsync(item.TeamName, item.PostNumber, PostIncludes.None);

                if (client.Logger.IsEnabled(LogLevel.Debug))
                {
                    client.Logger.LogDebug("Get Complete: {0} - {1}/{2}", item.TeamName, post.category, post.name);
                }

                var newPost = await client.CreateNewPostAsync(toTeamName, new NewPost
                {
                    body_md = post.body_md,
                    category = string.IsNullOrEmpty(toCategoryPre) ? post.category : (toCategoryPre.TrimEnd('/') + "/" + post.category.TrimStart('/')),
                    message = post.message,
                    name = post.name,
                    tags = post.tags,
                    wip = post.wip,
                    user = (userAsEsaBot) ? "esa_bot" : null
                });

                if (client.Logger.IsEnabled(LogLevel.Debug))
                {
                    client.Logger.LogDebug("Post Complete: {0} - {1}/{2}", toTeamName, newPost.category, newPost.name);
                }
            }
        }
    }
}
