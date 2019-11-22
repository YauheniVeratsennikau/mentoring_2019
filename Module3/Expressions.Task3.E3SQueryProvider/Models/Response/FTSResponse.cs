using System.Collections.Generic;

namespace Expressions.Task3.E3SQueryProvider.Models.Response
{
    public class FTSResponse<T> where T : class
    {
        public int Total { get; set; }

        public List<ResponseItem<T>> Items { get; set; }
    }
}
