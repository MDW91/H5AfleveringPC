using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    class Program
    {
        static void Main(string[] args)
        {

            string filePath = System.IO.Path.GetFullPath("TestFile.txt");
            //indlæs tekstfil 
            Parser parser = new Parser("TestFile.txt");

            parser.FileReader();
            Console.ReadKey();

        }
    }
}
