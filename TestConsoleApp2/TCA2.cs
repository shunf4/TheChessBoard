using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp2
{
    class TCA2
    {
        static void Main()
        {
            String str1 = "Please input something." + Environment.NewLine;
            Console.Write(str1);

            String str0 = Console.ReadLine();
            String str2 = "TCA2: you just inputed : " + str0;
            Console.Write(str2 + Environment.NewLine);
        }
    }
}
