using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SpecKurs_lab2
{
    class Table
    {
        public string table;
        public string tableDB;
        public string pk;
        public string parK;
        public List<Field> fields;

        private void skip(ref XmlTextReader reader)
        {
            reader.Read();
            while (reader.NodeType != XmlNodeType.Element)
                reader.Read();
        }

        public Table(ref XmlTextReader reader)
        {
            fields = new List<Field>();
            int id = 0;
            bool pk = true, visible = true, attribute = true;
            string pname = "", type = "", nameinDB = "";
            string t = reader.Name;
            this.table = t;
            while (reader.NodeType != XmlNodeType.EndElement || reader.Name != t)
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "Fields")
                    {
                        skip(ref reader);
                        while (reader.NodeType != XmlNodeType.EndElement || reader.Name != "Fields")
                        {
                            string t1 = reader.Name;
                            while (reader.NodeType != XmlNodeType.EndElement || reader.Name != t1)
                            {
                                reader.Read();
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    if (reader.Name == "Id")
                                    {
                                        reader.Read();
                                        id = Convert.ToInt32(reader.Value);
                                    }
                                    else if (reader.Name == "PK")
                                    {
                                        reader.Read();
                                        if (reader.Value == "true") pk = true;
                                        else pk = false;
                                    }
                                    else if (reader.Name == "Pname")
                                    {
                                        reader.Read();
                                        pname = reader.Value;
                                    }
                                    else if (reader.Name == "Type")
                                    {
                                        reader.Read();
                                        type = reader.Value;
                                    }
                                    else if (reader.Name == "Visible")
                                    {
                                        reader.Read();
                                        if (reader.Value == "true") visible = true;
                                        else visible = false;
                                    }
                                    else if (reader.Name == "Name")
                                    {
                                        reader.Read();
                                        nameinDB = reader.Value;
                                    }
                                    else if (reader.Name == "Attribute")
                                    {
                                        reader.Read();
                                        if (reader.Value == "true") attribute = true;
                                        else attribute = false;
                                    }
                                }
                            }
                            fields.Add(new Field(t, id, pk, t1, pname, type, visible, nameinDB, attribute));
                            reader.Read();
                            reader.Read();
                        }
                    }
                    else if (reader.Name == "TableInfo")
                    {
                        while (reader.NodeType != XmlNodeType.EndElement || reader.Name != "TableInfo")
                        {
                            reader.Read();
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                if (reader.Name == "NameDB")
                                {
                                    reader.Read();
                                    this.tableDB = reader.Value;
                                }
                                else if (reader.Name == "PK")
                                {
                                    reader.Read();
                                    this.pk = reader.Value;
                                }
                                else if (reader.Name == "Parent")
                                {
                                    reader.Read();
                                    this.parK = reader.Value;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
