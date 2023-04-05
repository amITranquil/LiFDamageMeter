using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LiFDamageMeter
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            var hit = new List<string>();
            var hitwho = new List<string>();
            var igothit = new List<string>();
            var tookhit = new List<string>();
            var killed = new List<string>();
            var enslaved = new List<string>();
            var dead = new List<string>();
            bool hasTaken = false;
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Log Files (*.log)|*.log";
                    openFileDialog.FilterIndex = 1;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {

                        var lines = File.ReadAllLines(openFileDialog.FileName);

                        foreach (var line in lines)
                        {
                            var regExp1 = new Regex("(have hit)");
                            if (regExp1.IsMatch(line))
                            {
                                var regExp3 = new Regex("(?<=\\>)(\\d*[0-9][.]\\d*[0-9])");
                                if (regExp3.IsMatch(line))
                                {
                                    hit.Add(regExp3.Match(line).Groups[1].Value);
                                }
                            }

                            var regExp2 = new Regex("(has hit)");
                            if (regExp2.IsMatch(line))
                            {
                                var regExp3 = new Regex("(?<=\\>)(\\d*[0-9][.]\\d*[0-9])");
                                if (regExp3.IsMatch(line))
                                {
                                    tookhit.Add(regExp3.Match(line).Groups[1].Value);
                                }
                            }
                            var regExp4 = new Regex(@"(?<=have killed\s+)\w+");
                            Match match = regExp4.Match(line);
                            if (match.Success)
                            {
                                killed.Add(match.Value);
                            }
                            var regExp5 = new Regex(@"(?<=have enslaved <spush><color:C65F5F>)\w+\s+\w+");
                            Match matchEnslave = regExp5.Match(line);
                            if (matchEnslave.Success)
                            {
                                enslaved.Add(matchEnslave.Value);
                            }

                            var regExp6 = new Regex(@"(?<=have hit\s+)[\w-]+");
                            Match matchHitwho = regExp6.Match(line);
                            if (matchHitwho.Success)
                            {
                                hitwho.Add("to " + matchHitwho.Value);
                            }

                            var regExp7 = new Regex(@"([\w-]+(?<!-))[\w-]+<spop> has hit you");
                            Match matchTakeDamage = regExp7.Match(line);
                            if (matchTakeDamage.Success)
                            {
                                igothit.Add("from " + matchTakeDamage.Groups[1].Value);
                            }
                            var regExp99 = new Regex("(DeathWindow)");
                            if (regExp99.IsMatch(line))
                            { hasTaken = true; }
                            else if (hasTaken)
                            {
                                var regexDeath = new Regex("(Message: <spush><color:C65F5F>)[\\w-]+");
                                if (regexDeath.IsMatch(line))
                                {
                                    string matchedString = regexDeath.Match(line).Value;
                                    int index = matchedString.IndexOf('>') + 15 ;
                                    string output = matchedString.Substring(index);
                                    dead.Add(output);
                                    hasTaken = false;
                                }
                            }


                        }


                        var totalHit = hit.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).Sum();
                        lblTotalHit.Text = totalHit.ToString();
                        var totalTookHit = tookhit.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).Sum();
                        lblTotalTakenHit.Text = totalTookHit.ToString();
                        lblTotalKill.Text = killed.Count.ToString();
                        lblTotalSlave.Text = enslaved.Count.ToString();



                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Clear();
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


                        dataGridView1.Columns.Add("Hit", "Dealed Damages");
                        dataGridView1.Columns.Add("Hitwho", "Who Did You Hit");
                        dataGridView1.Columns.Add("TookHit", "Taken Damages");
                        dataGridView1.Columns.Add("Igothit", "Who Hit You");
                        dataGridView1.Columns.Add("Killed", "Killed");
                        dataGridView1.Columns.Add("Enslaved", "Enslaved");
                        dataGridView1.Columns.Add("Death", "Death");



                        int maxRowCount = Math.Max(hit.Count, hitwho.Count);
                        for (int i = 0; i < maxRowCount; i++)
                        {
                            if (i < hit.Count)
                            {
                                dataGridView1.Rows.Add(hit[i]);
                            }
                            else
                            {
                                dataGridView1.Rows.Add("");
                            }
                            if (i < hitwho.Count)
                            {
                                dataGridView1.Rows[i].Cells[1].Value = hitwho[i];
                            }
                            else
                            {
                                dataGridView1.Rows[i].Cells[1].Value = "";

                            }
                        }
                        for (int w = 0; w < maxRowCount; w++)
                        {

                            if (w < tookhit.Count)
                            {
                                dataGridView1.Rows[w].Cells[2].Value = tookhit[w];
                            }
                            else
                            {
                                dataGridView1.Rows[w].Cells[2].Value = "";
                            }


                            if (w < igothit.Count)
                            {
                                dataGridView1.Rows[w].Cells[3].Value = igothit[w];
                            }
                            else
                            {
                                dataGridView1.Rows[w].Cells[3].Value = "";
                            }
                        }

                        for (int j = 0; j < killed.Count; j++)
                        {
                            if (j < killed.Count)
                            {
                                dataGridView1.Rows[j].Cells[4].Value = killed[j];
                            }
                            else
                            {
                                dataGridView1.Rows[j].Cells[4].Value = "";
                            }

                        }
                        for (int k = 0; k < enslaved.Count; k++)
                        {
                            if (k < enslaved.Count)
                            {
                                dataGridView1.Rows[k].Cells[5].Value = enslaved[k];
                            }
                            else
                            {
                                dataGridView1.Rows[k].Cells[5].Value = "";
                            }
                        }
                        for (int d = 0; d < dead.Count; d++)
                        {
                            if (d < dead.Count)
                            {
                                dataGridView1.Rows[d].Cells[6].Value = dead[d];
                            }
                            else
                            {
                                dataGridView1.Rows[d].Cells[6].Value = "";
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                string a;
                a= ex.Message;
                a = "";
                MessageBox.Show("Log-Parse-Issue");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}