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
        public Form1()
        {
            InitializeComponent();
        }

        private void Bind()
        {
            currencyBindingSource.DataSource = currencyTable;
            dataGridView1.DataSource = currencyBindingSource;
            dataGridView1.Columns["Multiplier"].Visible = false;
            dataGridView1.Columns["Name"].HeaderText = "Currency";
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            xmlParser = new XmlParser();
            currencyBindingSource = new BindingSource();
            currencyTable = await xmlParser.GetCurrencyTable();
            Bind();
        }

        private async void refreshBtn_Click(object sender, EventArgs e)
        {
            refreshBtn.Enabled = false;
            currencyTable = await xmlParser.GetCurrencyTable();
            Bind();
            refreshBtn.Enabled = true;
        }
    }
}
