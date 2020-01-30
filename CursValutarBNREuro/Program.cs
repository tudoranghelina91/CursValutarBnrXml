using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CursValutarBNREuro
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlReader reader = XmlReader.Create("https://www.bnr.ro/nbrfxrates.xml");

            reader.ReadToDescendant("Cube");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.GetAttribute("currency") == "EUR")
                    {
                        Console.WriteLine(reader.GetAttribute("currency"));
                        Console.WriteLine(reader.ReadInnerXml());
                        break;
                    }
                }
            }
        }
    }
}
