using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MySqlHelp
{
    public class Connector
    {
        private MySqlConnector.MySqlConnection conn;
        public Connector(string host, int port, string database, string username, string password)
        {
            MySqlConnector.MySqlConnectionStringBuilder ConString = new MySqlConnector.MySqlConnectionStringBuilder
            {
                Server = host,
                Database = database,
                UserID = username,
                Password = password,
                ConnectionTimeout = 60,
                Port = 3306,
                AllowZeroDateTime = true
            };
            conn = new MySqlConnector.MySqlConnection(ConString.ConnectionString);
        }
        public MySqlConnector.MySqlConnection GetConnection() => conn;
        public MySqlConnector.MySqlConnection Connection => conn;
    }
    public class MySqlDate
    {
        private string nameVariable;
        public string NameVariable => nameVariable;
        private string typeVariable;
        public string TypeeVariable => typeVariable;
        private string dateVariable;
        public string DateVariable => dateVariable;
        public MySqlDate(string name,string type,string date)
        {
            nameVariable = name;typeVariable = type;dateVariable = date;    
        }

    }
    public class MySqlObject: IMySqlObject
    {
        public T Init<T>(List<MySqlDate> Date) where T : IMySqlObject, new()
        {
            T myTable = new T();
            PropertyInfo[] myPropertyInfo = myTable.GetType().GetProperties();
            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                foreach (var item in Date)
                {
                    if (item.NameVariable == myPropertyInfo[i].Name)
                    {
                        PropertyInfo Variable_Selected = myTable.GetType().GetProperty(item.NameVariable);
                        Variable_Selected.SetValue(myTable, item.DateVariable);
                    }

                }
            }
            return myTable;
        }
    }
    public interface IMySqlObject
    {
        
         T Init<T>(List<MySqlDate> Date) where T: IMySqlObject, new();
    }
    public class MySqlWork
    {
        private Connector conn;
        public MySqlWork(Connector conector) => conn = conector;
        public List<T> Select<T>(string sql) where T : IMySqlObject, new()
        {
            List<T> result = new List<T>();
            T Obj_Creaters = new T();
            MySqlConnector.MySqlCommand cmd = new MySqlConnector.MySqlCommand();
            conn.Connection.Open();
            cmd.Connection = conn.Connection;
            cmd.CommandText = sql;
            using (MySqlConnector.MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List< MySqlDate > dates = new List<MySqlDate> ();
                        for(int i = 0;i< reader.VisibleFieldCount; i++)
                        {
                            (string type, string date, string name) tmp;
                            tmp.name = reader.GetName(i);
                            tmp.date = reader.GetString(i);
                            tmp.type = reader.GetDataTypeName(i);
                            dates.Add(new MySqlDate(tmp.name, tmp.type, tmp.date));
                        }
                        result.Add(Obj_Creaters.Init<T>(dates));


                    }
                }
            }
            return result;
        }
    }
}
