using System;

namespace EsaClient.Responses
{
    public class ErrorResponse
    {
        public string error { get; set; }
        public string message { get; set; }
    }

    public class Team
    {
        public string name { get; set; }
        public string privacy { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
    }

    public class CreatedBy
    {
        public string name { get; set; }
        public string screen_name { get; set; }
        public string icon { get; set; }
    }

    public class UpdatedBy
    {
        public string name { get; set; }
        public string screen_name { get; set; }
        public string icon { get; set; }
    }

    public class Post
    {
        public int number { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public bool wip { get; set; }
        public string body_md { get; set; }
        public string body_html { get; set; }
        public DateTime created_at { get; set; }
        public string message { get; set; }
        public string url { get; set; }
        public DateTime updated_at { get; set; }
        public string[] tags { get; set; }
        public string category { get; set; }
        public int revision_number { get; set; }
        public CreatedBy created_by { get; set; }
        public UpdatedBy updated_by { get; set; }
        public string kind { get; set; }
        public int comments_count { get; set; }
        public int tasks_count { get; set; }
        public int done_tasks_count { get; set; }
        public int stargazers_count { get; set; }
        public int watchers_count { get; set; }
        public bool star { get; set; }
        public bool watch { get; set; }

        public override string ToString()
        {
            return category + "/" + name;
        }
    }

    public class NewPostRequest
    {
        public NewPost post { get; set; }
    }

    public class NewPost
    {
        public string name { get; set; }
        public string body_md { get; set; }
        public string[] tags { get; set; }
        public string category { get; set; }
        public bool wip { get; set; }
        public string message { get; set; }
        public string user { get; set; }
    }
}
