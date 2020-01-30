using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace CursValutarBNREuro
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

            // handler ce se ocupa de erori de validare
            settings.ValidationEventHandler += ValidationEventHandler;

            // Citeste un Xml Valid
            XmlReader reader = XmlReader.Create("https://www.bnr.ro/nbrfxrates.xml", settings);

            //// Xml Invalid
            //XmlReader reader = XmlReader.Create("invalidTest.xml", settings);

            try
            {
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
