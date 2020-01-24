using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Podchet
{
    public partial class Form1 : Form
    {
        string sav = "";
        public Form1()
        {
            InitializeComponent();
            button3.Visible = false;
            ToolTip t = new ToolTip();
            t.SetToolTip(checkBox1, "Ведется подсчет только букв и цифр. Исключая такие символы как: .,?!'@' \n \r");
        }
        public class table
        {
            public table(char ch,int i,double d)
            {
                simvol = ch;
                count = i;
                veroitnost = d;
            }
            public char simvol {get;set;}
            public int count {get;set;}
            public double veroitnost { get; set; }
        }
        public int sim_col(string s,char h)
        {
            int count = 0;
            for (int i = 0; i < s.Count();i++ )
            {
                if(s[i]==h)
                {
                    count++;
                }
            }
                return count;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<table> tb = new List<table> { };
            var text = textBox1.Text;
            if (checkBox1.Checked == true)
            {
                List<char> aa = new List<char>();
                foreach (var i in text)
                {
                    aa.Add(i);
                }

                text = "";
                aa.Remove('\r');
                foreach (var i in aa)
                {
                    if (',' == i || '.' == i || '"' == i || '?' == i || '!' == i || '@' == i || '\n' == i)
                    {
                        text += ' ';
                    }
                    else { text += i; }

                }
            }
            // text = aa.ToString();
            textBox1.Text = text;
            label1.Text = "Всего символов: " + text.Count();

            var distinct_text = text.Distinct();
            label2.Text = "Без повтора:" + distinct_text.Count();
            foreach (var a in distinct_text)
            {
                tb.Add(new table(a, sim_col(text, a), (double)Math.Round((double)sim_col(text, a)/ text.Count(), 3)));
            }

            textBox1.Text += "\r\n";

            textBox1.Text += "Символ" + "\t\n" + "Количество" + "\t" + "Вероятность" + "\r\n";
            sav += "\"Символ\";"+ "\"Количество\";" + "\"Вероятность\";" + "\n";
            foreach (var i in tb)
            {

                textBox1.Text += i.simvol + "\t\n    " + i.count + "\t                        " + Math.Round((double)i.count / text.Count(), 3) + "\r\n";
                sav += "\"" + i.simvol + "\";\"" + i.count + "\";\"" + Math.Round((double)i.count / text.Count(), 3) + "\";" + "\n";
            }
            sav += "\n\"Всего символов\";" + "\"" + text.Count() + "\";";
            sav += "\n\"Без повтора\";" + "\"" + distinct_text.Count() + "\";";
            button3.Visible = true;
            if (checkBox2.Checked == true)
            {
               // dataGridView1.AutoGenerateColumns = false;                
                dataGridView1.DataSource =tb;                
                dataGridView1.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            sav = "";
            dataGridView1.Visible = false;
            dataGridView1.DataSource = null;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.Rows.Count - 1]);
            textBox1.Clear();
        }       
        private void button3_Click(object sender, EventArgs e)
        {          
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый документ (*.csv)|*.csv|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, true, Encoding.UTF8);
                streamWriter.WriteLine(sav);
                streamWriter.Close();
            }
           
        }
    }
}
