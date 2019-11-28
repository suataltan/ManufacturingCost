using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UretimMaliyeti.Model;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;


namespace UretimMaliyeti
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        d1Entities db = new d1Entities();

        //---------------------------------------------------------------------------------------------------------------------------------

        int[] r1 = new int[1];
        (List<int>, List<float>, List<float>, List<int>, List<int>) Recete_Al(int id)
        {
            int urunID = id;
            int[] receteust = new int[1];
            int[] ustbirimFiyat = new int[1];
            int[] ustbirimMiktar = new int[1];

            var listeRid = db.depo.Where(x => x.urunID == urunID).Where(y => y.derece == 0).Select(x => new { x.receteID }).ToArray();
            var listeBM = db.depo.Where(x => x.urunID == urunID).Where(y => y.derece == 0).Select(x => new { x.birimMiktar }).ToArray();
            var listeBF = db.depo.Where(x => x.urunID == urunID).Where(y => y.derece == 0).Select(x => new { x.birimFiyat }).ToArray();
            foreach (var item in listeRid)
            {
                receteust[0] = Convert.ToInt32(item.receteID);
            }
            foreach (var item in listeBM)
            {
                ustbirimMiktar[0] = Convert.ToInt32(item.birimMiktar);
            }
            foreach (var item in listeBF)
            {
                ustbirimFiyat[0] = Convert.ToInt32(item.birimFiyat);
            }
            int tmp = receteust[0];
            int tmp2 = r1[0];

            var liste2 = db.depo.Where(x => x.receteID == tmp).Select(x => new { x.urunID }).ToList();
            var liste3 = db.depo.Where(x => x.receteID == tmp).Select(x => new { x.birimMiktar }).ToList();
            var liste4 = db.depo.Where(x => x.receteID == tmp).Select(x => new { x.birimFiyat }).ToList();
            var liste5 = db.depo.Where(x => x.receteID == tmp).Select(x => new { x.receteID }).ToList();
            var liste6 = db.depo.Where(x => x.receteID == tmp).Select(x => new { x.derece }).ToList();

            List<int> rIDs = new List<int>();
            List<float> rBMs = new List<float>();
            List<float> rBFs = new List<float>();
            List<int> rRids = new List<int>();
            List<int> rDs = new List<int>();
            foreach (var item in liste2)
            {
                rIDs.Add(Convert.ToInt32(item.urunID));
            }
            foreach (var item in liste3)
            {
                rBMs.Add(Convert.ToInt32(item.birimMiktar));
            }
            foreach (var item in liste4)
            {
                rBFs.Add(Convert.ToInt32(item.birimFiyat));
            }
            foreach (var item in liste5)
            {
                rRids.Add(Convert.ToInt32(item.receteID));
            }
            foreach (var item in liste6)
            {
                rDs.Add(Convert.ToInt32(item.derece));
            }

            return (rIDs, rBMs, rBFs, rRids, rDs);
        }

        //---------------------------------------------------------------------------------------------------------------------------------

        int sayac = 0;
        (List<int>, List<float>, List<float>, List<int>, List<int>) Recete_Ac(List<int> id, List<float> bm, List<float> bf, List<int> r, List<int> d)
        {
            List<int> id2 = new List<int>();
            List<float> bm2 = new List<float>();
            List<float> bf2 = new List<float>();
            List<int> r2 = new List<int>();
            List<int> d2 = new List<int>();
            List<int> acilacakID = new List<int>();

            List<int> id3 = new List<int>();
            List<float> bm3 = new List<float>();
            List<float> bf3 = new List<float>();
            List<int> r3 = new List<int>();
            List<int> d3 = new List<int>();

            id3.AddRange(id);
            bm3.AddRange(bm);
            bf3.AddRange(bf);
            r3.AddRange(r);
            d3.AddRange(d);


            acilacakID.AddRange(id);
            if (acilacakID[0] == id[0] && sayac == 0 && id.Count() > 1)
                acilacakID.RemoveAt(0);


            id.Clear();
            bm.Clear();
            bf.Clear();
            r.Clear();
            d.Clear();

            for (int i = 0; i < acilacakID.Count(); i++)
            {

                (id2, bm2, bf2, r2, d2) = Recete_Al(acilacakID[i]);

                id.AddRange(id2);
                bm.AddRange(bm2);
                bf.AddRange(bf2);
                r.AddRange(r2);
                d.AddRange(d2);
            }

            if (d.Find(x => x == 1) > 0)
            {
                List<int> tmpid = new List<int>();

                int u;
                List<int> tmpCnt = new List<int>();
                for (int rindex = 1; rindex < r.Count() + 1; rindex++)
                {
                    u = r.Count() - rindex;
                    int tmpru = id[u];
                    var a = db.depo.Where(x => x.receteID == tmpru).Select(x => new { x.receteID }).ToList();
                    int k = a.Count();
                    if (k > 1 && r[u] == id[u])
                        tmpid.Add(u);
                }

                foreach (var item in tmpid)
                {
                    id.RemoveAt(item);
                    bm.RemoveAt(item);
                    bf.RemoveAt(item);
                    r.RemoveAt(item);
                    d.RemoveAt(item);
                }

            }

            for (int i = 0; i < id.Count(); i++)
            {
                int recfix = id[i];
                var a = db.depo.Where(x => x.receteID == recfix).Select(x => new { x.receteID }).ToList();
                if (a.Count() > 1)
                    d[i] = 1;
                else
                {
                    d[i] = 0;

                }
            }

            for (int i = 0; i < r.Count(); i++)
            {
                if (r[i] == id[i])
                {
                    List<int> c1 = new List<int>();
                    c1.AddRange(id3.FindAll(x => x == r[i]));
                    int c2 = id3.FindIndex(x => x == r[i]);
                    r[i] = r3[c2];
                    if (c1.Count() > 1)
                    {
                        id3.RemoveAt(c2);
                        r3.RemoveAt(c2);
                    }

                }
            }

            sayac++;
            while (d.Contains(1))
                (id, bm, bf, r, d) = Recete_Ac(id, bm, bf, r, d);

            return (id, bm, bf, r, d);
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        (List<int>, List<float>, List<float>, float, List<float>, List<float>) Recete_Topla(int a, int gerekliMiktar)
        {
            List<int> ID;
            List<float> Miktar;
            List<float> Fiyat;
            List<int> r3;
            List<int> d3;

            (ID, Miktar, Fiyat, r3, d3) = Recete_Al(a);
            float birimMiktar = Miktar[0];
            var rec = Recete_Ac(ID, Miktar, Fiyat, r3, d3);
            float toplamFiyat = 0;

            List<float> bmr2 = new List<float>();
            List<float> bfr2 = new List<float>();
            List<float> stok = new List<float>();
            List<float> stokToplam = new List<float>();
            for (int i = 0; i < rec.Item1.Count(); i++)
            {
                int uid = a;
                int aid = rec.Item1[i];
                int ir = rec.Item4[i];
                var bmr = db.depo.Where(x => x.urunID == aid).Where(x => x.receteID == ir).Select(x => new { x.birimMiktar }).ToList();
                var bfr = db.depo.Where(x => x.urunID == aid).Where(x => x.receteID == ir).Select(x => new { x.birimFiyat }).ToList();
                var s = db.Warehouse.Where(x => x.urunID == aid).Select(x => new { x.stok }).ToList();

                foreach (var item in s)
                {
                    stok.Add(Convert.ToInt32(item.stok));
                    stokToplam.Add(Convert.ToInt32(item.stok));
                }

                if (bmr.Count() > 0)
                {
                    foreach (var item in bmr)
                    {
                        bmr2.Add(Convert.ToInt32(item.birimMiktar));
                    }
                    foreach (var item in bfr)
                    {
                        bfr2.Add(Convert.ToInt32(item.birimFiyat));
                    }
                    rec.Item2[i] = bmr2[0];
                    rec.Item3[i] = bfr2[0];
                    bmr2.RemoveAt(0);
                    bfr2.RemoveAt(0);
                }
            }

            for (int i = 0; i < rec.Item1.Count(); i++)
            {

                rec.Item2[i] = (rec.Item2[i] * gerekliMiktar) / birimMiktar;
                if (rec.Item2[i] < stok[i])
                {
                    stok[i] = stok[i] - rec.Item2[i];
                    rec.Item2[i] = 0;
                }
                else
                {
                    rec.Item2[i] = rec.Item2[i] - stok[i];
                    stok[i] = 0;
                }
                stokToplam[i] = stokToplam[i] - stok[i];
                rec.Item3[i] = rec.Item2[i] * rec.Item3[i];
                toplamFiyat += rec.Item3[i];
            }

            for (int i = 0; i < rec.Item1.Count(); i++)
            {
                for (int j = 0; j < rec.Item1.Count(); j++)
                {
                    if (rec.Item1[i] == rec.Item1[j] && i < j)
                    {
                        rec.Item2[i] = rec.Item2[i] + rec.Item2[j];
                        rec.Item3[i] = rec.Item3[i] + rec.Item3[j];
                        rec.Item1.RemoveAt(j);
                        rec.Item2.RemoveAt(j);
                        rec.Item3.RemoveAt(j);
                        j--;
                    }
                }
            }

            return (rec.Item1, rec.Item2, rec.Item3, toplamFiyat, stok, stokToplam);
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            List<int> ID = new List<int>();
            List<float> Miktar = new List<float>();
            List<float> Fiyat = new List<float>();
            sayac = 0;
            textBox3.Clear();
            int i = Convert.ToInt32(textBox1.Text);
            int j = Convert.ToInt32(textBox2.Text);

            //var ri = db.depo.Where(x => x.urunID == i).Where(y => y.derece == 0).Select(x => new { x.receteID }).ToArray();
            //foreach (var item in ri)
            //{
            //    r1[0] = Convert.ToInt32(item.receteID);
            //}


            var son = Recete_Topla(i, j);
            foreach (var item in son.Item1)
            {
                ID.Add(item);
            }
            foreach (var item in son.Item2)
            {
                Miktar.Add(item);
            }
            foreach (var item in son.Item3)
            {
                Fiyat.Add(item);
            }
            float toplamFiyat = son.Item4;

            for (int k = 0; k < ID.Count(); k++)
            {
                textBox3.Text += "ID = " + ID[k] + "        Miktar = " + Miktar[k] + "       Fiyat = " + Fiyat[k];
                textBox3.Text += "\r\n";
            }
            textBox3.Text += "\r\n";
            textBox3.Text += "Toplam Fiyat: " + toplamFiyat;
        }


        //----------------------------------------------------------------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            sayac = 0;
            List<int> ID = new List<int>();
            List<float> Miktar = new List<float>();
            List<float> Fiyat = new List<float>();
            int i = Convert.ToInt32(textBox1.Text);
            int j = Convert.ToInt32(textBox2.Text);

            var son = Recete_Topla(i, j);
            foreach (var item in son.Item1)
            {
                ID.Add(item);
            }
            foreach (var item in son.Item2)
            {
                Miktar.Add(item);
            }
            foreach (var item in son.Item3)
            {
                Fiyat.Add(item);
            }
            float toplamFiyat = son.Item4;

            //id(son.item1), miktar, fiyat, kullanılan stok, kalan stok
            var maliyet = new List<XlsxO>();
            {
                for (int k = 0; k < Miktar.Count(); k++)
                {
                    maliyet.Add(new XlsxO() { xID = son.Item1[k], xGM = Miktar[k], xFiyat = Fiyat[k], xKullanilanStok = son.Item6[k], xKalanStok = son.Item5[k] });
                }
                CreateSpreadsheet(maliyet);
                MessageBox.Show("all done");
            };

        }
        private void CreateSpreadsheet(List<XlsxO> maliyet)
        {
            string spreadsheetPath = "maliyet.xlsx";
            File.Delete(spreadsheetPath);
            FileInfo spreadsheetInfo = new FileInfo(spreadsheetPath);

            ExcelPackage pck = new ExcelPackage(spreadsheetInfo);
            var activitiesWorksheet = pck.Workbook.Worksheets.Add("Maliyet");
            activitiesWorksheet.Cells["A1"].Value = "Ürün ID";
            activitiesWorksheet.Cells["B1"].Value = "Gerekli Miktar";
            activitiesWorksheet.Cells["C1"].Value = "Fiyat";
            activitiesWorksheet.Cells["D1"].Value = "Kullanılan Stok";
            activitiesWorksheet.Cells["E1"].Value = "Kalan Stok";
            activitiesWorksheet.Cells["A1:E1"].Style.Font.Bold = true;

            // populate spreadsheet with data
            int currentRow = 2;
            foreach (var activity in maliyet)
            {
                activitiesWorksheet.Cells["A" + currentRow.ToString()].Value = activity.xID;
                activitiesWorksheet.Cells["B" + currentRow.ToString()].Value = activity.xGM;
                activitiesWorksheet.Cells["C" + currentRow.ToString()].Value = activity.xFiyat;
                activitiesWorksheet.Cells["D" + currentRow.ToString()].Value = activity.xKullanilanStok;
                activitiesWorksheet.Cells["E" + currentRow.ToString()].Value = activity.xKalanStok;

                currentRow++;
            }

            activitiesWorksheet.View.FreezePanes(2, 1);

            //activitiesWorksheet.Cells["B" + (currentRow).ToString()].Formula = "SUM(B2:B" + (currentRow - 1).ToString() + ")";
            activitiesWorksheet.Cells["C" + (currentRow).ToString()].Formula = "SUM(C2:C" + (currentRow - 1).ToString() + ")";
            //activitiesWorksheet.Cells["D" + (currentRow).ToString()].Formula = "SUM(D2:D" + (currentRow - 1).ToString() + ")";
            //activitiesWorksheet.Cells["B" + (currentRow).ToString()].Style.Font.Bold = true;
            activitiesWorksheet.Cells["C" + (currentRow).ToString()].Style.Font.Bold = true;
            //activitiesWorksheet.Cells["D" + (currentRow).ToString()].Style.Font.Bold = true;
            //activitiesWorksheet.Cells["B" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            activitiesWorksheet.Cells["C" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            //activitiesWorksheet.Cells["D" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            pck.Save();
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(463, 78);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(463, 162);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(253, 219);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(481, 166);
            this.textBox3.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(770, 277);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(770, 336);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1117, 504);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private Button button1;
        private Button button2;
    }
}

