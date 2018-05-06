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
            String str1 = "d4";
            Thread.Sleep(4000);
            Console.Write(str1 + Environment.NewLine);
            Console.Write(Environment.NewLine);
            Console.ReadLine();
        }
        
    }
}
