using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HttpTest
{
    public partial class Base64ToolForm : Form
    {
        public Base64ToolForm()
        {
            InitializeComponent();

            openFileDialog1.Multiselect = true;
            saveFileDialog1.Filter = "所有文件（*.*）|*.*";
        }

        private void button1_Click( object sender, EventArgs e )
        {
            var r = openFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                string[] names = openFileDialog1.FileNames;
                StringBuilder sb = new StringBuilder();
                foreach (string name in names)
                {
                    sb.Append(name).Append(",");
                }
                if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
                textBox1.Text = sb.ToString();
            }
            else textBox1.Text = string.Empty;
        }

        private void radioButton1_CheckedChanged( object sender, EventArgs e )
        {
            richTextBox1.Enabled = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged( object sender, EventArgs e )
        {
            textBox1.Enabled = radioButton2.Checked;
            button1.Enabled = radioButton2.Checked;
        }

        private void button2_Click( object sender, EventArgs e )
        {
            try
            {
                richTextBox2.Text = string.Empty;
                if (radioButton1.Checked)
                {
                    string text = richTextBox1.Text;
                    byte[] source = Encoding.UTF8.GetBytes(text);
                    string result = Convert.ToBase64String(source);
                    Clipboard.SetText(result);
                    statusLabel.Text = "编码完成，结果已复制到剪贴板。";
                    richTextBox2.Text = result;
                }
                else if (radioButton2.Checked)
                {
                    string[] paths = textBox1.Text.Split(',');

                    StringBuilder sb = new StringBuilder();

                    foreach (string path in paths)
                    {
                        if (string.IsNullOrWhiteSpace(path))
                        {
                            richTextBox2.AppendText("路径不能为空。\r\n");
                            continue;
                        }
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            progressBar.Value = 0;
                            progressBar.Maximum = (int)(fs.Length / 240);
                            byte[] buffer = new byte[2400];
                            int offset = 0;
                            int count;
                            while ((count = fs.Read(buffer, offset, buffer.Length)) > 0)
                            {
                                string result = Convert.ToBase64String(buffer, 0, count);
                                richTextBox2.AppendText(result);
                                richTextBox2.ScrollToCaret();
                                sb.Append(result);
                                progressBar.PerformStep();
                            }
                            progressBar.Value = progressBar.Maximum;
                            richTextBox2.AppendText("\r\n" + path + "编码完成\r\n");
                            sb.Append(",");
                        }
                    }
                    if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
                    string r = sb.ToString();
                    if (!string.IsNullOrWhiteSpace(r))
                    {
                        Clipboard.SetText(r);
                        statusLabel.Text = "编码完成，结果已复制到剪贴板。";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click( object sender, EventArgs e )
        {

            var r = saveFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                textBox2.Text = saveFileDialog1.FileName;
            }
        }

        private void radioButton4_CheckedChanged( object sender, EventArgs e )
        {
            richTextBox4.Enabled = radioButton4.Checked;
        }

        private void radioButton3_CheckedChanged( object sender, EventArgs e )
        {
            textBox2.Enabled = radioButton3.Checked;
            button3.Enabled = radioButton3.Checked;
        }

        private void button4_Click( object sender, EventArgs e )
        {
            try
            {
                richTextBox4.Text = string.Empty;
                string source = richTextBox3.Text;
                if (string.IsNullOrWhiteSpace(source))
                {
                    statusLabel.Text = "解码内容为空";
                    return;
                }
                progressBar.Value = 0;
                byte[] sArr = Convert.FromBase64String(source);

                if (radioButton3.Checked)
                {
                    string path = textBox2.Text;
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        MessageBox.Show("路径不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(sArr, 0, sArr.Length);
                    }
                    var r = MessageBox.Show("解码完成，点击确定键打开文件", "解码完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (r == DialogResult.OK)
                    {
                        Process.Start(path);
                    }
                }
                else if (radioButton4.Checked)
                {
                    string result = Encoding.UTF8.GetString(sArr);
                    richTextBox4.Text = result;
                    Clipboard.SetText(result);
                    statusLabel.Text = "解码完成，内容已复制到剪贴板";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
