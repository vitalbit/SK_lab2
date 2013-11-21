using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SpecKurs_lab2
{
    class Section
    {
        public string path;
        public string dimByColumn;
        public string dimByRow;
        public string fixedDim;
        public string fixedField;
        public List<int> fixedId;
        public List<string> selectField;
        public List<List<int>> selectId;
        public List<string> selectDim;

        private void skip(ref XmlTextReader reader)
        {
            reader.Read();
            while (reader.NodeType != XmlNodeType.Element)
                reader.Read();
        }

        public Section(string xmlFile)
        {
            fixedId = new List<int>();
            selectField = new List<string>();
            selectId = new List<List<int>>();
            selectDim = new List<string>();
            XmlTextReader reader = new XmlTextReader(xmlFile);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "Applebd")
                    {
                        reader.MoveToNextAttribute();
                        path = reader.Value;
                    }
                    else if (reader.Name == "DimensionByColumn")
                    {
                        reader.Read();
                        dimByColumn = reader.Value;
                    }
                    else if (reader.Name == "DimensionByRow")
                    {
                        reader.Read();
                        dimByRow = reader.Value;
                    }
                    else if (reader.Name == "FixedDimension")
                    {
                        skip(ref reader);
                        fixedDim = reader.Name;
                        skip(ref reader);
                        fixedField = reader.Name;
                        reader.Read();
                        string [] split = reader.Value.Split(new Char[] { ' ' });
                        foreach (string s in split)
                            fixedId.Add(Convert.ToInt32(s));
                    }
                    else if (reader.Name == "Selection")
                    {
                        reader.Read();
                        int i = 0;
                        for (; i<2; i++)
                        {
                            skip(ref reader);
                            selectDim.Add(reader.Name);
                            skip(ref reader);
                            selectField.Add(reader.Name);
                            reader.Read();
                            string[] split = reader.Value.Split(new Char[] { ' ' });
                            selectId.Add(new List<int>());
                            foreach (string s in split)
                                selectId[i].Add(Convert.ToInt32(s));
                        }
                    }
                }
            }
        }
    }
}
