using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anime
{
    public class anime_base
    {
        
        public class anime
        {
            public string name { get; set; }
            public string series { get; set; }
            public string img { get; set; }
            public string desc { get; set; }
            public string url { get; set; }
        }

        public class anime_list
        {
            public List<anime> anime { get; set; }
        }
    }
}
