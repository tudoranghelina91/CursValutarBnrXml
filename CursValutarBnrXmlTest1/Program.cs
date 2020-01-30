using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CursValutarBnrXmlTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlReader reader = XmlReader.Create("https://www.bnr.ro/nbrfxrates.xml");

            Dictionary<string, decimal> currencyTable = new Dictionary<string, decimal>();

            reader.ReadToDescendant("Cube");

            decimal value = 0;
            string key = null;

            while(reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    value = XmlConvert.ToDecimal(reader.Value);
                    currencyTable.Add(key, value);
                }

                if (reader.NodeType == XmlNodeType.Element)
                {
                    key = reader.GetAttribute("currency");
                }
            }

            foreach (var element in currencyTable)
            {
                Console.WriteLine($"{element.Key} {element.Value}");
            }

        }
    }
}
