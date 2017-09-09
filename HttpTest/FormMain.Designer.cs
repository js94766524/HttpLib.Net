namespace HttpTest
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton_Post = new System.Windows.Forms.RadioButton();
            this.radioButton_Get = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.dataGridView_KV = new System.Windows.Forms.DataGridView();
            this.KEY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VALUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_FILE = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.KEY2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FILE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_KV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_FILE)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(778, 448);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.radioButton_Post);
            this.tabPage1.Controls.Add(this.radioButton_Get);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(770, 418);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "GET / POST";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox2);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.dataGridView_FILE);
            this.tabPage2.Controls.Add(this.dataGridView_KV);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(770, 418);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "FORM";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "RESP：";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.Location = new System.Drawing.Point(64, 100);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(698, 310);
            this.richTextBox1.TabIndex = 17;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(696, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 25);
            this.button1.TabIndex = 16;
            this.button1.Text = "SEND";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton_Post
            // 
            this.radioButton_Post.AutoSize = true;
            this.radioButton_Post.Location = new System.Drawing.Point(633, 68);
            this.radioButton_Post.Name = "radioButton_Post";
            this.radioButton_Post.Size = new System.Drawing.Size(57, 21);
            this.radioButton_Post.TabIndex = 15;
            this.radioButton_Post.Text = "POST";
            this.radioButton_Post.UseVisualStyleBackColor = true;
            // 
            // radioButton_Get
            // 
            this.radioButton_Get.AutoSize = true;
            this.radioButton_Get.Checked = true;
            this.radioButton_Get.Location = new System.Drawing.Point(581, 68);
            this.radioButton_Get.Name = "radioButton_Get";
            this.radioButton_Get.Size = new System.Drawing.Size(49, 21);
            this.radioButton_Get.TabIndex = 14;
            this.radioButton_Get.TabStop = true;
            this.radioButton_Get.Text = "GET";
            this.radioButton_Get.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "DATA：";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(64, 37);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(698, 23);
            this.textBox2.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "URL：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(64, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(698, 23);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "http://192.168.55.217:12580/";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "URL：";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(64, 10);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(617, 23);
            this.textBox3.TabIndex = 12;
            this.textBox3.Text = "http://192.168.55.217:12580/";
            // 
            // dataGridView_KV
            // 
            this.dataGridView_KV.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_KV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_KV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KEY,
            this.VALUE});
            this.dataGridView_KV.Location = new System.Drawing.Point(11, 39);
            this.dataGridView_KV.Name = "dataGridView_KV";
            this.dataGridView_KV.RowTemplate.Height = 23;
            this.dataGridView_KV.Size = new System.Drawing.Size(243, 111);
            this.dataGridView_KV.TabIndex = 14;
            // 
            // KEY
            // 
            this.KEY.HeaderText = "KEY";
            this.KEY.Name = "KEY";
            // 
            // VALUE
            // 
            this.VALUE.HeaderText = "VALUE";
            this.VALUE.Name = "VALUE";
            // 
            // dataGridView_FILE
            // 
            this.dataGridView_FILE.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_FILE.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView_FILE.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_FILE.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_FILE.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KEY2,
            this.FILE});
            this.dataGridView_FILE.Location = new System.Drawing.Point(260, 39);
            this.dataGridView_FILE.Name = "dataGridView_FILE";
            this.dataGridView_FILE.RowTemplate.Height = 23;
            this.dataGridView_FILE.Size = new System.Drawing.Size(502, 111);
            this.dataGridView_FILE.TabIndex = 15;
            this.dataGridView_FILE.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_FILE_CellDoubleClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(687, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "SEND";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBox2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox2.Location = new System.Drawing.Point(11, 156);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(751, 254);
            this.richTextBox2.TabIndex = 17;
            this.richTextBox2.Text = "";
            // 
            // KEY2
            // 
            this.KEY2.HeaderText = "KEY";
            this.KEY2.Name = "KEY2";
            this.KEY2.Width = 55;
            // 
            // FILE
            // 
            this.FILE.HeaderText = "FILE";
            this.FILE.Name = "FILE";
            this.FILE.Width = 56;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 448);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "HTTP TEST";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_KV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_FILE)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButton_Post;
        private System.Windows.Forms.RadioButton radioButton_Get;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView_FILE;
        private System.Windows.Forms.DataGridView dataGridView_KV;
        private System.Windows.Forms.DataGridViewTextBoxColumn KEY;
        private System.Windows.Forms.DataGridViewTextBoxColumn VALUE;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridViewTextBoxColumn KEY2;
        private System.Windows.Forms.DataGridViewTextBoxColumn FILE;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

