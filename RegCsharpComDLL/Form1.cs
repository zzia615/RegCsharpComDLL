using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RegCsharpComDLL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = ".net40";
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "全部文件（*.*）|*.*";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.FileOk += (ss, ee) =>
            {
                textBox2.Text = openFileDialog1.FileName;
            };
            openFileDialog1.ShowDialog();
        }
        void RunCmd(string file,string args)
        {


            string s_regasm = file;

            ProcessStartInfo startInfo = new ProcessStartInfo(s_regasm);
            startInfo.Arguments = args;
            startInfo.StandardOutputEncoding = Encoding.Default;
            startInfo.StandardErrorEncoding = Encoding.Default;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            Process process = Process.Start(startInfo);
            process.WaitForExit();
            if (textBox1.Text.Length > 0)
            {
                textBox1.Text += "\r\n";
                textBox1.Text += "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
                textBox1.Text += "\r\n";
            }
            textBox1.Text += process.StandardOutput.ReadToEnd();
            textBox1.Text += process.StandardError.ReadToEnd();

            textBox1.Focus();
            this.textBox1.Select(this.textBox1.TextLength, 0);//光标定位到文本最后
            this.textBox1.ScrollToCaret();//滚动到光标处
        }
        string GetPath()
        {
            string path = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\";
            if (comboBox1.Text == ".net40")
            {
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("未安装.net framework 4.0环境");
                    return "";
                }
            }
            else
            {
                path = @"C:\Windows\Microsoft.NET\Framework\v2.0.50727\";

                if (!Directory.Exists(path))
                {
                    MessageBox.Show("未安装.net framework 2.0环境");
                    return "";
                }
            }
            return path;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            string path = GetPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string s_regasm = Path.Combine(path, "regasm.exe");
            RunCmd(s_regasm, "/codebase " + textBox2.Text);
        }



        private void button2_Click(object sender, EventArgs e)
        {
            string path = GetPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string s_regasm = Path.Combine(path, "regasm.exe");
            RunCmd(s_regasm, "/codebase " + textBox2.Text+" /u");
        }
    }
}
