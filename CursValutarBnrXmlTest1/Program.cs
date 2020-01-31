using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace CursValutarBnrXmlTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlReaderSettings settings = new XmlReaderSettings();

            // Xsd pentru validare
            settings.Schemas.Add("http://www.bnr.ro/xsd", "https://www.bnr.ro/xsd/nbrfxrates.xsd");
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.MaxCharactersInDocument = 2000;

            // handler ce se ocupa de erori de validare
            settings.ValidationEventHandler += ValidationEventHandler;

            // Citeste un Xml Valid
            XmlReader reader = XmlReader.Create("https://www.bnr.ro/nbrfxrates.xml", settings);

            //// Xml Invalid
            //XmlReader reader = XmlReader.Create("invalidTest.xml", settings);

            Dictionary<string, decimal> currencyTable = new Dictionary<string, decimal>();

            try
            {
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
                        if (reader.AttributeCount == 2)
                        {
                            key = reader.GetAttribute("multiplier") + " " + reader.GetAttribute("currency");
                        }

                        else
                        {
                            key = reader.GetAttribute("currency");
                        }
                    }
                }

                foreach (var element in currencyTable)
                {
                    Console.WriteLine($"{element.Key} {element.Value}");
                }
            }

            catch
            {
                Console.WriteLine("Invalid XML file!");
            }
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            throw args.Exception;
        }
    }
}
