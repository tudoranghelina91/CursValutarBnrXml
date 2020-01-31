using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursValutarBnrWinForms
{
    class Currency
    {
        private string _name;
        public string Name {
            get 
            { 
                if (Multiplier > 1)
                {
                    return Multiplier + " " + _name;
                }

                else
                {
                    return _name;
                }
            } 
            set
            {
                _name = value;
            }
        }
        public decimal Value { get; set; }
        public int Multiplier { get; set; }

        public Currency(string name, decimal value, int multiplier = 1)
        {
            Name = name;
            Value = value;
            Multiplier = multiplier;
        }

        public Currency() { }
    }
}
