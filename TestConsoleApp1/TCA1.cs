using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TestConsoleApp1
{
    class TCA1
    {
        static void Main()
        {
            Thread.Sleep(4000);
            Console.Write("d4" + Environment.NewLine);
            Thread.Sleep(2000);

            Console.Write("e4" + Environment.NewLine);
            Thread.Sleep(500);

            Console.Write("e5" + Environment.NewLine);
            Console.ReadLine();
        }
        
    }
}
