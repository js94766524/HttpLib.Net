using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            string resp = string .Empty;
            try
            {
                if (radioButton_Get.Checked)
                {
                    if (!string.IsNullOrEmpty(data)) url += "?" + data;
                    resp = HttpLib.Client.HttpMethods.Get(url);
                }
                else if (radioButton_Post.Checked)
                {
                    resp = HttpLib.Client.HttpMethods.Post(url, data);
                }
                richTextBox1.Text = resp;

            }catch(Exception ex)
            {
                richTextBox1.Text = ex.ToString();
            }
        }
    }
}
