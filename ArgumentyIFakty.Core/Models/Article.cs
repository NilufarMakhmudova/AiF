using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentyIFakty.Core.Models
{
   public class Article
    {
       public int Id { get; set; }

       public DateTime PublishedDate { get; set; }

       public string Title { get; set; }

       public string Author { get; set; }

       public string Context { get; set; }

       public string URL { get; set; }
       public string EditionFullName { get; set; }
    }
}
