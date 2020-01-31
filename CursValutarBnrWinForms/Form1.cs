using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CursValutarBnrWinForms
{
    public partial class Form1 : Form
    {
        BindingList<Currency> currencyTable;
        XmlParser xmlParser;
        BindingSource currencyBindingSource;
        Timer timer;
        public Form1()
        {
            InitializeComponent();
        }

        private void Bind()
        {
            if (currencyTable != null)
            {
                currencyBindingSource.DataSource = currencyTable;
                dataGridView1.DataSource = currencyBindingSource;
                dataGridView1.Columns["Multiplier"].Visible = false;
                dataGridView1.Columns["Name"].HeaderText = "Currency";
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
            }

            else
            {
                MessageBox.Show("Could not retrieve XML file");
                Application.Exit();
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 10000;

            xmlParser = new XmlParser();
            currencyBindingSource = new BindingSource();
            refreshBtn.Enabled = false;

            currencyTable = await Task.Run(() => xmlParser.GetCurrencyTable());
            refreshBtn.Enabled = true;
            timer.Start();
            Bind();
        }

        private async void refreshBtn_Click(object sender, EventArgs e)
        {
            refreshBtn.Enabled = false;
            timer.Stop();
            if (currencyTable != null)
            {
                Currency[] currencyTableAux = new Currency[currencyTable.Count];
                currencyTable.CopyTo(currencyTableAux, 0);
                currencyTable = await Task.Run(() => xmlParser.GetCurrencyTable());

                if (currencyTable != null)
                {
                    for (int i = 0; i < currencyTable.Count; i++)
                    {
                        if (currencyTable[i].Value > currencyTableAux[i].Value)
                        {
                            dataGridView1.Rows[i].Cells["Value"].Style.ForeColor = Color.Green;
                        }

                        else if (currencyTable[i].Value < currencyTableAux[i].Value)
                        {
                            dataGridView1.Rows[i].Cells["Value"].Style.ForeColor = Color.Red;
                        }

                        else
                        {
                            dataGridView1.Rows[i].Cells["Value"].Style.ForeColor = Color.Black;
                        }
                    }
                    timer.Start();
                    Bind();
                    refreshBtn.Enabled = true;
                }

                else
                {
                    MessageBox.Show("Something went wrong. Could not retrieve XML data");
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            refreshBtn_Click(sender, e);
        }
    }
}
