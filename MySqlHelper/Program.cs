using MySqlHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MySqlHelper
{
    
    class  Product: MySqlObject
    {
        public string maker { get; set; }
        public string model { get; set; }
        public string type { get; set; }
    }
    class Prinetr : MySqlObject
    {
        public int code { get; set; }
        public string model { get; set; }
        public bool color { get; set; }
        public string type { get; set; }
        public double price { get; set; }   
    }
    class PC : MySqlObject
    {
        public int code { get; set; }
        public string model { get; set; }
        public short speed { get; set; }
        public short ram { get; set; }
        public int hd { get; set; }
        public string cd { get; set; }
        public double price { get; set; }
    }
    class Laptop : MySqlObject
    {
        public int code { get; set; }
        public string model { get; set; }
        public short speed { get; set; }
        public short ram { get; set; }
        public int hd { get; set; }
        public double price { get; set; }
        public float screen { get; set; }

    }
    class Database
    {
        List<Product> Products;
        List<Prinetr> Prinetrs;
        List<PC> PCs;
        List<Laptop> Laptops;

        public Database(List<Product> products, List<Prinetr> prinetrs, List<PC> pCs, List<Laptop> laptops)
        {
            Products = products;
            Prinetrs = prinetrs;
            PCs = pCs;
            Laptops = laptops;
        }
        public void TASK0()
        {

        }
        public void TASK1()
        {
            var Users = Products.FindAll(p => p.maker == "A");
            double avg = 0;int counter = 0;
            Users.ForEach(item =>
            {
                if(PCs.FindAll(p=>p.model == item.model).Count > 0)
                {
                    var res = PCs.Find(p => p.model == item.model);
                    avg = res.price;counter++;
                }
            });
            Console.WriteLine(avg/counter);
        }
        public void TASK2()
        {
            List<(string, bool, bool, bool)> res = new List<(string, bool, bool, bool)>();
            foreach(var item in Products)
            {
                if(res.FindAll(p=>p.Item1==item.maker).Count == 0)
                {
                    (bool, bool, bool) tmp;
                    tmp.Item1 = false;
                    tmp.Item2 = false;
                    tmp.Item3 = false;
                    var allModelMakers = Products.FindAll(items => item.maker == items.maker);
                    allModelMakers.ForEach(items =>
                    {
                        if (Prinetrs.FindAll(p => p.model == items.model).Count > 0) tmp.Item3 = true;
                        if (PCs.FindAll(p => p.model == items.model).Count > 0) tmp.Item1 = true;
                        if (Laptops.FindAll(p => p.model == items.model).Count > 0) tmp.Item2 = true;
                    });
                    res.Add((item.maker, tmp.Item1, tmp.Item2, tmp.Item3));
                }
            }
            Console.WriteLine("Maker | PC | Laptop | Printer");
            foreach(var i in res)
            {
                Console.WriteLine($"{i.Item1} | {i.Item2} | {i.Item3} | {i.Item4} |");
            }

        }
        public void TASK3()
        {
            for (int i = 0, count = 0; i < PCs.Count; i++, count = 0)
            {
                for (int j = 0; j < PCs.Count; j++)
                    if (j != i && PCs[j].hd == PCs[i].hd) count++;
                if (count > 1) Console.WriteLine(PCs[i].hd);


            }
        }
        public void TASK4()
        {
            for (int i = 0, count = 0; i < PCs.Count; i++, count = -1)
            {
                for (int j = 0; j < PCs.Count; j++)
                    if (j != i && PCs[j].ram == PCs[i].ram && PCs[i].speed == PCs[i].speed) count = j;
                if (count !=  -1)
                {
                    Console.WriteLine($"{PCs[i].model} | {PCs[count].model}");
                }


            }
        }
        public void TASK5()
        {
            foreach (var item in this.Laptops)
                if (item.hd > 10)
                    foreach (var itemIn in this.Products)
                        if (item.model == itemIn.model)
                        {
                            Console.WriteLine($"{item.model}|{item.speed}");
                            break;
                        }
        }
        public void TASK6()
        {
            var res = (from item in Prinetrs
                       orderby item.price
                       select item).ToList();
            foreach (var item in res)
            
                if(item.color)
                {
                    Console.WriteLine($"{item.model} | {item.price} | {Products.Find(p=>p.model==item.model).maker}");
                    break;
                }
            


        }
        public void TASK7()
        {
            List<Product> result = new List<Product>();
           foreach(var item in this.Products)
            {
                var res = Products.FindAll(p => p.maker == item.maker);
                (bool lap, bool pc) tmp;
                tmp.lap = false;
                tmp.pc = false;
                foreach (var itemIn in res)
                {
                    if (Laptops.FindAll(x => x.model == itemIn.model).Count > 0)
                    {
                        if (Laptops.Find(x => x.model == itemIn.model).speed < 750) tmp.lap = true;
                    }
                    if (PCs.FindAll(x => x.model == itemIn.model).Count > 0)
                    {
                        if (PCs.Find(x => x.model == itemIn.model).speed < 750) tmp.pc = true;
                    }
                }
                if(tmp.lap&&tmp.pc&&result.FindAll(p=>p.maker==item.maker).Count==0)result.Add(item);
            }
            foreach (var res in result) Console.WriteLine(res.maker);
               
        }
        public void TASK8()
        {
            double avg = 0; int counter = 0;
            PCs.ForEach(p => {
                if (Products.Find(x => x.model == p.model).maker == "A")
                {
                    avg += p.price;counter++;
                }
                });
            Console.WriteLine(avg / counter);

        }
        public void TASK9()
        {
            double avg = 0;
            PCs.ForEach(p =>avg+=p.hd);
            Console.WriteLine(avg/PCs.Count);

        }
        public void TASK10()
        {
            Products.ForEach(p => {
                if (Products.FindAll(x => p.maker == x.maker).Count == 1) Console.WriteLine(p.maker);
            });

        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            MySqlHelp.Connector main_conn = new Connector("127.0.0.1", 3306, "finall", "root", "root");
            MySqlHelp.MySqlWork work = new MySqlWork(main_conn);
            
            MySqlHelp.Experemental ex = new Experemental(main_conn);
            MySqlHelp.ExperementalHelper exH = new ExperementalHelper(ex.CteClass("laptop"));
            Console.WriteLine(exH[(exH[("code", 2)], "hd")].ToString());
            var 
            Console.ReadKey();

           
           
            
         
        }
    }
}
