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
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path to your xml file or nothing to take xml file from \"xml//AppleSection.xml\":");
            string pathxml = Console.ReadLine();
            Section section1;
            if (pathxml == "") section1 = new Section("xml//AppleSection.xml");
            else section1 = new Section(pathxml);
            Console.WriteLine(section1.path);
            XmlTextReader xtr = new XmlTextReader("xml//xmldb.xml");
            Table [] tb = new Table[3];
            string [] tablesName = {"Apple", "Dept", "Sale_date"};
            for (int i = 0; i!=3; i++)
            {
                while (xtr.Name != tablesName[i])
                    xtr.Read();
                tb[i] = new Table(ref xtr);
            }
            Datatable dt = new Datatable(ref xtr);
            SelectQueries sql = new SelectQueries(dt, tb, section1);
            SQLiteConnection appleConnection = new SQLiteConnection(String.Format("Data Source={0}", section1.path));
            appleConnection.Open();
            SQLiteCommand appleCommand = new SQLiteCommand(appleConnection);
            Console.Write("{0, -20} |", ' ');
            for (int i = 0; i!=sql.idCol.Count; i++)
                Console.Write("{0, -20} |", sql.idCol[i]);
            Console.WriteLine();
            int i2 = 0;
            for (int j = 0; j != sql.idRow.Count; j++)
            {
                Console.Write("{0, -20} |", sql.idRow[j]);
                for (int i = 0; i != sql.idCol.Count; i++)
                {
                    appleCommand.CommandText = sql.queries[i2];
                    SQLiteDataReader dataReader = appleCommand.ExecuteReader();
                    DataTable data = new DataTable();
                    data.Load(dataReader);
                    dataReader.Close();
                    if (data.Rows.Count != 0) Console.Write("{0, -20} |", data.Rows[0][0]);
                    else Console.Write("{0, -20} |", 0);
                    i2++;
                }
                Console.WriteLine();
            }
            appleConnection.Close();
            Console.ReadKey();
        }
    }
}
