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
            
            // Premium zebra pattern events
            SetupPremiumZebraPattern();
        }

        private void SetupPremiumZebraPattern()
        {
            // Mouse hover effect for rows
            dataGridView1.CellMouseEnter += (sender, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(70, 70, 85);
                }
            };

            dataGridView1.CellMouseLeave += (sender, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    // Restore zebra pattern
                    if (e.RowIndex % 2 == 0)
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 45);
                    else
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(50, 50, 55);
                }
            };
        }

        private string ExtractTimestamp(string line)
        {
            // Different timestamp patterns in log files
            var patterns = new[]
            {
                @"\[(\d{2}:\d{2}:\d{2})\]",           // [HH:MM:SS]
                @"(\d{2}:\d{2}:\d{2})",               // HH:MM:SS
                @"\[(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\]", // [YYYY-MM-DD HH:MM:SS]
                @"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})",     // YYYY-MM-DD HH:MM:SS
                @"(\d{1,2}:\d{2}:\d{2} \w{2})",       // H:MM:SS AM/PM
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return DateTime.Now.ToString("HH:mm:ss"); // Fallback to current time
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
            
            // Zaman damgaları için listeler
            var hitTimes = new List<string>();
            var hitwhoTimes = new List<string>();
            var igothitTimes = new List<string>();
            var tookhitTimes = new List<string>();
            var killedTimes = new List<string>();
            var enslavedTimes = new List<string>();
            var deadTimes = new List<string>();
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
                            // DÜZELTİLDİ: "You have hit" mesajlarını yakala
                            var regExp1 = new Regex(@"You have hit");
                            if (regExp1.IsMatch(line))
                            {
                                // DÜZELTİLDİ: HTML etiketleri içindeki hasar değerini al
                                // Örnek: <spush><color:C65F5F>29.36<spop>
                                var regExp3 = new Regex(@"<spush><color:C65F5F>(\d+\.\d+)<spop> of");
                                var match = regExp3.Match(line);
                                if (match.Success)
                                {
                                    hit.Add(match.Groups[1].Value);
                                    hitTimes.Add(ExtractTimestamp(line));
                                }

                                // DÜZELTİLDİ: Kime vurduğunu al (HTML etiketleri içinden)
                                var regExpWho = new Regex(@"You have hit <spush><color:C65F5F>([^<]+)<spop>");
                                var matchWho = regExpWho.Match(line);
                                if (matchWho.Success)
                                {
                                    hitwho.Add("to " + matchWho.Groups[1].Value);
                                    hitwhoTimes.Add(ExtractTimestamp(line));
                                }
                            }

                            // DÜZELTİLDİ: "has hit you" mesajlarını yakala
                            var regExp2 = new Regex(@"has hit you");
                            if (regExp2.IsMatch(line))
                            {
                                // DÜZELTİLDİ: Aldığın hasar değerini al
                                var regExp3 = new Regex(@"<spush><color:C65F5F>(\d+\.\d+)<spop> of");
                                var match = regExp3.Match(line);
                                if (match.Success)
                                {
                                    tookhit.Add(match.Groups[1].Value);
                                    tookhitTimes.Add(ExtractTimestamp(line));
                                }

                                // DÜZELTİLDİ: Kim vurduğunu al
                                var regExpWho = new Regex(@"<spush><color:C65F5F>([^<]+)<spop> has hit you");
                                var matchWho = regExpWho.Match(line);
                                if (matchWho.Success)
                                {
                                    igothit.Add("from " + matchWho.Groups[1].Value);
                                    igothitTimes.Add(ExtractTimestamp(line));
                                }
                            }

                            // Killed - multiple patterns to catch different formats
                            var regExp4 = new Regex(@"have killed\s+<spush><color:C65F5F>([^<]+)<spop>");
                            Match matchKill = regExp4.Match(line);
                            if (matchKill.Success)
                            {
                                killed.Add(matchKill.Groups[1].Value);
                                killedTimes.Add(ExtractTimestamp(line));
                            }
                            else
                            {
                                // Alternative kill pattern
                                var regExp4Alt = new Regex(@"You have killed\s+([^<>\r\n]+)");
                                var matchKillAlt = regExp4Alt.Match(line);
                                if (matchKillAlt.Success)
                                {
                                    killed.Add(matchKillAlt.Groups[1].Value.Trim());
                                    killedTimes.Add(ExtractTimestamp(line));
                                }
                                else
                                {
                                    // Even more basic kill pattern
                                    var regExp4Basic = new Regex(@"killed\s+([A-Za-z0-9_]+)");
                                    var matchKillBasic = regExp4Basic.Match(line);
                                    if (matchKillBasic.Success)
                                    {
                                        killed.Add(matchKillBasic.Groups[1].Value);
                                        killedTimes.Add(ExtractTimestamp(line));
                                    }
                                }
                            }

                            // Enslaved - zaten doğru çalışıyor
                            var regExp5 = new Regex(@"have enslaved <spush><color:C65F5F>([^<]+)<spop>");
                            Match matchEnslave = regExp5.Match(line);
                            if (matchEnslave.Success)
                            {
                                enslaved.Add(matchEnslave.Groups[1].Value);
                                enslavedTimes.Add(ExtractTimestamp(line));
                            }

                            // Death detection
                            var regExp99 = new Regex("(DeathWindow)");
                            if (regExp99.IsMatch(line))
                            {
                                hasTaken = true;
                            }
                            else if (hasTaken)
                            {
                                // DÜZELTİLDİ: Death mesajını daha güvenli parse et
                                var regexDeath = new Regex(@"Message: <spush><color:C65F5F>([^<]+)<spop>");
                                var matchDeath = regexDeath.Match(line);
                                if (matchDeath.Success)
                                {
                                    dead.Add(matchDeath.Groups[1].Value);
                                    deadTimes.Add(ExtractTimestamp(line));
                                    hasTaken = false;
                                }
                            }
                        }

                        // Toplamları hesapla
                        var totalHit = hit.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).Sum();
                        lblTotalHit.Text = totalHit.ToString("F2");

                        var totalTookHit = tookhit.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).Sum();
                        lblTotalTakenHit.Text = totalTookHit.ToString("F2");

                        lblTotalKill.Text = killed.Count.ToString();
                        lblTotalSlave.Text = enslaved.Count.ToString();
                        lblTotalDeath.Text = dead.Count.ToString();

                        // DataGridView'i hazırla
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Clear();
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        // Kategorize edilmiş kolonlar
                        dataGridView1.Columns.Add("DamageDealt", "💀 Dealt Damage");
                        dataGridView1.Columns.Add("DamageTarget", "🎯 Who Hit");
                        dataGridView1.Columns.Add("DamageTime", "🕒 Time");
                        dataGridView1.Columns.Add("DamageTaken", "🛡️ Taken Damage");
                        dataGridView1.Columns.Add("DamageSource", "⚔️ Who Hit You");
                        dataGridView1.Columns.Add("TakenTime", "🕒 Time");
                        dataGridView1.Columns.Add("Kills", "💀 Kills");
                        dataGridView1.Columns.Add("KillTime", "🕒 Time");
                        dataGridView1.Columns.Add("Deaths", "☠️ Deaths");
                        dataGridView1.Columns.Add("DeathTime", "🕒 Time");
                        dataGridView1.Columns.Add("Enslaved", "⛓️ Enslaved");
                        dataGridView1.Columns.Add("EnslavedTime", "🕒 Time");

                        // Kolon genişlikleri - damage kolonları daha geniş
                        dataGridView1.Columns["DamageDealt"].FillWeight = 70;   // Dar - sadece sayılar
                        dataGridView1.Columns["DamageTarget"].FillWeight = 150;  // Geniş - isimler
                        dataGridView1.Columns["DamageTime"].FillWeight = 60;     // Dar - zaman
                        dataGridView1.Columns["DamageTaken"].FillWeight = 70;   // Dar - sadece sayılar  
                        dataGridView1.Columns["DamageSource"].FillWeight = 150; // Geniş - isimler
                        dataGridView1.Columns["TakenTime"].FillWeight = 60;     // Dar - zaman
                        dataGridView1.Columns["Kills"].FillWeight = 100;        // Orta - isimler
                        dataGridView1.Columns["KillTime"].FillWeight = 60;      // Dar - zaman
                        dataGridView1.Columns["Deaths"].FillWeight = 100;       // Orta - isimler
                        dataGridView1.Columns["DeathTime"].FillWeight = 60;     // Dar - zaman
                        dataGridView1.Columns["Enslaved"].FillWeight = 100;     // Orta - isimler
                        dataGridView1.Columns["EnslavedTime"].FillWeight = 60;  // Dar - zaman

                        // En uzun liste uzunluğunu bul
                        int maxRowCount = new[] {
                            hit.Count,
                            hitwho.Count,
                            tookhit.Count,
                            igothit.Count,
                            killed.Count,
                            dead.Count,
                            enslaved.Count
                        }.Max();

                        // Satırları doldur ve zebra pattern uygula
                        for (int i = 0; i < maxRowCount; i++)
                        {
                            int rowIndex = dataGridView1.Rows.Add(
                                i < hit.Count ? hit[i] : "",
                                i < hitwho.Count ? hitwho[i] : "",
                                i < hitTimes.Count ? hitTimes[i] : "",
                                i < tookhit.Count ? tookhit[i] : "",
                                i < igothit.Count ? igothit[i] : "",
                                i < tookhitTimes.Count ? tookhitTimes[i] : "",
                                i < killed.Count ? killed[i] : "",
                                i < killedTimes.Count ? killedTimes[i] : "",
                                i < dead.Count ? dead[i] : "",
                                i < deadTimes.Count ? deadTimes[i] : "",
                                i < enslaved.Count ? enslaved[i] : "",
                                i < enslavedTimes.Count ? enslavedTimes[i] : ""
                            );

                            // Güzel zebra pattern - her satır farklı renk
                            if (rowIndex % 2 == 0)
                            {
                                // Çift satırlar - daha koyu
                                dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 45);
                                dataGridView1.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
                            }
                            else
                            {
                                // Tek satırlar - daha açık
                                dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(50, 50, 55);
                                dataGridView1.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.White;
                            }

                            // Seçili satır rengi
                            dataGridView1.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 71, 85);
                            dataGridView1.Rows[rowIndex].DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Log Parse Error: {ex.Message}\n\nDetails: {ex.StackTrace}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
