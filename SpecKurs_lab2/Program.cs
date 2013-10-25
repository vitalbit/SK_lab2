using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SQLite;
using System.Data;

namespace SpecKurs_lab2
{
    class Program
    {
        const int N = 20;

        struct allTable
        {
            public string table;
            public string description;
            public string column;
            public List<int> val;
        };

        class Cube
        {
            public Dictionary<int, string> data;
            public List<string> sql;

            public Cube()
            {
                data = new Dictionary<int, string>();
                sql = new List<string>();
            }

            public bool addSQL(string sqlquery)
            {
                sql.Add(sqlquery);
                return true;
            }
        }

        class Section
        {
            public string path;
            public allTable[] fixedTable;
            public int fixedKol;
            public allTable[] factTable;
            public int factKol;
            public allTable[] selectTable;
            public int selectKol;

            public Section()
            {
                fixedTable = new allTable[N];
                for (int i = 0; i != N; i++)
                    fixedTable[i].val = new List<int>();
                fixedKol = 0;
                factTable = new allTable[N];
                factKol = 0;
                selectTable = new allTable[N];
                selectKol = 0;
            }

            public void xmlReadTable(string sost, ref XmlTextReader reader, ref allTable[] table, ref int kol)
            {
                reader.Read();
                reader.Read();
                while (reader.Name != sost)
                {
                    string t = reader.Name;
                    if (t != "")
                    {
                        while (reader.Read() && reader.Name != t)
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                table[kol].table = t;
                                table[kol].column = reader.Name;
                                reader.MoveToNextAttribute();
                                table[kol].description = reader.Value;
                            }
                            else if (reader.NodeType == XmlNodeType.Text && reader.Value != "")
                            {
                                string[] split = reader.Value.Split(new Char[] { ' ' });
                                foreach (string s in split)
                                    table[kol].val.Add(Convert.ToInt32(s));
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement)
                                kol++;
                        }
                    }
                    reader.Read();
                }
            }
        }

        static void Main(string[] args)
        {
            Section section1 = new Section();
            XmlTextReader reader = new XmlTextReader("xml\\sectionApple.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Appledb")
                {
                    reader.MoveToNextAttribute();
                    section1.path = reader.Value;
                }
                if (reader.NodeType == XmlNodeType.Element && (reader.Name == "Fixed" || reader.Name == "Selection" || reader.Name == "Fact"))
                {
                    if (reader.Name == "Fixed")
                        section1.xmlReadTable("Fixed", ref reader, ref section1.fixedTable, ref section1.fixedKol);
                    else if (reader.Name == "Selection")
                        section1.xmlReadTable("Selection", ref reader, ref section1.selectTable, ref section1.selectKol);
                    else if (reader.Name == "Fact")
                        section1.xmlReadTable("Fact", ref reader, ref section1.factTable, ref section1.factKol);
                }
            }
            StringBuilder sql = new StringBuilder("Select ");
            for (int i = 0; i != section1.selectKol; i++)
            {
                if (i + 1 != section1.selectKol) sql.AppendFormat("{0}, ", section1.selectTable[i].column);
                else sql.Append(section1.selectTable[i].column);
            }
            sql.AppendFormat(" from {0}", section1.factTable[0].table);
            for (int i = 0; i != section1.factKol; i++)
                sql.AppendFormat(" inner join {0} on {1}.{2} = {3}.{4}", section1.factTable[i].description, section1.factTable[i].table,
                    section1.factTable[i].column, section1.factTable[i].description, section1.factTable[i].column);
            for (int i = 0; i != section1.fixedKol; i++)
                sql.AppendFormat(" where {0}.{1} = {2}", section1.fixedTable[i].table, section1.fixedTable[i].column, section1.fixedTable[i].val[1]);
            SQLiteConnection appleConnection = new SQLiteConnection(String.Format("Data Source={0}", section1.path));
            appleConnection.Open();
            SQLiteCommand appleCommand = new SQLiteCommand(appleConnection);
            appleCommand.CommandText = sql.ToString();
            SQLiteDataReader dataReader = appleCommand.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dataReader);
            dataReader.Close();
            appleConnection.Close();

            for (int i = 0; i != section1.selectKol; i++)
                Console.Write("{0, -20}", section1.selectTable[i].description);
            Console.WriteLine();
            for (int i = 0; i != dt.Rows.Count; i++)
            {
                for (int j = 0; j != section1.selectKol; j++)
                    Console.Write("{0, -20}", dt.Rows[i][j]);
                Console.WriteLine();
            }
        }
    }
}
