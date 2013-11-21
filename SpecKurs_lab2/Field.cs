using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecKurs_lab2
{
    class Field
    {
        public string nameOfTable;
        public int id;
        public bool PK;
        public string name;
        public string pname;
        public string type;
        public bool visible;
        public string nameinDB;
        public bool attribute;

        public Field(string nameOfTable, int id, bool PK, string name, string pname, string type, bool visible, string nameinDB, bool attribute)
        {
            this.nameOfTable = nameOfTable;
            this.id = id;
            this.PK = PK;
            this.name = name;
            this.pname = pname;
            this.type = type;
            this.visible = visible;
            this.nameinDB = nameinDB;
            this.attribute = attribute;
        }
    }
}
