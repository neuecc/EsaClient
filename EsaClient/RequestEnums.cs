using System;

namespace EsaClient
{
    [Flags]
    public enum PostIncludes
    {
        None = 0,
        Comments = 1,
        CommentsStargazers = 2,
        Stargazers = 4
    }

    public enum PostSort
    {
        Updated,
        Created,
        Stars,
        Watches,
        Comments,
        BestMatch
    }

    public enum PostOrder
    {
        Desc,
        Asc
    }
}
