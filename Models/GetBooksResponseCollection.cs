using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class GetBooksResponseCollection
    {
        public List<BookSummaryItem> Books { get; set; }
    }


    public class BookSummaryItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public object genre { get; set; }
    }

}
