namespace spytrek2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.bOpen = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.bDisconnect = new System.Windows.Forms.Button();
      this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.bExit = new System.Windows.Forms.Button();
      this.Net = new System.Windows.Forms.TabControl();
      this.tNet = new System.Windows.Forms.TabPage();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.button1 = new System.Windows.Forms.Button();
      this.bTest2 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.label9 = new System.Windows.Forms.Label();
      this.lab_Name = new System.Windows.Forms.Label();
      this.lab_Imei = new System.Windows.Forms.Label();
      this.lab_Version = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.bClrListBox = new System.Windows.Forms.Button();
      this.tFiles = new System.Windows.Forms.TabPage();
      this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
      this.bTestTrackLoad = new System.Windows.Forms.Button();
      this.bGetFiles = new System.Windows.Forms.Button();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colUnuse1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.mileage = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.tUpdate = new System.Windows.Forms.TabPage();
      this.button3 = new System.Windows.Forms.Button();
      this.bTest1 = new System.Windows.Forms.Button();
      this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
      this.lStatus = new System.Windows.Forms.Label();
      this.lVersion = new System.Windows.Forms.Label();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
      this.Net.SuspendLayout();
      this.tNet.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.tFiles.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.tUpdate.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
      this.SuspendLayout();
      // 
      // bOpen
      // 
      this.bOpen.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bOpen.ForeColor = System.Drawing.SystemColors.InfoText;
      this.bOpen.Location = new System.Drawing.Point(109, 49);
      this.bOpen.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.bOpen.Name = "bOpen";
      this.bOpen.Size = new System.Drawing.Size(71, 30);
      this.bOpen.TabIndex = 1;
      this.bOpen.Text = "Open";
      this.bOpen.UseVisualStyleBackColor = true;
      this.bOpen.Click += new System.EventHandler(this.bConnect_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.bDisconnect);
      this.groupBox1.Controls.Add(this.numericUpDown1);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.bOpen);
      this.groupBox1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.groupBox1.ForeColor = System.Drawing.SystemColors.InfoText;
      this.groupBox1.Location = new System.Drawing.Point(16, 27);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.groupBox1.Size = new System.Drawing.Size(278, 98);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "NET";
      // 
      // bDisconnect
      // 
      this.bDisconnect.Enabled = false;
      this.bDisconnect.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bDisconnect.ForeColor = System.Drawing.SystemColors.InfoText;
      this.bDisconnect.Location = new System.Drawing.Point(192, 49);
      this.bDisconnect.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.bDisconnect.Name = "bDisconnect";
      this.bDisconnect.Size = new System.Drawing.Size(71, 30);
      this.bDisconnect.TabIndex = 3;
      this.bDisconnect.Text = "Close";
      this.bDisconnect.UseVisualStyleBackColor = true;
      this.bDisconnect.Click += new System.EventHandler(this.bDisconnect_click);
      // 
      // numericUpDown1
      // 
      this.numericUpDown1.Location = new System.Drawing.Point(12, 52);
      this.numericUpDown1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.numericUpDown1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.numericUpDown1.Name = "numericUpDown1";
      this.numericUpDown1.Size = new System.Drawing.Size(79, 22);
      this.numericUpDown1.TabIndex = 2;
      this.numericUpDown1.Value = new decimal(new int[] {
            20001,
            0,
            0,
            0});
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.ForeColor = System.Drawing.SystemColors.InfoText;
      this.label1.Location = new System.Drawing.Point(9, 28);
      this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(45, 16);
      this.label1.TabIndex = 1;
      this.label1.Text = "0.0.0.0";
      // 
      // listBox1
      // 
      this.listBox1.FormattingEnabled = true;
      this.listBox1.ItemHeight = 17;
      this.listBox1.Location = new System.Drawing.Point(4, 204);
      this.listBox1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.listBox1.Name = "listBox1";
      this.listBox1.ScrollAlwaysVisible = true;
      this.listBox1.Size = new System.Drawing.Size(628, 293);
      this.listBox1.TabIndex = 4;
      // 
      // bExit
      // 
      this.bExit.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bExit.ForeColor = System.Drawing.SystemColors.InfoText;
      this.bExit.Location = new System.Drawing.Point(577, 41);
      this.bExit.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.bExit.Name = "bExit";
      this.bExit.Size = new System.Drawing.Size(40, 30);
      this.bExit.TabIndex = 5;
      this.bExit.Text = "EXIT";
      this.bExit.UseVisualStyleBackColor = true;
      this.bExit.Click += new System.EventHandler(this.bExit_Click);
      // 
      // Net
      // 
      this.Net.Controls.Add(this.tNet);
      this.Net.Controls.Add(this.tFiles);
      this.Net.Controls.Add(this.tUpdate);
      this.Net.Location = new System.Drawing.Point(6, 15);
      this.Net.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.Net.Name = "Net";
      this.Net.SelectedIndex = 0;
      this.Net.Size = new System.Drawing.Size(645, 532);
      this.Net.TabIndex = 1;
      this.Net.Tag = "";
      // 
      // tNet
      // 
      this.tNet.BackColor = System.Drawing.SystemColors.Menu;
      this.tNet.Controls.Add(this.groupBox2);
      this.tNet.Controls.Add(this.bExit);
      this.tNet.Controls.Add(this.groupBox3);
      this.tNet.Controls.Add(this.bClrListBox);
      this.tNet.Controls.Add(this.groupBox1);
      this.tNet.Controls.Add(this.listBox1);
      this.tNet.Location = new System.Drawing.Point(4, 26);
      this.tNet.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.tNet.Name = "tNet";
      this.tNet.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.tNet.Size = new System.Drawing.Size(637, 502);
      this.tNet.TabIndex = 1;
      this.tNet.Text = "NET";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.button1);
      this.groupBox2.Controls.Add(this.bTest2);
      this.groupBox2.Controls.Add(this.button2);
      this.groupBox2.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.groupBox2.ForeColor = System.Drawing.SystemColors.InfoText;
      this.groupBox2.Location = new System.Drawing.Point(299, 27);
      this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox2.Size = new System.Drawing.Size(247, 98);
      this.groupBox2.TabIndex = 7;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Info";
      // 
      // button1
      // 
      this.button1.ForeColor = System.Drawing.SystemColors.InfoText;
      this.button1.Location = new System.Drawing.Point(167, 23);
      this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(64, 56);
      this.button1.TabIndex = 0;
      this.button1.Text = "Update file";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // bTest2
      // 
      this.bTest2.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bTest2.Location = new System.Drawing.Point(87, 49);
      this.bTest2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.bTest2.Name = "bTest2";
      this.bTest2.Size = new System.Drawing.Size(61, 30);
      this.bTest2.TabIndex = 2;
      this.bTest2.Text = "ECHO";
      this.bTest2.UseVisualStyleBackColor = true;
      this.bTest2.Click += new System.EventHandler(this.button3_Click);
      // 
      // button2
      // 
      this.button2.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.button2.Location = new System.Drawing.Point(8, 49);
      this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(61, 30);
      this.button2.TabIndex = 1;
      this.button2.Text = "INFO";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.label9);
      this.groupBox3.Controls.Add(this.lab_Name);
      this.groupBox3.Controls.Add(this.lab_Imei);
      this.groupBox3.Controls.Add(this.lab_Version);
      this.groupBox3.Controls.Add(this.label5);
      this.groupBox3.Controls.Add(this.label3);
      this.groupBox3.Controls.Add(this.label2);
      this.groupBox3.Controls.Add(this.label4);
      this.groupBox3.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.groupBox3.ForeColor = System.Drawing.SystemColors.InfoText;
      this.groupBox3.Location = new System.Drawing.Point(16, 132);
      this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBox3.Size = new System.Drawing.Size(468, 64);
      this.groupBox3.TabIndex = 3;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "UPDATE INFO";
      // 
      // label9
      // 
      this.label9.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label9.Location = new System.Drawing.Point(262, 40);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(140, 18);
      this.label9.TabIndex = 7;
      this.label9.Text = "N/D";
      this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lab_Name
      // 
      this.lab_Name.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lab_Name.Location = new System.Drawing.Point(262, 20);
      this.lab_Name.Name = "lab_Name";
      this.lab_Name.Size = new System.Drawing.Size(140, 18);
      this.lab_Name.TabIndex = 6;
      this.lab_Name.Text = "N/D";
      this.lab_Name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lab_Imei
      // 
      this.lab_Imei.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lab_Imei.Location = new System.Drawing.Point(63, 40);
      this.lab_Imei.Name = "lab_Imei";
      this.lab_Imei.Size = new System.Drawing.Size(140, 18);
      this.lab_Imei.TabIndex = 5;
      this.lab_Imei.Text = "N/D";
      this.lab_Imei.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lab_Version
      // 
      this.lab_Version.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lab_Version.Location = new System.Drawing.Point(63, 20);
      this.lab_Version.Name = "lab_Version";
      this.lab_Version.Size = new System.Drawing.Size(140, 18);
      this.lab_Version.TabIndex = 4;
      this.lab_Version.Text = "N/D";
      this.lab_Version.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label5
      // 
      this.label5.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label5.Location = new System.Drawing.Point(209, 40);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(50, 18);
      this.label5.TabIndex = 3;
      this.label5.Text = "NU:";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label3
      // 
      this.label3.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label3.Location = new System.Drawing.Point(209, 20);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(50, 18);
      this.label3.TabIndex = 2;
      this.label3.Text = "NAME:";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label2
      // 
      this.label2.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label2.Location = new System.Drawing.Point(10, 40);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(50, 18);
      this.label2.TabIndex = 1;
      this.label2.Text = "IMEI:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label4
      // 
      this.label4.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label4.Location = new System.Drawing.Point(10, 20);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(50, 18);
      this.label4.TabIndex = 0;
      this.label4.Text = "VER:";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // bClrListBox
      // 
      this.bClrListBox.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bClrListBox.ForeColor = System.Drawing.SystemColors.InfoText;
      this.bClrListBox.Location = new System.Drawing.Point(547, 166);
      this.bClrListBox.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.bClrListBox.Name = "bClrListBox";
      this.bClrListBox.Size = new System.Drawing.Size(70, 30);
      this.bClrListBox.TabIndex = 6;
      this.bClrListBox.Text = "Clr";
      this.bClrListBox.UseVisualStyleBackColor = true;
      this.bClrListBox.Click += new System.EventHandler(this.bClrLB_Click);
      // 
      // tFiles
      // 
      this.tFiles.BackColor = System.Drawing.SystemColors.Menu;
      this.tFiles.Controls.Add(this.numericUpDown2);
      this.tFiles.Controls.Add(this.bTestTrackLoad);
      this.tFiles.Controls.Add(this.bGetFiles);
      this.tFiles.Controls.Add(this.dataGridView1);
      this.tFiles.Location = new System.Drawing.Point(4, 26);
      this.tFiles.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.tFiles.Name = "tFiles";
      this.tFiles.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.tFiles.Size = new System.Drawing.Size(637, 502);
      this.tFiles.TabIndex = 2;
      this.tFiles.Text = "Tracks";
      // 
      // numericUpDown2
      // 
      this.numericUpDown2.Enabled = false;
      this.numericUpDown2.Location = new System.Drawing.Point(7, 465);
      this.numericUpDown2.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.numericUpDown2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDown2.Name = "numericUpDown2";
      this.numericUpDown2.Size = new System.Drawing.Size(52, 22);
      this.numericUpDown2.TabIndex = 7;
      this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // bTestTrackLoad
      // 
      this.bTestTrackLoad.ForeColor = System.Drawing.SystemColors.InfoText;
      this.bTestTrackLoad.Location = new System.Drawing.Point(67, 465);
      this.bTestTrackLoad.Margin = new System.Windows.Forms.Padding(0);
      this.bTestTrackLoad.Name = "bTestTrackLoad";
      this.bTestTrackLoad.Size = new System.Drawing.Size(65, 22);
      this.bTestTrackLoad.TabIndex = 2;
      this.bTestTrackLoad.Text = "TEST 5";
      this.bTestTrackLoad.UseVisualStyleBackColor = true;
      this.bTestTrackLoad.Click += new System.EventHandler(this.bTestTrackLoad_Click);
      // 
      // bGetFiles
      // 
      this.bGetFiles.ForeColor = System.Drawing.SystemColors.InfoText;
      this.bGetFiles.Location = new System.Drawing.Point(7, 24);
      this.bGetFiles.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.bGetFiles.Name = "bGetFiles";
      this.bGetFiles.Size = new System.Drawing.Size(125, 30);
      this.bGetFiles.TabIndex = 1;
      this.bGetFiles.Text = "Get Tracks";
      this.bGetFiles.UseVisualStyleBackColor = true;
      this.bGetFiles.Click += new System.EventHandler(this.bGetFiles_Click);
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.AllowUserToResizeColumns = false;
      this.dataGridView1.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
      this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
      this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedHorizontal;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ActiveBorder;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colName,
            this.colSize,
            this.colUnuse1,
            this.mileage});
      this.dataGridView1.EnableHeadersVisualStyles = false;
      this.dataGridView1.GridColor = System.Drawing.SystemColors.Menu;
      this.dataGridView1.Location = new System.Drawing.Point(7, 72);
      this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
      this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle8;
      this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridView1.Size = new System.Drawing.Size(618, 385);
      this.dataGridView1.TabIndex = 0;
      // 
      // colID
      // 
      this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.InfoText;
      this.colID.DefaultCellStyle = dataGridViewCellStyle3;
      this.colID.FillWeight = 50F;
      this.colID.HeaderText = "ID";
      this.colID.Name = "colID";
      this.colID.ReadOnly = true;
      this.colID.Width = 50;
      // 
      // colName
      // 
      this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.InfoText;
      this.colName.DefaultCellStyle = dataGridViewCellStyle4;
      this.colName.HeaderText = "Name";
      this.colName.Name = "colName";
      this.colName.ReadOnly = true;
      // 
      // colSize
      // 
      this.colSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
      dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.InfoText;
      this.colSize.DefaultCellStyle = dataGridViewCellStyle5;
      this.colSize.HeaderText = "Size (Bytes)";
      this.colSize.Name = "colSize";
      this.colSize.ReadOnly = true;
      // 
      // colUnuse1
      // 
      this.colUnuse1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
      dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.InfoText;
      this.colUnuse1.DefaultCellStyle = dataGridViewCellStyle6;
      this.colUnuse1.HeaderText = "Dist KM";
      this.colUnuse1.Name = "colUnuse1";
      this.colUnuse1.ReadOnly = true;
      this.colUnuse1.Width = 76;
      // 
      // mileage
      // 
      this.mileage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
      dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.InfoText;
      dataGridViewCellStyle7.NullValue = null;
      this.mileage.DefaultCellStyle = dataGridViewCellStyle7;
      this.mileage.HeaderText = "MileAge (KM)";
      this.mileage.Name = "mileage";
      this.mileage.ReadOnly = true;
      this.mileage.Width = 113;
      // 
      // tUpdate
      // 
      this.tUpdate.Controls.Add(this.button3);
      this.tUpdate.Controls.Add(this.bTest1);
      this.tUpdate.Location = new System.Drawing.Point(4, 26);
      this.tUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.tUpdate.Name = "tUpdate";
      this.tUpdate.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.tUpdate.Size = new System.Drawing.Size(637, 502);
      this.tUpdate.TabIndex = 3;
      this.tUpdate.Text = "Update";
      this.tUpdate.UseVisualStyleBackColor = true;
      // 
      // button3
      // 
      this.button3.Enabled = false;
      this.button3.ForeColor = System.Drawing.SystemColors.WindowText;
      this.button3.Location = new System.Drawing.Point(17, 180);
      this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(75, 28);
      this.button3.TabIndex = 3;
      this.button3.Text = "button3";
      this.button3.UseVisualStyleBackColor = true;
      // 
      // bTest1
      // 
      this.bTest1.Enabled = false;
      this.bTest1.ForeColor = System.Drawing.SystemColors.WindowText;
      this.bTest1.Location = new System.Drawing.Point(17, 216);
      this.bTest1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.bTest1.Name = "bTest1";
      this.bTest1.Size = new System.Drawing.Size(75, 28);
      this.bTest1.TabIndex = 0;
      this.bTest1.Text = "Test";
      this.bTest1.UseVisualStyleBackColor = true;
      // 
      // lStatus
      // 
      this.lStatus.AutoSize = true;
      this.lStatus.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lStatus.ForeColor = System.Drawing.SystemColors.InfoText;
      this.lStatus.Location = new System.Drawing.Point(11, 551);
      this.lStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lStatus.Name = "lStatus";
      this.lStatus.Size = new System.Drawing.Size(37, 17);
      this.lStatus.TabIndex = 7;
      this.lStatus.Text = "Start";
      // 
      // lVersion
      // 
      this.lVersion.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.lVersion.ForeColor = System.Drawing.SystemColors.InfoText;
      this.lVersion.Location = new System.Drawing.Point(607, 549);
      this.lVersion.Name = "lVersion";
      this.lVersion.Size = new System.Drawing.Size(40, 21);
      this.lVersion.TabIndex = 0;
      this.lVersion.Text = "2.1.1";
      this.lVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.InactiveBorder;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.ClientSize = new System.Drawing.Size(658, 572);
      this.Controls.Add(this.Net);
      this.Controls.Add(this.lVersion);
      this.Controls.Add(this.lStatus);
      this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "ST2";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
      this.Net.ResumeLayout(false);
      this.tNet.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.tFiles.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.tUpdate.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOpen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bDisconnect;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.TabControl Net;
        private System.Windows.Forms.TabPage tNet;
        private System.Windows.Forms.TabPage tFiles;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button bGetFiles;
        private System.Windows.Forms.Button bClrListBox;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button bTestTrackLoad;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.Button bTest1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnuse1;
        private System.Windows.Forms.DataGridViewTextBoxColumn mileage;
        private System.Windows.Forms.TabPage tUpdate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button bTest2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lab_Name;
        private System.Windows.Forms.Label lab_Imei;
        private System.Windows.Forms.Label lab_Version;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
    }
}

