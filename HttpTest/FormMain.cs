using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HttpLib.HttpClient;

namespace HttpTest
{
    public partial class FormMain : Form
    {

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            string url = textBox1.Text.Trim();
            string data = textBox2.Text.Trim();
            string resp = string.Empty;
            try
            {
                if (radioButton_Get.Checked)
                {
                    if (!string.IsNullOrEmpty(data)) url += "?" + data;
                    resp = HttpLib.HttpClient.HttpMethods.Get(url);
                }
                else if (radioButton_Post.Checked)
                {
                    resp = HttpLib.HttpClient.HttpMethods.Post(url, data);
                }
                richTextBox1.Text = resp;

            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.ToString();
            }
        }

        private void button2_Click( object sender, EventArgs e )
        {
            List<FormItem> list = new List<FormItem>();

            foreach (DataGridViewRow row in dataGridView_KV.Rows)
            {
                string key = row.Cells["KEY"].Value as string;
                if (string.IsNullOrEmpty(key)) continue;
                string value = row.Cells["VALUE"].Value as string;
                var item = new FormItem() { Key = key, Value = value };
                list.Add(item);
            }

            foreach (DataGridViewRow row in dataGridView_FILE.Rows)
            {
                string key = row.Cells["KEY2"].Value as string;
                if (string.IsNullOrEmpty(key)) continue;
                string path = row.Cells["FILE"].Value as string;
                if (!File.Exists(path)) continue;

                string filename = path.Substring(path.LastIndexOf("\\") + 1);
                var item = new FormItem() { Key = key, FileName = filename, FileContent = File.OpenRead(path) };
                list.Add(item);
            }
            string url = textBox3.Text.Trim();
            try
            {
                richTextBox1.Text = HttpMethods.PostForm(url, list);
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.ToString();
            }
        }

        private void dataGridView_FILE_CellDoubleClick( object sender, DataGridViewCellEventArgs e )
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string key = openFileDialog1.SafeFileName;
                string file = openFileDialog1.FileName;
                int index = dataGridView_FILE.Rows.Count - 1;
                dataGridView_FILE.Rows.Add();
                var row = dataGridView_FILE.Rows[index];
                row.Cells["KEY2"].Value = key;
                row.Cells["FILE"].Value = file;
            }
        }
    }
}
