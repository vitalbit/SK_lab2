using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace SpecKurs_lab2
{
    class SelectQueries
    {
        public List<string> queries;
        public List<string> idCol;
        public List<string> idRow;

        public SelectQueries(Datatable dt, Table[] tables, Section sec)
        {
            queries = new List<string>();
            idCol = new List<string>();
            idRow = new List<string>();
            string amount = "";
            foreach (Fact s in dt.facts)
                if (s.name == "Amount") amount = s.nameinDB;
            StringBuilder query = new StringBuilder("Select " + dt.nameDT + "." + amount + " from " + dt.nameDT);
            for (int j = 0; j!=tables.Length; j++)
            {
                string fk = "";
                foreach (Fact t in dt.facts)
                    if (t.link == tables[j].table) fk = t.nameinDB;
                query.AppendFormat(" inner join {0} on {1}.{2} = {3}.{4}", tables[j].tableDB, tables[j].tableDB, tables[j].pk, dt.nameDT, fk);
            }
            Table t1 = tables[0];
            foreach (Table t in tables)
                if (t.table == sec.fixedDim) t1 = t;
            Field f1 = t1.fields[0];
            foreach (Field f in t1.fields)
                if (f.name == sec.fixedField) f1 = f;
            query.AppendFormat(" where {0}.{1}={2} and ", t1.tableDB, f1.nameinDB, sec.fixedId[0]);
            Table dimCol = tables[0];
            foreach (Table t in tables)
                if (t.table == sec.dimByColumn) dimCol = t;
            Field vis = dimCol.fields[0];
            foreach (Field f in dimCol.fields)
                if (f.visible) vis = f;
            for (int colNum = 0; colNum != sec.selectId[0].Count; colNum++)
            {
                SQLiteConnection appleConnection = new SQLiteConnection(String.Format("Data Source={0}", sec.path));
                appleConnection.Open();
                SQLiteCommand appleCommand = new SQLiteCommand(appleConnection);
                appleCommand.CommandText = "select " + vis.nameinDB + " from " + dimCol.tableDB + " where " + dimCol.pk + "=" + sec.selectId[0][colNum];
                SQLiteDataReader dataReader = appleCommand.ExecuteReader();
                DataTable data = new DataTable();
                data.Load(dataReader);
                dataReader.Close();
                idCol.Add(data.Rows[0][0].ToString());
            }
            Table dimRow = tables[0];
            foreach (Table t in tables)
                if (t.table == sec.dimByRow) dimRow = t;
            vis = dimRow.fields[0];
            foreach (Field f in dimRow.fields)
                if (f.visible) vis = f;
            for (int rowNum = 0; rowNum != sec.selectId[1].Count; rowNum++)
            {
                SQLiteConnection appleConnection = new SQLiteConnection(String.Format("Data Source={0}", sec.path));
                appleConnection.Open();
                SQLiteCommand appleCommand = new SQLiteCommand(appleConnection);
                appleCommand.CommandText = "select " + vis.nameinDB + " from " + dimRow.tableDB + " where " + dimRow.pk + "=" + sec.selectId[1][rowNum];
                SQLiteDataReader dataReader = appleCommand.ExecuteReader();
                DataTable data = new DataTable();
                data.Load(dataReader);
                dataReader.Close();
                idRow.Add(data.Rows[0][0].ToString());
            }
            for (int rowNum = 0; rowNum != sec.selectId[1].Count; rowNum++)
            {
                for (int colNum = 0; colNum != sec.selectId[0].Count; colNum++)
                {
                    StringBuilder resQuery = new StringBuilder(query.ToString());
                    bool prov = true;
                    foreach (Table t in tables)
                    {
                        if (t.table != sec.fixedDim && prov)
                        {
                            resQuery.AppendFormat("{0}.{1}={2} and ", t.tableDB, t.pk, sec.selectId[0][colNum]);
                            prov = false;
                        }
                        else if (t.table != sec.fixedDim) resQuery.AppendFormat("{0}.{1}={2};", t.tableDB, t.pk, sec.selectId[1][rowNum]);
                    }
                    queries.Add(resQuery.ToString());
                }
            }
        }
    }
}
