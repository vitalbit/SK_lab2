using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data.SQLite;
using System.Data;

namespace CreateXML
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя файла:");
            string xmlName = Console.ReadLine();
            string dimRow, dimCol;
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            XmlWriter xtw = XmlWriter.Create(xmlName, xws);
            xtw.WriteStartDocument();
            Console.WriteLine("Введите имя базы данных:");
            string dbName = Console.ReadLine();
            xtw.WriteStartElement("Applebd");
            xtw.WriteAttributeString("database", dbName);
            ArrayList tables = new ArrayList(3);
            tables.Add("Dept");
            tables.Add("Apple");
            tables.Add("Sale_date");
            Console.WriteLine("Введите измерение по горизонтали:\n1 - {0}\n2 - {1}\n3 - {2}", tables[0], tables[1], tables[2]);
            int num = 0;
            while (num == 0)
            {
                num = Convert.ToInt32(Console.ReadLine());
                if (num <= 0 || num >= 4)
                {
                    Console.WriteLine("Неверно введен параметр! Введите еще раз:");
                    num = 0;
                }
            }
            dimCol = tables[num - 1].ToString();
            xtw.WriteElementString("DimensionByColumn", tables[num - 1].ToString());
            tables.RemoveAt(num - 1);
            Console.WriteLine("Введите измерение по вертикали:\n1 - {0}\n2 - {1}", tables[0], tables[1]);
            num = 0;
            while (num == 0)
            {
                num = Convert.ToInt32(Console.ReadLine());
                if (num <= 0 || num >= 3)
                {
                    Console.WriteLine("Неверно введен параметр! Введите еще раз:");
                    num = 0;
                }
            }
            dimRow = tables[num - 1].ToString();
            xtw.WriteElementString("DimensionByRow", tables[num - 1].ToString());
            tables.RemoveAt(num - 1);
            xtw.WriteStartElement("FixedDimension");
            dbName = "..\\..\\..\\..\\SpecKurs_lab2\\bin\\Debug\\database\\" + dbName;
            SQLiteConnection appleConn = new SQLiteConnection(String.Format("Data Source={0}", dbName));
            appleConn.Open();
            SQLiteCommand appleCom = new SQLiteCommand(appleConn);
            appleCom.CommandText = "Select * from " + tables[0].ToString();
            SQLiteDataReader reader = appleCom.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            ArrayList fixId = new ArrayList();
            for (int i = 0; i != dt.Rows.Count; i++)
                fixId.Add(dt.Rows[i][0]);
            Console.Write("Введите фиксированные id через пробел (");
            for (int i = 0; i != fixId.Count; i++)
                if (i == 0) Console.Write(fixId[i]);
                else Console.Write(",{0}", fixId[i]);
            Console.WriteLine("):");
            string idParam = Console.ReadLine();
            xtw.WriteStartElement(tables[0].ToString());
            xtw.WriteElementString(dt.Columns[0].ToString(), idParam);
            xtw.WriteEndElement();
            xtw.WriteEndElement();
            xtw.WriteStartElement("Selection");

            appleCom.CommandText = "Select * from " + dimCol;
            reader = appleCom.ExecuteReader();
            dt = new DataTable();
            dt.Load(reader);
            fixId = new ArrayList();
            for (int i = 0; i != dt.Rows.Count; i++)
                fixId.Add(dt.Rows[i][0]);
            Console.Write("Введите id для горизонтального среза через пробел (");
            for (int i = 0; i != fixId.Count; i++)
                if (i == 0) Console.Write(fixId[i]);
                else Console.Write(",{0}", fixId[i]);
            Console.WriteLine("):");
            idParam = Console.ReadLine();
            xtw.WriteStartElement(dimCol);
            xtw.WriteElementString(dt.Columns[0].ToString(), idParam);
            xtw.WriteEndElement();

            appleCom.CommandText = "Select * from " + dimRow;
            reader = appleCom.ExecuteReader();
            dt = new DataTable();
            dt.Load(reader);
            fixId = new ArrayList();
            for (int i = 0; i != dt.Rows.Count; i++)
                fixId.Add(dt.Rows[i][0]);
            Console.Write("Введите id для вертикального среза через пробел (");
            for (int i = 0; i != fixId.Count; i++)
                if (i == 0) Console.Write(fixId[i]);
                else Console.Write(",{0}", fixId[i]);
            Console.WriteLine("):");
            idParam = Console.ReadLine();
            xtw.WriteStartElement(dimRow);
            xtw.WriteElementString(dt.Columns[0].ToString(), idParam);
            xtw.WriteEndElement();
            xtw.Close();
        }
    }
}
