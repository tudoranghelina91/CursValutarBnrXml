using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace CursValutarBnrWinForms
{
    class XmlParser
    {
        public async Task<BindingList<Currency>> GetCurrencyTable()
        {
            XmlReaderSettings settings = new XmlReaderSettings();

            // Xsd pentru validare
            settings.Schemas.Add("http://www.bnr.ro/xsd", "https://www.bnr.ro/xsd/nbrfxrates.xsd");
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.Async = true;

            settings.ValidationEventHandler += ValidationEventHandler;

            XmlReader reader = XmlReader.Create("https://www.bnr.ro/nbrfxrates.xml", settings);

            BindingList<Currency> currencyTable = new BindingList<Currency>();

            try
            {
                reader.ReadToDescendant("Cube");

                Currency currency = new Currency();

                while (await reader.ReadAsync())
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        currency.Value = XmlConvert.ToDecimal(reader.Value);
                    }

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.AttributeCount == 2)
                        {
                            currency.Multiplier = XmlConvert.ToInt32(reader.GetAttribute("multiplier"));
                            currency.Name = reader.GetAttribute("currency");
                        }

                        else
                        {
                            currency.Name = reader.GetAttribute("currency");
                        }
                    }

                    if (!string.IsNullOrEmpty(currency.Name) && currency.Value != 0)
                    {
                        currencyTable.Add(currency);
                        currency = new Currency();
                    }
                }

                return currencyTable;
            }

            catch
            {
                throw;
            }
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            throw args.Exception;
        }
    }
}
