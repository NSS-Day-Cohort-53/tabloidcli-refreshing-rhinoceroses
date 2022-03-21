using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabloidCLI.Models
{
    public class Journal
    {
        public string Title {get; set;}
        public string Content { get; set; }
        public DateTime CreateDateTime { get; set; }

        public override string ToString()
        {
            return $"{Title} created on {CreateDateTime} : {Content}";
        }
    }
}
