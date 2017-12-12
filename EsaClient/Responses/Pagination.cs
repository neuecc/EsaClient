using System;
using System.Collections.Generic;
using System.Text;

namespace EsaClient.Responses
{
    public class Pagination<T>
    {
        public T[] items { get; set; }

        public int? prev_page { get; set; }
        public int? next_page { get; set; }
        public int total_count { get; set; }
        public int page { get; set; }
        public int per_page { get; set; }
        public int max_per_page { get; set; }
    }
}
