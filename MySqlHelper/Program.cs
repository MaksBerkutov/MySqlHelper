using MySqlHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MySqlHelper
{
    class MyTable : MySqlObject
    {
        public string OwnerSurname { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerStreet { get; set; }
        public string OwnerBlock { get; set; }
        public override string ToString() => $"OwnerSurname: {OwnerSurname} | OwnerFirstName: {OwnerFirstName} | OwnerStreet: {OwnerStreet} | OwnerBlock: {OwnerBlock}";


    }
    internal class Program
    {
        static void Main(string[] args)
        {
            MySqlHelp.Connector main_conn = new Connector("192.168.1.10", 3306, "tetsing", "root", "root");
            MySqlHelp.MySqlWork work = new MySqlWork(main_conn);
            MyTable Res = new MyTable();
            var res = work.Select<MyTable>("SELECT * FROM test");
            
            foreach(var item in res)
                Console.WriteLine(item);
            Console.ReadKey();
         
        }
    }
}
