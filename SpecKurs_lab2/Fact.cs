using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecKurs_lab2
{
    class Fact
    {
        public string name;
        public string nameinDB;
        public string type;
        public string link;

        public Fact(string name, string nameinDB, string type, string link)
        {
            this.name = name;
            this.nameinDB = nameinDB;
            this.type = type;
            this.link = link;
        }
    }
}
