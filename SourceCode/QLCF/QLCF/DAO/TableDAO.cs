using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCF.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; } 
        }
        public static int TableWidth = 90;
        public static int TableHeight = 90;
        private TableDAO() { }
        public void SwichTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTabel2", new object[] { id1, id2 });
        }
        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList;
        }
        public Table GetListTableByID(int id)
        {
            Table table = null;
            string query = "SELECT * FROM TableCF WHERE id = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                table = new Table(item);
                return table;
            }
            return table;
        }
        public bool InsertTable(string name)
        {
            string query = string.Format("INSERT dbo.TableCF ( name, status )VALUES  ( N'{0}', N'{1}')", name, "Trống");
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateTable(int id, string name)
        {
            string query = string.Format("UPDATE dbo.TableCF SET name = N'{0}' WHERE id = {1}" ,name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteTable(int id)
        {
            string query = string.Format("DELETE FROM dbo.BillInfo WHERE BillInfo.id = (SELECT bi.id FROM Bill AS b, BillInfo AS bi, TableCF AS t WHERE  t.id = {0} AND b.idTable = t.id AND bi.idBill = b.id) DELETE FROM dbo.Bill WHERE Bill.id = (SELECT b.id FROM Bill AS b, TableCF AS t WHERE  t.id = {0} AND b.idTable = t.id) \r\nDELETE FROM dbo.TableCF WHERE id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
