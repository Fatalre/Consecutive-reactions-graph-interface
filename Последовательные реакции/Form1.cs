using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Последовательные_реакции
{
    public partial class Form1 : Form
    {
        string g;
        double l = 0, a;
        int p, i = 0, w = 0;
        private double[] tau;
        private double[] Ca;
        private double[] Cb;
        private double[] Cd;
        public Form1()
        {
            InitializeComponent();
        }

        private void B2_Click(object sender, EventArgs e)
        {
            double t, y, u, d = 0, s = 0, r = 0;
            double k1;
            double k2;
            double x1;
            double x2;
            string m = " c";
            int ii;
            a = Convert.ToDouble(TB1.Text);
            if (a < 0)
            {
                MessageBox.Show("Концентрация НЕ может быть меньше нуля!\n\tПоменяйте концентрацию!");
                return;
            }
            if (a == 0)
            {
                MessageBox.Show("Концентрация НЕ может равняться нулю!\n\tПоменяйте концентрацию!");
            }
            u = Convert.ToDouble(TB2.Text);
            if (u < 0)
            {
                MessageBox.Show("Время НЕ может быть меньше нуля!\n\tПоменяйте время!");
                return;
            }
            t = Convert.ToDouble(TB3.Text);
            if (t < 0)
            {
                MessageBox.Show("Время НЕ может быть меньше нуля!\n\tПоменяйте время!");
            }
            if (u > t)
            {
                MessageBox.Show("Начальное время протекания реакции НЕ может быть больше, чем конечное время протекания реакции!\n\t\tПоменяйте время!!!");
                return;
            }
            if (u == t)
            {
                MessageBox.Show("Конечное и начальное время не может равняться друг другу!\n\t\tПоменяйте время!");
                return;
            }
            y = Convert.ToDouble(TB4.Text);
            if (y < 0.5)
            {
                MessageBox.Show("Шаг по времени НЕ может быть меньше, чем 0.5\n\t\tПоменяйте шаг!");
                return;
            }
            if (y > t)
            {
                MessageBox.Show("Шаг по времени НЕ может быть больше, чем конечное время протекания реакции!\n\t\tПоменяйте шаг!!!");
                return;
            }
            if (y == t)
            {
                MessageBox.Show("Шаг по времени НЕ может быть равным конечному времени протекания реакции!\n\t\tПоменяйте шаг!!!");
                return;
            }
            k1 = 0.001;
            k2 = 0.002;
            r = t - u;
            ii = Convert.ToInt32(t / y) + 1;
            if (ii > r)
            {
                ii = Convert.ToInt32(r + 1);
            }
            tau = new double[ii];
            Ca = new double[ii];
            Cb = new double[ii];
            Cd = new double[ii];
            tau[0] = Convert.ToDouble(u);
            for (i = 1; i <= ii - 1; i = i + 1)
            {

                tau[i] = tau[i - 1] + y;
                l = tau[i] + 0.2;
            }
            for (i = 0; i <= 1; i = i + 1)
            {
                x1 = -k1 * tau[i];
                x2 = -k2 * tau[i];
                Ca[i] = a * Math.Pow(Math.E, x1);
                Cb[i] = (k1 * a) / (k2 - k1) * (Math.Pow(Math.E, x1) - Math.Pow(Math.E, x2));
                Cd[i] = a - Ca[i] - Cb[i];
                if (Ca[i] < 0)
                {
                    Ca[i] = Ca[i] * (-1);
                }
                if (Cb[i] < 0)
                {
                    Cb[i] = Cb[i] * (-1);
                }
                if (Cd[i] < 0)
                {
                    Cd[i] = Cd[i] * (-1);
                }
            }
            k1 = Cb[1] / Ca[1];
            k2 = Cd[1] / Cb[1];
            for (i = 2; i <= ii - 1; i = i + 1)
            {
                x1 = -k1 * tau[i];
                x2 = -k2 * tau[i];
                Ca[i] = a * Math.Pow(Math.E, x1);
                Cb[i] = ((k1 * a) / (k2 - k1)) * (Math.Pow(Math.E, x1) - Math.Pow(Math.E, x2));
                Cd[i] = a - Ca[i] - Cb[i];
                if (Ca[i] < 0)
                {
                    Ca[i] = Ca[i] * (-1);
                }
                if (Cb[i] < 0)
                {
                    Cb[i] = Cb[i] * (-1);
                }
                if (Cd[i] < 0)
                {
                    Cd[i] = Cd[i] * (-1);
                }
                if (Ca[i] == 0)
                {
                    s = tau[i];
                    Ca[i] = 0.001;
                    Cb[i] = 0.001;
                    if (Cb[i] == 0.001)
                    {
                        d = s + y;
                    }
                    if (Cd[i] < a)
                    {
                        Cd[i] = Cd[i] + Cd[i];
                        if (Cd[i] > a)
                        {
                            Cd[i] = a - 0.014764;
                        }
                    }
                    goto M1;
                }
                p = i + 1;
                if (r + 1 == ii)
                {
                    p = i;
                }
                k1 = Cb[i] / Ca[i];
                k2 = Cd[i] / Cb[i];
            }
            M1:
            if (s == 0 && d == 0)
            {
                m += " (реакция еще не прекратилась)";
            }
            RTB1.Text += ("Последовательные реакции вида: \n" + "\ta -> b \n\t b -> d" + "\n" + "Начальная концентрация компонента 'a', в моль/л: " + a.ToString() + "\n" + "Начальное время реакции, в секундах: " + u.ToString() + "\n" + "Конечное время протекания реакции, в секундах: " + t.ToString() + "\n" + "Шаг по времени: " + y.ToString() + "\n" + "Реакция a -> b прекратилась на: " + s.ToString() + m.ToString() + "\n" + "Реакция b -> d прекратилась на: " + d.ToString() + m.ToString() + "\n\n" + "Время" + "\t" + "Ca" + "\t" + "Cb" + "\t" + "Cd" + "\n\n");
            for (i = 0; i < p + 2; i = i + 1)
            {
                RTB1.Text += (tau[i].ToString() + "\t" + Ca[i].ToString("0.000") + "\t" + Cb[i].ToString("0.000") + "\t" + Cd[i].ToString("0.00000000") + "\n");
            }
            B1.Enabled = true;
            B3.Enabled = true;
            B4.Enabled = true;
            TB1.ReadOnly = true;
            TB2.ReadOnly = true;
            TB3.ReadOnly = true;
            TB4.ReadOnly = true;
            this.tabControl1.SelectedTab = TP2;
            i = 0;
        }

        private void B1_Click(object sender, EventArgs e)
        {
            B2.Enabled = true;
            B1.Enabled = false;
            B3.Enabled = false;
            B4.Enabled = false;

            TB1.Clear();
            TB2.Clear();
            TB3.Clear();
            TB4.Clear();

            TB1.ReadOnly = false;
            TB2.ReadOnly = false;
            TB3.ReadOnly = false;
            TB4.ReadOnly = false;
        }

        private void B3_Click(object sender, EventArgs e)
        {
            RTB1.Clear();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.DefaultExt = "*.rez";
            saveFile1.Filter = "Text files|*.rez";
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                saveFile1.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(saveFile1.FileName, true))
                {
                    sw.WriteLine(RTB1.Text);
                    sw.Close();
                }
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Текстовые файлы|*.rez";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    RTB1.Text = File.ReadAllText(dialog.FileName);
                }
            }
        }

        private void вставитьДатуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RTB1.AppendText("\n" + DateTime.Now.ToShortDateString());
        }

        private void вставитьВремяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RTB1.AppendText("\t" + DateTime.Now.ToShortTimeString() + "\n");
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа для моделирования процесса протекания последовательных реакций. Вида:\n\t a -> b\n\tb -> d\nПостроения графика в координатах концентрация - время.\n\nАвтор программы: Группа CITG\nРуководитель: Игорь Мукминов\ne-mail: fatalrew@gmail.com");
        }

        private void B5_Click(object sender, EventArgs e)
        {
            L1.Text = "0";
            B5.Enabled = false;
            B6.Enabled = true;
            for (int i = 0; i < 3; i++)
            {
                CH1.Series[i].Points.Clear();
            }
        }

        private void сохранитьКакКартинкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить картинку как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Image Files(*.JPG)|*.JPG";
            savedialog.ShowHelp = true;
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CH1.SaveImage(savedialog.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Jpeg);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void B4_Click(object sender, EventArgs e)
        {
            g = CMB1.Text;
            switch (g)
            {
                case "Быстрый график":
                    {
                        B6.Enabled = false;
                        for (i = 0; i < p + 2; i++)
                        {
                            CH1.Series[0].Points.AddXY(tau[i], Ca[i]);
                            CH1.Series[1].Points.AddXY(tau[i], Cb[i]);
                            CH1.Series[2].Points.AddXY(tau[i], Cd[i]);
                        }

                        //CH1.ChartAreas[0].AxisX.ScaleView.Zoom(0, l);
                        //CH1.ChartAreas[0].CursorX.IsUserEnabled = true;
                        //CH1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                        //CH1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                        //CH1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

                        //CH1.ChartAreas[0].AxisY.ScaleView.Zoom(0, a+0.5);
                        //CH1.ChartAreas[0].CursorY.IsUserEnabled = true;
                        //CH1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
                        //CH1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                        //CH1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;

                        this.tabControl1.SelectedTab = TP3;
                        break;
                    }
                case "Последовательный график":
                    {

                        this.tabControl1.SelectedTab = TP3;
                        break;
                    }
            }
            B5.Enabled = true;
            B6.Enabled = true;
        }

        private void B6_Click(object sender, EventArgs e)
        {
            w = p + 2;
            if (i==w)
            {
                B5.Enabled = true;
                B6.Enabled = false;
                B6.Text = "Старт";
                i = 0;
            }
            else
            { 
                B6.Text = "След. точка";
                {
                    CH1.Series[0].Points.AddXY(tau[i], Ca[i]);
                    CH1.Series[1].Points.AddXY(tau[i], Cb[i]);
                    CH1.Series[2].Points.AddXY(tau[i], Cd[i]);
                    L1.Text = i.ToString();
                }
                i++;

                //CH1.ChartAreas[0].AxisX.ScaleView.Zoom(0, l);
                //CH1.ChartAreas[0].CursorX.IsUserEnabled = true;
                //CH1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                //CH1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                //CH1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

                //CH1.ChartAreas[0].AxisY.ScaleView.Zoom(0, a+0.5);
                //CH1.ChartAreas[0].CursorY.IsUserEnabled = true;
                //CH1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
                //CH1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                //CH1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            }
        }
    }
}