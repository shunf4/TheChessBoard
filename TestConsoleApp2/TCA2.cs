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
            Console.Write("Please input something." + Environment.NewLine);
            String str0 = Console.ReadLine();
            String str2 = "TCA2: you just inputed : " + str0;
            Console.Write(str2 + Environment.NewLine);

            Console.Write("Who do you want me to fuck : " + Environment.NewLine);
            Console.Out.Flush();
            String str3 = Console.ReadLine();
            Console.Write("Fuck " + str3 + Environment.NewLine);
            Console.Out.Flush();
        }
    }
}
