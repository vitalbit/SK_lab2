using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SpecKurs_lab2
{
    class Datatable
    {
        public string nameDT;
        public List<Fact> facts;

        public Datatable(ref XmlTextReader reader)
        {
            facts = new List<Fact>();
            while (reader.Name != "DataTable")
                reader.Read();
            reader.MoveToNextAttribute();
            nameDT = reader.Value;
            while (reader.Name != "Fields")
                reader.Read();
            reader.Read();
            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement || reader.Name != "Fields")
            {
                string name = "", type = "", link = "", nameinDB = "";
                name = reader.Name;
                while (reader.NodeType != XmlNodeType.EndElement || reader.Name != name)
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Name")
                        {
                            reader.Read();
                            nameinDB = reader.Value;
                        }
                        else if (reader.Name == "Type")
                        {
                            reader.Read();
                            type = reader.Value;
                        }
                        else if (reader.Name == "Link")
                        {
                            reader.Read();
                            link = reader.Value;
                        }
                    }
                }
                facts.Add(new Fact(name, nameinDB, type, link));
                reader.Read();
                reader.Read();
            }
        }
    }
}
