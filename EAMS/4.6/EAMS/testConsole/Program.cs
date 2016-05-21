using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using strategyLib;

namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            strategyDAL sDal = new strategyDAL();
            cStrategy s = new strategyLib.cStrategy(){ main = new Main() { cMemo = "a" }};
            sDal.Save(s);
             s = new strategyLib.cStrategy() { main = new Main() { cMemo = "b" } };
            sDal.Save(s);
             s = new strategyLib.cStrategy() { main = new Main() { cMemo = "c" } };
            sDal.Save(s);

            IEnumerable<cStrategy> list = sDal.getStrategys(null,null);
            foreach (cStrategy i in list)
            {
                Console.WriteLine(i.main.ID.ToString()+"."+i.main.cMemo);
            }
            Console.ReadLine();
        }
    }
}
