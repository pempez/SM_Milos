using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pregledMilos
{
    public partial class FormUkupnoNaloga : Form
    {
        public FormUkupnoNaloga()
        {
            InitializeComponent();
            button1_Click(null, null);

        }

        public GroupBox GetGroupBox()
        {
            return groupBox1;
        }
        private void test()
        {
            string a = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = metode.DB.baza_upit("SELECT        TOP (100) PERCENT Status, " +
                     "    CASE WHEN Status = 0 THEN 'simulirani radni nalozi' WHEN status = 1 THEN 'planirani radni nalozi' WHEN status = 2 THEN 'potvrđeni radni nalozi' WHEN status = 3 THEN 'izdati radni nalozi' ELSE 'završeni radni nalozi' END AS " +
                      "    Naziv, COUNT(Status) AS broj FROM   dbo.[Stirg Produkcija$Production Order] GROUP BY Status ORDER BY Status");

            foreach (DataRow r in dt.Rows)
            {
                if (r["status"].ToString() == "0") label1.Text = "Ukupno " + r["naziv"].ToString() + " " + r["broj"].ToString();
                if (r["status"].ToString() == "1") label2.Text = "Ukupno " + r["naziv"].ToString() + " " + r["broj"].ToString();
                if (r["status"].ToString() == "2") label3.Text = "Ukupno " + r["naziv"].ToString() + " " + r["broj"].ToString();
                if (r["status"].ToString() == "3") label4.Text = "Ukupno " + r["naziv"].ToString() + " " + r["broj"].ToString();
                if (r["status"].ToString() == "4") label5.Text = "Ukupno " + r["naziv"].ToString() + " " + r["broj"].ToString();
            }
        }
    }
}
