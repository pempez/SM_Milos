using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace pregledMilos
{
    public partial class Form1 : Form
    {
        private volatile bool m_StopThread = false;
        string BOJA = "";
        public Form1()
        {
            InitializeComponent();
            cbStatus.SelectedIndex = 4;
            popuniComboBoxProdOrderNo("3");
            popuniCBroutingNo();
            popuniCBSourceNo();
            popunicbSifraPorjkta();
            popuniCBExternalNo();
            popuniCBartikal();
            popuniCBradnik();
            popuniWC();
            FormUkupnoNaloga f1 = new FormUkupnoNaloga();
            this.groupBox1.Controls.Clear();
            this.groupBox1.Controls.Add(f1.GetGroupBox());
            f1.GetGroupBox().Dock = DockStyle.Fill;
            f1.GetGroupBox().Show();

            cbRadnik.SelectedIndex = -1;
            cbRadniCentar.SelectedIndex = -1;
            cbProdOrderNo.SelectedIndex = -1;
            cbItemNo.SelectedIndex = -1;
            cbExternalDocumentNo.SelectedIndex = -1;
            cbRoutingNo.SelectedIndex = -1;
            cbSourceNo.SelectedIndex = -1;
            cbSifraProjekta.SelectedIndex = -1;


           // groupBox3.Paint += PaintBorderlessGroupBox;
           

        }


        private void PaintBorderlessGroupBox(object sender, PaintEventArgs p)
        {
            GroupBox box = (GroupBox)sender;
            p.Graphics.Clear(SystemColors.Control);
           // p.Graphics.DrawString(box.Text, box.Font, Brushes.Blue, 7, 1);
            p.Graphics.DrawRectangle(Pens.Black, box.Location.X, box.Location.Y,box.Size.Width  , box.Size.Height);
        }

        private void popuniCBradnik()
        {
            cbRadnik.DataSource = metode.DB.baza_upit("SELECT   TOP (100) PERCENT No_, Name " +
                                    " FROM            dbo.[Stirg Produkcija$Resource] WHERE        (Name <> N'') ORDER BY Name");
            cbRadnik.DisplayMember = "Name";
            cbRadnik.ValueMember = "No_";

        }

        private void popuniWC()
        {
            cbRadniCentar.DataSource = metode.DB.baza_upit("SELECT        No_, Name " +
                        " FROM              dbo.[Stirg Produkcija$Work Center] " +
                        " ORDER BY  No_");
            cbRadniCentar.DisplayMember = "Name";
            cbRadniCentar.ValueMember = "No_";
        }
        private void popuniComboBoxProdOrderNo()
        {
            cbProdOrderNo.DataSource = metode.DB.baza_upit("SELECT DISTINCT TOP (100) PERCENT No_ " +
          " FROM            dbo.[Stirg Produkcija$Production Order] " +
          "  ORDER BY No_");
            cbProdOrderNo.DisplayMember = "No_";
            cbProdOrderNo.ValueMember = "No_";
        }

        private void popuniComboBoxProdOrderNo(string status)
        {
            cbProdOrderNo.DataSource = metode.DB.baza_upit("SELECT DISTINCT TOP (100) PERCENT No_ " +
          " FROM            dbo.[Stirg Produkcija$Production Order] " +
          " where status = '" + status + "'" +
          "  ORDER BY No_");
            cbProdOrderNo.DisplayMember = "No_";
            cbProdOrderNo.ValueMember = "No_";
        }

        private void popuniCBroutingNo()
        {
            cbRoutingNo.DataSource = metode.DB.baza_upit("SELECT DISTINCT TOP (100) PERCENT [Routing No_]" +
                         " FROM            dbo.[Stirg Produkcija$Production Order] " +
                         "  ORDER BY [Routing No_]");
            cbRoutingNo.DisplayMember = "Routing No_";
            cbRoutingNo.ValueMember = "Routing No_";
        }
        private void popuniCBExternalNo()
        {

            cbExternalDocumentNo.DataSource = metode.DB.baza_upit("SELECT DISTINCT TOP (100) PERCENT [External Document No_]" +
                         " FROM            dbo.[Stirg Produkcija$Production Order] " +
                         "  ORDER BY [External Document No_]");
            cbExternalDocumentNo.DisplayMember = "External Document No_";
            cbExternalDocumentNo.ValueMember = "External Document No_";
        }

        private void popuniCBSourceNo()
        {
            cbSourceNo.DataSource = metode.DB.baza_upit("SELECT DISTINCT TOP (100) PERCENT [Source No_] " +
                      " FROM            dbo.[Stirg Produkcija$Production Order] " +
                      "  ORDER BY [Source No_] ");
            cbSourceNo.DisplayMember = "Source No_";
            cbSourceNo.ValueMember = "Source No_";
        }
        private void popunicbSifraPorjkta()
        {

            cbSifraProjekta.DataSource = metode.DB.baza_upit("SELECT DISTINCT TOP (100) PERCENT  [Shortcut Dimension 1 Code] " +
                      " FROM            dbo.[Stirg Produkcija$Production Order] " +
                      "  ORDER BY  [Shortcut Dimension 1 Code] ");
            cbSifraProjekta.DisplayMember = "Shortcut Dimension 1 Code";
            cbSifraProjekta.ValueMember = "Shortcut Dimension 1 Code";
        }

        private void popuniCBartikal()
        {

            cbItemNo.DataSource = metode.DB.baza_upit("SELECT DISTINCT TOP (100) PERCENT [Item No_]" +
                         " FROM           dbo.[Stirg Produkcija$Prod_ Order Line]  " +
                         "  ORDER BY [Item No_]");
            cbItemNo.DisplayMember = "Item No_";
            cbItemNo.ValueMember = "Item No_";
        }


        private void ucitajProdOrder(DateTime datumOd, DateTime datumDo, Thread thread)
        {
            string qSelect = " select DISTINCT  dbo.[Stirg Produkcija$Production Order].No_ AS [radni nalog], dbo.[Stirg Produkcija$Production Order].Status, dbo.[Stirg Produkcija$Production Order].Description AS Opis,  " +
                         " dbo.[Stirg Produkcija$Production Order].Quantity AS Količina, dbo.[Stirg Produkcija$Production Order].[Starting Date] AS [Početni datum], dbo.[Stirg Produkcija$Production Order].[Ending Date] AS [Krajnji datum],  " +
                        "  dbo.[Stirg Produkcija$Production Order].[Due Date] AS [Datum dospeca], dbo.[Stirg Produkcija$Production Order].[External Document No_] AS [broj eksternog dokumenta],  " +
                        "  dbo.[Stirg Produkcija$Production Order].[Location Code] AS [šifra lokacije], dbo.[Stirg Produkcija$Production Order].[Shortcut Dimension 1 Code] AS [šifra projekta],  " +
                        "  dbo.[Stirg Produkcija$Production Order].[Source No_] AS [broj izvora], dbo.[Stirg Produkcija$Production Order].[Routing No_] AS [broj proizvodnog postupka]  FROM [Stirg Produkcija$Production Order] " +
                   "  INNER JOIN dbo.[Stirg Produkcija$Prod_ Order Line] ON dbo.[Stirg Produkcija$Production Order].No_ = dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_] ";

            if (cbRadnik.Text != "" || cbSmena.Text != "" || cbRadniCentar.Text != "" || cbPeriodOJD.Checked)
            {
                qSelect += " INNER JOIN  dbo.[Stirg Produkcija$Output Journal Data] ON dbo.[Stirg Produkcija$Prod_ Order Line].[Item No_] = dbo.[Stirg Produkcija$Output Journal Data].[Item No_] " +
                    " AND dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_] = dbo.[Stirg Produkcija$Output Journal Data].[Prod_ Order No_]";
            }

            if (cbRadniCentar.Text != "")
            {
                qSelect += " INNER JOIN    dbo.[Stirg Produkcija$Prod_ Order Routing Line] ON dbo.[Stirg Produkcija$Output Journal Data].[Operation No_] = dbo.[Stirg Produkcija$Prod_ Order Routing Line].[Operation No_]  AND " +
                           " dbo.[Stirg Produkcija$Output Journal Data].[Prod_ Order No_] = dbo.[Stirg Produkcija$Prod_ Order Routing Line].[Prod_ Order No_] AND " +
                           " dbo.[Stirg Produkcija$Prod_ Order Line].[Routing No_] = dbo.[Stirg Produkcija$Prod_ Order Routing Line].[Routing No_]  AND " +
                         " dbo.[Stirg Produkcija$Prod_ Order Line].[Line No_] = dbo.[Stirg Produkcija$Prod_ Order Routing Line].[Routing Reference No_]";
            }

            qSelect += " where 1=1 ";

            if (cbRadnik.Text != "")
            {
                qSelect += " AND (dbo.[Stirg Produkcija$Output Journal Data].[Resource No_] = N'" + cbRadnik.SelectedValue + "')";
            }
            if (cbRadniCentar.Text != "")
            {
                qSelect += " AND (dbo.[Stirg Produkcija$Prod_ Order Routing Line].[Work Center No_] = N'" + cbRadniCentar.SelectedValue + "')";
            }
            if (cbSmena.Text != "")
            {
                int satiOd = 0;
                int satiDo = 0;
                if (cbSmena.Text == "I")
                {
                    satiOd = 7;
                    satiDo = 15;
                    qSelect += " AND (CAST(dbo.[Stirg Produkcija$Output Journal Data].[Starting Time] AS time)>= '" + satiOd + ":00:00')" +
                   " AND (CAST(dbo.[Stirg Produkcija$Output Journal Data].[Starting Time] AS time)<= '" + satiDo + ":00:00') ";
                }
                else if (cbSmena.Text == "III")
                {
                    satiOd = 23;
                    satiDo = 7;
                    qSelect += " AND ((CAST(dbo.[Stirg Produkcija$Output Journal Data].[Starting Time] AS time)>= '" + satiOd + ":00:00') and (CAST(dbo.[Stirg Produkcija$Output Journal Data].[Starting Time] AS time) <= '23:59:59')  " +
                   " or  (CAST(dbo.[Stirg Produkcija$Output Journal Data].[Starting Time] AS time) >= '00:00:00')  and (CAST(dbo.[Stirg Produkcija$Output Journal Data].[Starting Time] AS time)<= '" + satiDo + ":00:00')) ";
                }
                else if (cbSmena.Text == "II")
                {
                    satiOd = 15;
                    satiDo = 23;
                    qSelect += " AND (CAST(dbo.[Stirg Produkcija$Output Journal Data].[Starting Time] AS time)>= '" + satiOd + ":00:00')" +
                   " AND (CAST(dbo.[Stirg Produkcija$Output Journal Data].[Starting Time] AS time)<= '" + satiDo + ":00:00') ";
                }


            }

            if (cbPeriod.Checked)
            {
                qSelect += " and ( dbo.[Stirg Produkcija$Production Order].[Starting Date] >= CONVERT(DATETIME, '" + datumOd.Year + "-" + datumOd.Month + "-" + datumOd.Day + " 00:00:00', 102))" +
                     " and ( dbo.[Stirg Produkcija$Production Order].[Starting Date] <= CONVERT(DATETIME, '" + datumDo.Year + "-" + datumDo.Month + "-" + datumDo.Day + " 23:59:59', 102)) ";
            }

            if (cbPeriodOJD.Checked)
            {
                qSelect += " and (   dbo.[Stirg Produkcija$Output Journal Data].[Posting Date] >= CONVERT(DATETIME, '" + datumOd.Year + "-" + datumOd.Month + "-" + datumOd.Day + " 00:00:00', 102))" +
                     " and (   dbo.[Stirg Produkcija$Output Journal Data].[Posting Date] <= CONVERT(DATETIME, '" + datumDo.Year + "-" + datumDo.Month + "-" + datumDo.Day + " 23:59:59', 102)) ";
            }

            if (cbExternalDocumentNo.Text != "")
            {
                qSelect += " and ( dbo.[Stirg Produkcija$Production Order].[External Document No_] = N'" + cbExternalDocumentNo.Text + "') ";
            }
            if (cbProdOrderNo.Text != "")//(tbProdOrderNo.Text != "")
            {
                qSelect += "  AND ( dbo.[Stirg Produkcija$Production Order].No_ =  N'" + cbProdOrderNo.Text + "') ";
            }
            if (cbRoutingNo.Text != "")
            {
                qSelect += " AND ( dbo.[Stirg Produkcija$Production Order].[Routing No_] =  N'" + cbRoutingNo.Text + "')  ";
            }
            if (cbSourceNo.Text != "")
            {

                qSelect += "  AND ( dbo.[Stirg Produkcija$Production Order].[Source No_] =  N'" + cbSourceNo.Text + "')";
            }
            if (cbSifraProjekta.Text != "")
            {

                qSelect += "  AND ( dbo.[Stirg Produkcija$Production Order].[Shortcut Dimension 1 Code] =  N'" + cbSifraProjekta.Text + "')";
            }
            if (cbItemNo.Text != "")
            {

                qSelect += "  AND (dbo.[Stirg Produkcija$Prod_ Order Line].[Item No_] = N'" + cbItemNo.Text + "')";
            }
            if (cbStatus.Text != "Svi")
            {
                string status = "";
                switch (cbStatus.Text)
                {
                    case "simulirani radni nalozi":
                        status = "0";
                        break;
                    case "planirani radni nalozi":
                        status = "1";
                        break;
                    case "potvrđeni radni nalozi":
                        status = "2";
                        break;
                    case "izdati radni nalozi":
                        status = "3";
                        break;
                    case "završeni radni nalozi":
                        status = "4";
                        break;
                }
                qSelect += "  AND ( dbo.[Stirg Produkcija$Production Order].Status =  " + status + ")";
            }

            qSelect += " order by  dbo.[Stirg Produkcija$Production Order].[Starting Date]";


            DataTable dt = metode.DB.baza_upit(qSelect);

            if (dt.Rows.Count > 0)
            {
                lblPO.Text = dt.Rows.Count.ToString();
                thread.Start();
                dgvProdOrder.DataSource = dt;
                dgvProdOrder.Columns["Količina"].DefaultCellStyle.Format = "N2";
                dgvProdOrder_Click(null, null);

                foreach (DataGridViewRow row in dgvProdOrder.Rows)
                {
                    row.DefaultCellStyle.BackColor = bojenjeProdOrder(row.Cells["radni nalog"].Value.ToString(), dt.Rows.Count);
                }

                dgvProdOrder.Focus();

                thread.Abort();
            }
            else
            {
                lblPO.Text = "";
                dgvProdOrder.DataSource = null;
                dgvProdOrderComponent.DataSource = null;
                dgvProdOrderLine.DataSource = null;
                dgvProdOrderRoutingLine.DataSource = null;
                dgvOutpuJournalData.DataSource = null;
                MessageBox.Show("Nema podataka za zadati kriterijum.");
            }
        }




        private System.Drawing.Color bojenjeProdOrder(string prodOrNo, int broj)
        {
            string qSelect = "SELECT        isnull(SUM(dbo.[Stirg Produkcija$Prod_ Order Line].Quantity),0) AS ukupno, isnull(SUM(dbo.[Stirg Produkcija$Prod_ Order Line].[Finished Quantity]),0) AS zavrseno, isnull(SUM(dbo.[Stirg Produkcija$Prod_ Order Line].[Remaining Quantity]),0)  " +
                        " AS preostalo " +
                        " FROM            dbo.[Stirg Produkcija$Production Order] INNER JOIN " +
                        "  dbo.[Stirg Produkcija$Prod_ Order Line] ON dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_] = dbo.[Stirg Produkcija$Production Order].No_ " +
                        "  WHERE        (dbo.[Stirg Produkcija$Production Order].No_ = N'" + prodOrNo + "')";

            DataTable dt = metode.DB.baza_upit(qSelect);

            double zavrseno = 0;
            double peostalo = 0;


            zavrseno = double.Parse(dt.Rows[0]["zavrseno"].ToString());
            peostalo = double.Parse(dt.Rows[0]["preostalo"].ToString());
            if (zavrseno == 0) return System.Drawing.Color.Coral;

            else if (peostalo == 0) return System.Drawing.Color.LightGreen;
            else return System.Drawing.Color.LightYellow;
        }
        private void ucitajProdOrderLine(string prodOrderNo)
        {
            TimeSpan vremeOd;
            TimeSpan vremeDo;
            TimeSpan vremeOd3 = new TimeSpan(00, 00, 00);



            if (cbSmena.Text != "I")
            {
                if (cbSmena.Text == "II")
                {
                    vremeOd = new TimeSpan(15, 00, 00);
                    vremeDo = new TimeSpan(23, 0, 0);
                }
                else
                {
                    vremeOd = new TimeSpan(23, 00, 00);
                    vremeDo = new TimeSpan(7, 0, 0);

                }
            }
            else
            {
                vremeOd = new TimeSpan(7, 00, 00);
                vremeDo = new TimeSpan(15, 0, 0);
            }

            DataTable dt = metode.DB.baza_upit(" select [Prod_ Order No_] ,[Routing No_],  [Line No_] as [Broj reda],[Planning Level Code] as [Nivo ugradnje], [Item No_] as [Broj artikla], Description as [Opis]," +
                "  CONVERT(char(10), [Starting Date], 104) AS [Pocetni datum], CONVERT(char(5), [Starting Time], 108) AS [Pocetno vreme], CONVERT(char(10), [Ending Date], 104) AS [Krajnji datum],CONVERT(char(5), [ending Time], 108) AS [Krajnje vreme], " +
            " Quantity as [količina], [Finished Quantity] as [završena količina], [Remaining Quantity] as [preostala količina], [Scrap _] as [škart], [Unit of Measure Code] as [šifra jedinice mere]" +
            " FROM             [Stirg Produkcija$Prod_ Order Line] " +
            " WHERE        ([Prod_ Order No_] = N'" + prodOrderNo + "') order by [Line No_]");

            DataTable dtPomocna = metode.DB.baza_upit("SELECT        TOP (100) PERCENT dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_], dbo.[Stirg Produkcija$Prod_ Order Line].[Routing No_], dbo.[Stirg Produkcija$Prod_ Order Line].[Line No_] AS [Broj reda],  " +
                        " dbo.[Stirg Produkcija$Prod_ Order Line].[Planning Level Code] AS[Nivo ugradnje], dbo.[Stirg Produkcija$Prod_ Order Line].[Item No_] AS artikal, dbo.[Stirg Produkcija$Prod_ Order Line].Description AS Opis, " +
                      "  CONVERT(char(10),  dbo.[Stirg Produkcija$Output Journal Data].[Posting Date], 104) AS [datum rada], CONVERT(char(5), dbo.[Stirg Produkcija$Prod_ Order Line].[Starting Time], 108) AS[Pocetno vreme], CONVERT(char(10), " +
                   "   dbo.[Stirg Produkcija$Prod_ Order Line].[Ending Date], 104) AS[Krajnji datum], CONVERT(char(5), dbo.[Stirg Produkcija$Prod_ Order Line].[Ending Time], 108) AS[Krajnje vreme],  " +
                      "    dbo.[Stirg Produkcija$Prod_ Order Line].Quantity AS količina, dbo.[Stirg Produkcija$Prod_ Order Line].[Finished Quantity] AS[završena količina], dbo.[Stirg Produkcija$Prod_ Order Line].[Remaining Quantity] AS[preostala količina],  " +
                    "      dbo.[Stirg Produkcija$Prod_ Order Line].[Scrap _] AS škart, dbo.[Stirg Produkcija$Prod_ Order Line].[Unit of Measure Code] AS[šifra jedinice mere], dbo.[Stirg Produkcija$Output Journal Data].[Resource No_] as radnik, " +
                    " dbo.[Stirg Produkcija$Output Journal Data].[Posting Date] as datumRada, dbo.[Stirg Produkcija$Output Journal Data].[Operation No_] as opera, dbo.[Stirg Produkcija$Prod_ Order Routing Line].[Operation No_] AS brojOperacijeOJD, " +
                    " CONVERT(char(5), dbo.[Stirg Produkcija$Output Journal Data].[Starting Time], 108) AS [pocetnoVremeOJD], dbo.[Stirg Produkcija$Prod_ Order Routing Line].No_ as sifraRC " +
       "  FROM dbo.[Stirg Produkcija$Prod_ Order Line] INNER JOIN " +
    " dbo.[Stirg Produkcija$Output Journal Data] ON dbo.[Stirg Produkcija$Prod_ Order Line].[Item No_] = dbo.[Stirg Produkcija$Output Journal Data].[Item No_] AND " +
                " dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_] = dbo.[Stirg Produkcija$Output Journal Data].[Prod_ Order No_] " +
                " INNER JOIN dbo.[Stirg Produkcija$Prod_ Order Routing Line] ON dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_] = dbo.[Stirg Produkcija$Prod_ Order Routing Line].[Prod_ Order No_] AND " +
                      "    dbo.[Stirg Produkcija$Prod_ Order Line].[Line No_] = dbo.[Stirg Produkcija$Prod_ Order Routing Line].[Routing Reference No_] " +
                " WHERE        (dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_] = N'" + prodOrderNo + "') " +
                " ORDER BY[Broj reda]");

            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView(dtPomocna);
                dgvProdOrderLine.DataSource = dt;
                dgvProdOrderLine.Columns["Routing No_"].Visible = false;
                dgvProdOrderLine.Columns["Prod_ Order No_"].Visible = false;
                dgvProdOrderLine.Columns["količina"].DefaultCellStyle.Format = "N2";
                dgvProdOrderLine.Columns["završena količina"].DefaultCellStyle.Format = "N2";
                dgvProdOrderLine.Columns["preostala količina"].DefaultCellStyle.Format = "N2";
                dgvProdOrderLine.Columns["škart"].DefaultCellStyle.Format = "N2";

                lblPOL.Text = dt.Rows.Count.ToString();

                foreach (DataGridViewRow row in dgvProdOrderLine.Rows)
                {
                    if (double.Parse(row.Cells["preostala količina"].Value.ToString()) == 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;

                    }
                    else if (double.Parse(row.Cells["završena količina"].Value.ToString()) == 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.Coral;

                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                    }

                    if (!desniFilter() && cbItemNo.Text!="")
                    {
                        if (row.Cells["Broj artikla"].Value.ToString() == cbItemNo.Text)//dv.Count > 0 && 
                        {
                            row.DefaultCellStyle.BackColor = Color.MediumPurple;
                        }
                        
                    }

                    else if (cbPeriodOJD.Checked || cbItemNo.Text != "" || cbRadnik.Text != "" || cbSmena.Text != "" || cbRadniCentar.Text != "")
                    {
                        if (cbItemNo.Text != "")
                        {
                            dv.RowFilter = " artikal ='" + cbItemNo.Text + "' and  artikal ='" + row.Cells["Broj artikla"].Value.ToString() + "'";
                        }
                        else
                        {
                            dv.RowFilter = "  artikal ='" + row.Cells["Broj artikla"].Value.ToString() + "'";
                        }
                        if (cbSmena.Text != "")
                        {
                            if (cbSmena.Text == "III")
                            {
                                dv.RowFilter += " and ((pocetnoVremeOJD >= '" + vremeOd + "') or " +
                                " (pocetnoVremeOJD >= '" + vremeOd3 + "' and pocetnoVremeOJD <= '" + vremeDo + "')) ";
                            }
                            else
                            {
                                dv.RowFilter += " and pocetnoVremeOJD >= '" + vremeOd + "' and pocetnoVremeOJD <= '" + vremeDo + "'";
                            }


                        }
                        if (cbPeriodOJD.Checked)
                        {
                            dv.RowFilter += " and (datumRada >=  '" + dtpOJDod.Value.Year + "-" + dtpOJDod.Value.Month + "-" + dtpOJDod.Value.Day + "') " +
                                " AND  (datumRada <= '" + dtpOJDdo.Value.Year + "-" + dtpOJDdo.Value.Month + "-" + dtpOJDdo.Value.Day + "' )";
                        }

                        if (cbRadnik.Text != "")
                        {
                            dv.RowFilter += " and radnik = '" + cbRadnik.SelectedValue.ToString() + "' ";

                        }

                        if (cbRadniCentar.Text != "")
                        {

                            dv.RowFilter += " AND (opera = brojOperacijeOJD ) ";// and (sifraRC =" + cbRadniCentar.SelectedValue.ToString()+")";
                        }


                        //if (cbPeriodOJD.Checked || cbRadnik.Text != "" || cbSmena.Text != "" || cbRadniCentar.Text != "")
                        //{

                        if (dv.Count > 0)
                        {
                            if (cbRadniCentar.Text != "")
                            {
                                foreach (DataRowView dataRow in dv)
                                {
                                    if (cbRadniCentar.Text != "")
                                    {
                                        if (dataRow["sifraRC"].ToString() == cbRadniCentar.SelectedValue.ToString())
                                            row.DefaultCellStyle.BackColor = Color.MediumPurple;
                                    }
                                    else
                                    {
                                        row.DefaultCellStyle.BackColor = Color.MediumPurple;
                                    }
                                }
                            }
                            else
                            {
                                row.DefaultCellStyle.BackColor = Color.MediumPurple;
                            }
                            
                        }
                        
                    }
                }

            }
            else
            {
                lblPOL.Text = "";
                dgvProdOrderLine.DataSource = null;
            }
            dgvProdOrderLine_Click(null, null);

        }

        private void obojiPOLfilter(DataGridViewRow row, string radnik, string artikal, string radniNalog)
        {
            if (metode.DB.baza_upit("SELECT DISTINCT TOP (100) PERCENT dbo.[Stirg Produkcija$Output Journal Data].[Resource No_], dbo.[Stirg Produkcija$Prod_ Order Line].[Item No_], dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_] " +
                               " FROM            dbo.[Stirg Produkcija$Prod_ Order Line] INNER JOIN " +
                          "   dbo.[Stirg Produkcija$Output Journal Data] ON dbo.[Stirg Produkcija$Prod_ Order Line].[Item No_] = dbo.[Stirg Produkcija$Output Journal Data].[Item No_] AND " +
                           "  dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_] = dbo.[Stirg Produkcija$Output Journal Data].[Prod_ Order No_] " +
                               " WHERE (dbo.[Stirg Produkcija$Output Journal Data].[Resource No_] = N'" + radnik + "') AND(dbo.[Stirg Produkcija$Prod_ Order Line].[Item No_] = N'" + artikal + "') AND " +
                          "   (dbo.[Stirg Produkcija$Prod_ Order Line].[Prod_ Order No_] = N'" + radniNalog + "')").Rows.Count > 0)

                row.DefaultCellStyle.BackColor = Color.MediumPurple;



        }
        private void ucitajProdOrderRoutingLine(string prodOrderNo, string referenceNo, string routingNo)
        {
            DataTable dt = metode.DB.baza_upit(" select [Routing Reference No_] as [referentni broj proizvodnog postupka], [Operation No_] as [Broj operacije]," +
                " 'Radni centar' as Vrsta,[Work Center No_] as Broj, Description as Opis ,[Setup Time] as [vreme podešavanja], [Run Time] as [vreme izvođenja], [Send-Ahead Quantity] as [unapred poslata količina], [Concurrent Capacities] as  [uporedni kapaciteti] FROM            [Stirg Produkcija$Prod_ Order Routing Line]" +
                 " WHERE        ([Prod_ Order No_] = N'" + prodOrderNo + "') AND ([Routing Reference No_] = '" + referenceNo + "')");

            if (dt.Rows.Count > 0)
            {
                dgvProdOrderRoutingLine.DataSource = dt;
                dgvProdOrderRoutingLine.Columns["vreme podešavanja"].DefaultCellStyle.Format = "N2";
                dgvProdOrderRoutingLine.Columns["vreme izvođenja"].DefaultCellStyle.Format = "N2";
                dgvProdOrderRoutingLine.Columns["unapred poslata količina"].DefaultCellStyle.Format = "N2";
                dgvProdOrderRoutingLine.Columns["uporedni kapaciteti"].DefaultCellStyle.Format = "N2";
            }
            else
            {
                dgvProdOrderRoutingLine.DataSource = null;
            }

        }
        private void ucitajProdOrderComponent(string lineNo, string prodOrderNo)
        {
            DataTable dt = metode.DB.baza_upit(" select distinct  [Line No_],[Prod_ Order Line No_] as [Broj reda naloga za proizvodnju] ,[Item No_] as [Broj artikla] ,Description as [Opis], [Quantity per] as [Količina po] , [Unit of Measure Code] as [Šifra jedinice mere] ," +
                "    CASE WHEN [flushing method] = 0 THEN 'ručno' WHEN [flushing method] = 1 THEN 'unapred' ELSE 'unazad' END AS [metod knjiženja]" +
                " FROM   [Stirg Produkcija$Prod_ Order Component] WHERE        ([Prod_ Order Line No_] = '" + lineNo + "') AND ([Prod_ Order No_] = N'" + prodOrderNo + "') order by  [Line No_]");

            if (dt.Rows.Count > 0)
            {
                dgvProdOrderComponent.DataSource = dt;
                dgvProdOrderComponent.Columns["Količina po"].DefaultCellStyle.Format = "N2";
                dgvProdOrderComponent.Columns["Line No_"].Visible = false;
            }
            else
            {
                dgvProdOrderComponent.DataSource = null;
            }

        }
        private void outputJournalData(string itemNo, string prodOrderNo, string lineNo, string quantity)
        {
            DataTable dt = metode.DB.baza_upit(" SELECT        dbo.[Stirg Produkcija$Output Journal Data].[Item No_] as [Broj artikla] , CASE WHEN dbo.[Stirg Produkcija$Output Journal Data].Status = 0 THEN 'započeta' ELSE 'proknjižena' END AS status, dbo.[Stirg Produkcija$Output Journal Data].[Prod_ Order No_] as [Broj naloga za proizvodnju], dbo.[Stirg Produkcija$Output Journal Data].[Operation No_] as [Broj operacije], " +
                        "  dbo.[Stirg Produkcija$Output Journal Data].[Last Operation No_] as [Broj poslednje operacije] , dbo.[Stirg Produkcija$Output Journal Data].[Posting Date] as [Datum knjiženja],   " +
                     "  CONVERT(char(5),  dbo.[Stirg Produkcija$Output Journal Data].[Starting Time], 108) AS [Vreme prvog registrovanog početka] , " +
                        "  CONVERT(char(5),  dbo.[Stirg Produkcija$Output Journal Data].[Ending Time], 108) AS [Vreme poslednjeg registrovanog završetka], " +
                        "   dbo.[Stirg Produkcija$Output Journal Data].[output quantity] as [Izlazna količina], dbo.[Stirg Produkcija$Output Journal Data].[Scrap Quantity] as [Količina škarta],  " +
                         "  dbo.[Stirg Produkcija$Output Journal Data].[Resource No_] as [Šifra resursa], dbo.[Stirg Produkcija$Resource].Name as [Ime resursa], DATEDIFF(minute, dbo.[Stirg Produkcija$Output Journal Data].[Starting Time],  " +
                         "  dbo.[Stirg Produkcija$Output Journal Data].[Ending Time]) AS [stvarno Trajanje], dbo.[Stirg Produkcija$Output Journal Data].[Line No_] as [Broj reda] , dbo.[Stirg Produkcija$Output Journal Data].[Controlled Operation No_] as [Broj kontrolisane operacije] " +
                         " FROM            dbo.[Stirg Produkcija$Output Journal Data] INNER JOIN " +
                       "    dbo.[Stirg Produkcija$Resource] ON dbo.[Stirg Produkcija$Output Journal Data].[Resource No_] = dbo.[Stirg Produkcija$Resource].No_ " +
                       " WHERE          (dbo.[Stirg Produkcija$Output Journal Data].[Item No_] = N'" + itemNo + "') AND (dbo.[Stirg Produkcija$Output Journal Data].[Prod_ Order No_] = N'" + prodOrderNo + "')" +
                       " order by [Operation No_], [Posting Date], [Starting Time] ");// AND (dbo.[Stirg Produkcija$Output Journal Data].[Line No_] = '" + lineNo + "')");
            if (dt.Rows.Count > 0)
            {
                lblOJD.Text = dt.Rows.Count.ToString();
                dgvOutpuJournalData.DataSource = dt;
                dgvOutpuJournalData.Columns["Izlazna količina"].DefaultCellStyle.Format = "N2";
                dgvOutpuJournalData.Columns["Količina škarta"].DefaultCellStyle.Format = "N2";

                //bojenje
                foreach (DataGridViewRow r in dgvOutpuJournalData.Rows)
                {

                    if (double.Parse(r.Cells["Izlazna količina"].Value.ToString()) == double.Parse(quantity))
                    {

                        r.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else if (double.Parse(r.Cells["Izlazna količina"].Value.ToString()) > 0)
                    {
                        r.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                    else
                    {
                        r.DefaultCellStyle.BackColor = Color.Coral;
                    }
                    if (desniFilter())
                    {
                        if (cbItemNo.Text != "")
                        {
                            if (cbItemNo.Text == dgvProdOrderLine.CurrentRow.Cells["Broj artikla"].Value.ToString())
                                bojenjeODJfilter(r);
                        }
                        else
                            bojenjeODJfilter(r);
                    }
                }

            }
            else
            {
                lblOJD.Text = "";
                dgvOutpuJournalData.DataSource = null;
            }


        }

        private bool desniFilter()
        {
            if (cbRadnik.Text != "" || cbPeriodOJD.Checked || cbSmena.Text != "" || cbRadniCentar.Text != "")
                return true;
            else return false;
        }
        private void prikaziProgresBar()
        {
            FormProgressBar un = new FormProgressBar();

            un.ShowDialog();


        }

        private void btnPronadji_Click(object sender, EventArgs e)
        {
            DateTime sad = DateTime.Now;
            DateTime kraj;

            DateTime datumOd;
            DateTime datumDo;
            Thread thread = new Thread(new ThreadStart(prikaziProgresBar));


            if (cbPeriodOJD.Checked)
            {
                datumOd = new DateTime(dtpOJDod.Value.Year, dtpOJDod.Value.Month, dtpOJDod.Value.Day);
                datumDo = new DateTime(dtpOJDdo.Value.Year, dtpOJDdo.Value.Month, dtpOJDdo.Value.Day);
            }
            else
            {
                datumOd = new DateTime(dtpDatum.Value.Year, dtpDatum.Value.Month, dtpDatum.Value.Day);
                datumDo = new DateTime(dtpDatumDo.Value.Year, dtpDatumDo.Value.Month, dtpDatumDo.Value.Day);
            }

            ucitajProdOrder(datumOd, datumDo, thread);


            try
            {
                BOJA = dgvProdOrder.CurrentRow.DefaultCellStyle.BackColor.Name;
            }
            catch { BOJA = ""; }
            if (BOJA == "Coral")
            {
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dgvProdOrder_Click(null, null);
            }
            else if (BOJA == "LightGreen")
            {
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dgvProdOrder_Click(null, null);
            }

            else if (BOJA == "LightYellow")
            {
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dgvProdOrder_Click(null, null);
            }

            kraj = DateTime.Now;
            var seconds = System.Math.Abs((sad - kraj).TotalSeconds);
            label16.Text = "uradjeno za " + seconds;


        }

        private void dgvProdOrder_Click(object sender, EventArgs e)
        {
            if (dgvProdOrder.SelectedRows.Count > 0)
            {
                ucitajProdOrderLine(dgvProdOrder.CurrentRow.Cells["radni nalog"].Value.ToString());

                //
                try
                {
                    BOJA = dgvProdOrderLine.CurrentRow.DefaultCellStyle.BackColor.Name;
                }
                catch { BOJA = ""; }
                if (BOJA == "Coral")
                {
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }
                else if (BOJA == "LightGreen")
                {
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }

                else if (BOJA == "LightYellow")
                {
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }
                else if (BOJA == "MediumPurple")
                {
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkViolet;
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;


                }

            }
        }

        private void dgvProdOrderLine_Click(object sender, EventArgs e)
        {

            if (dgvProdOrderLine.SelectedRows.Count > 0)
            {

                try
                {

                    string prodOrderNo = dgvProdOrderLine.CurrentRow.Cells["Prod_ Order No_"].Value.ToString();
                    string lineNo = dgvProdOrderLine.CurrentRow.Cells["Broj reda"].Value.ToString();
                    string itemNo = dgvProdOrderLine.CurrentRow.Cells["Broj artikla"].Value.ToString();
                    string quantity = dgvProdOrderLine.CurrentRow.Cells["količina"].Value.ToString();
                    ucitajProdOrderRoutingLine(prodOrderNo, lineNo, dgvProdOrderLine.CurrentRow.Cells["Routing No_"].Value.ToString());

                    ucitajProdOrderComponent(dgvProdOrderLine.CurrentRow.Cells["Broj reda"].Value.ToString(), dgvProdOrderLine.CurrentRow.Cells["Prod_ Order No_"].Value.ToString());

                    outputJournalData(itemNo, prodOrderNo, lineNo, quantity);

                    //bojenjeProdOrderLine
                    try
                    {
                        BOJA = dgvProdOrderLine.CurrentRow.DefaultCellStyle.BackColor.Name;
                    }
                    catch { BOJA = ""; }
                    if (BOJA == "Coral")
                    {
                        dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                        dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    }
                    else if (BOJA == "LightGreen")
                    {
                        dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                        dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    }

                    else if (BOJA == "LightYellow")
                    {
                        dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                        dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    }
                    else if (BOJA == "MediumPurple")
                    {
                        dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkViolet;
                        dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                    }

                    //bojenje OjD
                    try
                    {
                        BOJA = dgvOutpuJournalData.CurrentRow.DefaultCellStyle.BackColor.Name;
                        if (BOJA == "Coral")
                        {
                            dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                            dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                        }
                        else if (BOJA == "LightGreen")
                        {
                            dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                            dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                        }

                        else if (BOJA == "LightYellow")
                        {
                            dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                            dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                        }
                        else if (BOJA == "MediumPurple")
                        {
                            dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkViolet;
                            dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                        }

                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
            }
        }



        private void cbPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPeriod.Checked)
            {
                cbProdOrderNo.Text = "";
                cbItemNo.Text = "";
                cbPeriodOJD.Checked = false;
                cbRadniCentar.Text = "";
                cbRadnik.Text = "";
                cbSmena.Text = "";
            }
            else cbStatus_SelectedValueChanged(null, null);// cbProdOrderNo.SelectedIndex = 1;
        }

        private void dgvProdOrder_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                string aa = dgvProdOrder.CurrentRow.Cells["radni nalog"].Value.ToString();
                // ucitajProdOrderLine(aa);
                //}
                //catch { }
                //try
                //{
                BOJA = dgvProdOrder.CurrentRow.DefaultCellStyle.BackColor.Name;
                if (BOJA == "Coral")
                {
                    dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                    dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    dgvProdOrder_Click(null, null);
                }
                else if (BOJA == "LightGreen")
                {
                    dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                    dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    dgvProdOrder_Click(null, null);
                }

                else if (BOJA == "LightYellow")
                {
                    dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                    dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    dgvProdOrder_Click(null, null);
                }


            }
            catch { }
        }

        private void dgvProdOrderLine_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                BOJA = dgvProdOrderLine.CurrentRow.DefaultCellStyle.BackColor.Name;
                if (BOJA == "Coral")
                {
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    dgvProdOrderLine_Click(null, null);
                }
                else if (BOJA == "LightGreen")
                {
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    dgvProdOrderLine_Click(null, null);
                }

                else if (BOJA == "LightYellow")
                {
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    dgvProdOrderLine_Click(null, null);
                }
                else if (BOJA == "MediumPurple")
                {
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkViolet;
                    dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    dgvProdOrderLine_Click(null, null);
                }

            }
            catch { }
        }

        private void dgvOutpuJournalData_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                BOJA = dgvOutpuJournalData.CurrentRow.DefaultCellStyle.BackColor.Name;
                if (BOJA == "Coral")
                {
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }
                else if (BOJA == "LightGreen")
                {
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }

                else if (BOJA == "LightYellow")
                {
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }
                else if (BOJA == "MediumPurple")
                {
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkViolet;
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }

            }
            catch { }
        }

        private void cbProdOrderNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {

                try
                {
                    string godina = DateTime.Now.Year.ToString();
                    if (cbProdOrderNo.Text.Length != 0)
                    {

                        if (cbProdOrderNo.Text.Length == 4)
                        {
                            cbProdOrderNo.Text = "RN" + godina.Substring(2, 2) + "-0" + cbProdOrderNo.Text;
                        }
                        else if (cbProdOrderNo.Text.Length == 3)
                        {
                            cbProdOrderNo.Text = "RN" + godina.Substring(2, 2) + "-00" + cbProdOrderNo.Text;
                        }
                        else if (cbProdOrderNo.Text.Length == 2)
                        {
                            cbProdOrderNo.Text = "RN" + godina.Substring(2, 2) + "-000" + cbProdOrderNo.Text;
                        }
                        else if (cbProdOrderNo.Text.Length == 1)
                        {
                            cbProdOrderNo.Text = "RN" + godina.Substring(2, 2) + "-0000" + cbProdOrderNo.Text;
                        }
                        else if (cbProdOrderNo.Text.Length == 5)
                        {
                            cbProdOrderNo.Text = "RN" + godina.Substring(2, 2) + "-" + cbProdOrderNo.Text;
                        }
                    }
                    //if (cbProdOrderNo.SelectedValue != null)
                    //{
                    //    cbProdOrderNo.SelectedValue = cbProdOrderNo.SelectedValue;
                    //}
                    cbStatus.SelectedIndex = 0;
                    btnPronadji.Focus();

                }
                catch
                {
                    MessageBox.Show("Niste uneli dobru vrednost za Broj naloga", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbProdOrderNo.Focus();
                    return;
                }
            }
        }

        private void cbStatus_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbStatus.Text != "Svi")
            {
                string status = "";
                switch (cbStatus.Text)
                {
                    case "simulirani radni nalozi":
                        status = "0";
                        break;
                    case "planirani radni nalozi":
                        status = "1";
                        break;
                    case "potvrđeni radni nalozi":
                        status = "2";
                        break;
                    case "izdati radni nalozi":
                        status = "3";
                        break;
                    case "završeni radni nalozi":
                        status = "4";
                        break;
                }
                popuniComboBoxProdOrderNo(status);
            }
            else
            {
                popuniComboBoxProdOrderNo();
            }
            cbProdOrderNo.SelectedIndex = -1;
        }

        private void dgvProdOrderLine_MouseEnter(object sender, EventArgs e)
        {
            dgvProdOrderLine.Focus();
        }

        private void dgvProdOrder_MouseEnter(object sender, EventArgs e)
        {
            dgvProdOrder.Focus();
        }

        private void dgvOutpuJournalData_MouseEnter(object sender, EventArgs e)
        {
            dgvOutpuJournalData.Focus();
        }

        private void dgvProdOrderRoutingLine_MouseEnter(object sender, EventArgs e)
        {
            dgvProdOrderRoutingLine.Focus();
        }

        private void dgvProdOrderComponent_MouseEnter(object sender, EventArgs e)
        {
            dgvProdOrderComponent.Focus();
        }

        private void dgvProdOrder_Sorted(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(prikaziProgresBar));
            thread.Start();
            foreach (DataGridViewRow row in dgvProdOrder.Rows)
            {
                row.DefaultCellStyle.BackColor = bojenjeProdOrder(row.Cells["radni nalog"].Value.ToString(), 1);

            }
            thread.Abort();

            try
            {
                BOJA = dgvProdOrder.CurrentRow.DefaultCellStyle.BackColor.Name;
            }
            catch { BOJA = ""; }
            if (BOJA == "Coral")
            {
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            }
            else if (BOJA == "LightGreen")
            {
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            }

            else if (BOJA == "LightYellow")
            {
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                dgvProdOrder.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            }

        }

        private void dgvProdOrderLine_Sorted(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvProdOrderLine.Rows)
            {
                if (double.Parse(row.Cells["preostala količina"].Value.ToString()) == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;

                }
                else if (double.Parse(row.Cells["završena količina"].Value.ToString()) == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Coral;

                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                }
                if (cbRadnik.Text != "")
                    obojiPOLfilter(row, cbRadnik.SelectedValue.ToString(), row.Cells["Broj artikla"].Value.ToString(), row.Cells["Prod_ Order No_"].Value.ToString());
            }

            BOJA = dgvProdOrderLine.CurrentRow.DefaultCellStyle.BackColor.Name;
            if (BOJA == "Coral")
            {
                dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            }
            else if (BOJA == "LightGreen")
            {
                dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            }

            else if (BOJA == "LightYellow")
            {
                dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            }
            else if (BOJA == "MediumPurple")
            {
                dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkViolet;
                dgvProdOrderLine.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dgvProdOrderLine_Click(null, null);
            }

        }

        private void bojenjeODJfilter(DataGridViewRow r)
        {
            DateTime pocetakRada = DateTime.Parse(r.Cells["Vreme prvog registrovanog početka"].Value.ToString());
            TimeSpan vremeOd;
            TimeSpan vremeDo;
            TimeSpan vreme3 = new TimeSpan(23, 59, 59);
            TimeSpan vreme33 = new TimeSpan(00, 00, 00);

            if (cbSmena.Text != "I")
            {
                if (cbSmena.Text == "II")
                {
                    vremeOd = new TimeSpan(15, 00, 00);
                    vremeDo = new TimeSpan(23, 0, 0);
                }
                else
                {
                    vremeOd = new TimeSpan(23, 00, 00);
                    vremeDo = new TimeSpan(7, 0, 0);
                }
            }
            else
            {
                vremeOd = new TimeSpan(7, 00, 00);
                vremeDo = new TimeSpan(15, 0, 0);
            }


            #region bojenjeFilteri //bojenje za izabrane filtere
            if (cbRadnik.Text != "" && cbPeriodOJD.Checked && cbSmena.Text != "" && cbRadniCentar.Text != "")
            {
                if (r.Cells["Ime resursa"].Value.ToString() == cbRadnik.Text && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) >= dtpOJDod.Value && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) <= dtpOJDdo.Value
                    && bojiSmena(pocetakRada, vremeOd, vremeDo, vreme3, vreme33, r))
                {
                    foreach (DataGridViewRow rr in dgvProdOrderRoutingLine.Rows)
                    {
                        if (rr.Cells["Broj"].Value.ToString() == cbRadniCentar.SelectedValue.ToString())
                        {
                            if (r.Cells["Broj operacije"].Value.ToString() == rr.Cells["Broj operacije"].Value.ToString())
                            {
                                r.DefaultCellStyle.BackColor = Color.MediumPurple;

                            }
                        }
                    }
                }
            }
            else if (cbRadnik.Text != "" && cbPeriodOJD.Checked && cbRadniCentar.Text != "")
            {
                if (r.Cells["Ime resursa"].Value.ToString() == cbRadnik.Text && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) >= dtpOJDod.Value && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) <= dtpOJDdo.Value)
                {

                    foreach (DataGridViewRow rr in dgvProdOrderRoutingLine.Rows)
                    {
                        if (rr.Cells["Broj"].Value.ToString() == cbRadniCentar.SelectedValue.ToString())
                        {
                            if (r.Cells["Broj operacije"].Value.ToString() == rr.Cells["Broj operacije"].Value.ToString())
                            {
                                r.DefaultCellStyle.BackColor = Color.MediumPurple;

                            }
                        }
                    }
                }

            }

            else if (cbRadnik.Text != "" && cbPeriodOJD.Checked && cbSmena.Text != "")
            {
                if (r.Cells["Ime resursa"].Value.ToString() == cbRadnik.Text && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) >= dtpOJDod.Value && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) <= dtpOJDdo.Value
                    && bojiSmena(pocetakRada, vremeOd, vremeDo, vreme3, vreme33, r))
                    r.DefaultCellStyle.BackColor = Color.MediumPurple;

            }

            else if (cbRadnik.Text != "" && cbSmena.Text != "" && cbRadniCentar.Text != "")
            {
                if (r.Cells["Ime resursa"].Value.ToString() == cbRadnik.Text && bojiSmena(pocetakRada, vremeOd, vremeDo, vreme3, vreme33, r))
                {

                    foreach (DataGridViewRow rr in dgvProdOrderRoutingLine.Rows)
                    {
                        if (rr.Cells["Broj"].Value.ToString() == cbRadniCentar.SelectedValue.ToString())
                        {
                            if (r.Cells["Broj operacije"].Value.ToString() == rr.Cells["Broj operacije"].Value.ToString())
                            {
                                r.DefaultCellStyle.BackColor = Color.MediumPurple;

                            }
                        }
                    }
                }
            }

            else if (cbSmena.Text != "" && cbPeriodOJD.Checked && cbRadniCentar.Text != "")
            {
                if (DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) >= dtpOJDod.Value && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) <= dtpOJDdo.Value
                   && bojiSmena(pocetakRada, vremeOd, vremeDo, vreme3, vreme33, r))
                {

                    foreach (DataGridViewRow rr in dgvProdOrderRoutingLine.Rows)
                    {
                        if (rr.Cells["Broj"].Value.ToString() == cbRadniCentar.SelectedValue.ToString())
                        {
                            if (r.Cells["Broj operacije"].Value.ToString() == rr.Cells["Broj operacije"].Value.ToString())
                            {
                                r.DefaultCellStyle.BackColor = Color.MediumPurple;

                            }
                        }
                    }
                }

            }


            else if (cbRadnik.Text != "" && cbPeriodOJD.Checked)
            {
                if (r.Cells["Ime resursa"].Value.ToString() == cbRadnik.Text && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) >= dtpOJDod.Value && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) <= dtpOJDdo.Value)
                    r.DefaultCellStyle.BackColor = Color.MediumPurple;

            }

            else if (cbRadnik.Text != "" && cbSmena.Text != "")
            {
                if (r.Cells["Ime resursa"].Value.ToString() == cbRadnik.Text && bojiSmena(pocetakRada, vremeOd, vremeDo, vreme3, vreme33, r))
                    r.DefaultCellStyle.BackColor = Color.MediumPurple;

            }

            else if (cbSmena.Text != "" && cbPeriodOJD.Checked)
            {
                if (DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) >= dtpOJDod.Value && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) <= dtpOJDdo.Value
                   && bojiSmena(pocetakRada, vremeOd, vremeDo, vreme3, vreme33, r))
                    r.DefaultCellStyle.BackColor = Color.MediumPurple;
            }

            else if (cbPeriodOJD.Checked && cbRadniCentar.Text != "")
            {

                if (DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) >= dtpOJDod.Value && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) <= dtpOJDdo.Value)
                {

                    foreach (DataGridViewRow rr in dgvProdOrderRoutingLine.Rows)
                    {
                        if (rr.Cells["Broj"].Value.ToString() == cbRadniCentar.SelectedValue.ToString())
                        {
                            if (r.Cells["Broj operacije"].Value.ToString() == rr.Cells["Broj operacije"].Value.ToString())
                            {
                                r.DefaultCellStyle.BackColor = Color.MediumPurple;

                            }
                        }
                    }




                }

            }
            else if (cbPeriodOJD.Checked)
            {

                if (DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) >= dtpOJDod.Value && DateTime.Parse(r.Cells["Datum knjiženja"].Value.ToString()) <= dtpOJDdo.Value)
                    r.DefaultCellStyle.BackColor = Color.MediumPurple;
            }

            else if (cbRadnik.Text != "" && cbRadniCentar.Text != "")
            {
                if (r.Cells["Ime resursa"].Value.ToString() == cbRadnik.Text)
                {

                    foreach (DataGridViewRow rr in dgvProdOrderRoutingLine.Rows)
                    {
                        if (rr.Cells["Broj"].Value.ToString() == cbRadniCentar.SelectedValue.ToString())
                        {
                            if (r.Cells["Broj operacije"].Value.ToString() == rr.Cells["Broj operacije"].Value.ToString())
                            {
                                r.DefaultCellStyle.BackColor = Color.MediumPurple;

                            }
                        }
                    }


                }

            }


            else if (cbRadnik.Text != "")
            {
                if (r.Cells["Ime resursa"].Value.ToString() == cbRadnik.Text)
                    r.DefaultCellStyle.BackColor = Color.MediumPurple;
            }

            else if (cbSmena.Text != "" && cbRadniCentar.Text != "")
            {
                if (bojiSmena(pocetakRada, vremeOd, vremeDo, vreme3, vreme33, r))
                {

                    foreach (DataGridViewRow rr in dgvProdOrderRoutingLine.Rows)
                    {
                        if (rr.Cells["Broj"].Value.ToString() == cbRadniCentar.SelectedValue.ToString())
                        {
                            if (r.Cells["Broj operacije"].Value.ToString() == rr.Cells["Broj operacije"].Value.ToString())
                            {
                                r.DefaultCellStyle.BackColor = Color.MediumPurple;

                            }
                        }
                    }


                }

            }
            else if (cbSmena.Text != "")
            {
                //if (cbSmena.Text == "III")
                //{
                //    if ((pocetakRada.TimeOfDay >= vremeOd && pocetakRada.TimeOfDay < vreme3) || (pocetakRada.TimeOfDay >= vreme33 && pocetakRada.TimeOfDay < vremeDo))
                //        r.DefaultCellStyle.BackColor = Color.MediumPurple;
                //}
                //else
                //{
                //    if (pocetakRada.TimeOfDay >= vremeOd &&  pocetakRada.TimeOfDay < vremeDo)
                //        r.DefaultCellStyle.BackColor = Color.MediumPurple;
                //}
                if (bojiSmena(pocetakRada, vremeOd, vremeDo, vreme3, vreme33, r))
                    r.DefaultCellStyle.BackColor = Color.MediumPurple;
            }

            else if (cbRadniCentar.Text != "")
            {

                foreach (DataGridViewRow rr in dgvProdOrderRoutingLine.Rows)
                {
                    if (rr.Cells["Broj"].Value.ToString() == cbRadniCentar.SelectedValue.ToString())
                    {
                        if (r.Cells["Broj operacije"].Value.ToString() == rr.Cells["Broj operacije"].Value.ToString())
                        {
                            r.DefaultCellStyle.BackColor = Color.MediumPurple;

                        }
                    }
                }


            }
            #endregion

        }


        private bool bojiSmena(DateTime pocetakRada, TimeSpan vremeOd, TimeSpan vremeDo, TimeSpan vreme3, TimeSpan vreme33, DataGridViewRow r)
        {
            if (cbSmena.Text == "III")
            {
                if ((pocetakRada.TimeOfDay >= vremeOd && pocetakRada.TimeOfDay < vreme3) || (pocetakRada.TimeOfDay >= vreme33 && pocetakRada.TimeOfDay < vremeDo))
                    return true;
            }
            else
            {
                if (pocetakRada.TimeOfDay >= vremeOd && pocetakRada.TimeOfDay < vremeDo)
                    return true;
            }
            return false;
        }
        private void dgvOutpuJournalData_Sorted(object sender, EventArgs e)
        {
            string quantity = dgvProdOrderLine.CurrentRow.Cells["količina"].Value.ToString();
            foreach (DataGridViewRow r in dgvOutpuJournalData.Rows)
            {
                if (double.Parse(r.Cells["Izlazna količina"].Value.ToString()) == double.Parse(quantity))
                {

                    r.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if (double.Parse(r.Cells["Izlazna količina"].Value.ToString()) > 0)
                {
                    r.DefaultCellStyle.BackColor = Color.LightYellow;
                }
                else
                {
                    r.DefaultCellStyle.BackColor = Color.Coral;
                }

                bojenjeODJfilter(r);
            }

            try
            {
                BOJA = dgvOutpuJournalData.CurrentRow.DefaultCellStyle.BackColor.Name;
                if (BOJA == "Coral")
                {
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkRed;
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }
                else if (BOJA == "LightGreen")
                {
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkGreen;
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }

                else if (BOJA == "LightYellow")
                {
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Goldenrod;
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }
                else if (BOJA == "MediumPurple")
                {
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkViolet;
                    dgvOutpuJournalData.CurrentRow.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                }

            }
            catch { }
        }

        private void cbPeriodOJD_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPeriodOJD.Checked)
            {
                cbPeriod.Checked = false;
                cbProdOrderNo.Text = "";
            }
        }

        private void dtpOJDod_ValueChanged(object sender, EventArgs e)
        {
            DateTime od = new DateTime(dtpOJDod.Value.Year, dtpOJDod.Value.Month, dtpOJDod.Value.Day, 0, 0, 0);
            dtpOJDod.Value = od;

        }

        private void dtpOJDdo_ValueChanged(object sender, EventArgs e)
        {
            DateTime doo = new DateTime(dtpOJDdo.Value.Year, dtpOJDdo.Value.Month, dtpOJDdo.Value.Day, 23, 59, 59);
            dtpOJDdo.Value = doo;
        }
    }

}
