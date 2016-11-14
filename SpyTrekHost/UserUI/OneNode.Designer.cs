namespace SpyTrekHost.UserUI
{
    partial class OneNodeForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnGetTreks = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblImei = new System.Windows.Forms.Label();
            this.lblVer = new System.Windows.Forms.Label();
            this.fileIdNum = new System.Windows.Forms.NumericUpDown();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAvrSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mileage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileIdNum)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.name,
            this.cDuration,
            this.cAvrSpeed,
            this.size,
            this.dist,
            this.mileage});
            this.dataGridView1.Location = new System.Drawing.Point(12, 101);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 4;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(626, 361);
            this.dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(79, 473);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Get Trek";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(370, 23);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(75, 23);
            this.btnInfo.TabIndex = 2;
            this.btnInfo.Text = "INFO";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnGetTreks
            // 
            this.btnGetTreks.Location = new System.Drawing.Point(472, 23);
            this.btnGetTreks.Name = "btnGetTreks";
            this.btnGetTreks.Size = new System.Drawing.Size(75, 23);
            this.btnGetTreks.TabIndex = 3;
            this.btnGetTreks.Text = "Get Tracks";
            this.btnGetTreks.UseVisualStyleBackColor = true;
            this.btnGetTreks.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(533, 468);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Clear Data";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(176, 477);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Idle";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "VER:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "IMEI:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "NAME:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.lblImei);
            this.groupBox1.Controls.Add(this.lblVer);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 83);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "UPDATE INFO";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(225, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 14;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(64, 61);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(31, 13);
            this.lblName.TabIndex = 13;
            this.lblName.Text = "N/D";
            // 
            // lblImei
            // 
            this.lblImei.AutoSize = true;
            this.lblImei.Location = new System.Drawing.Point(64, 41);
            this.lblImei.Name = "lblImei";
            this.lblImei.Size = new System.Drawing.Size(31, 13);
            this.lblImei.TabIndex = 12;
            this.lblImei.Text = "N/D";
            // 
            // lblVer
            // 
            this.lblVer.AutoSize = true;
            this.lblVer.Location = new System.Drawing.Point(64, 21);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(31, 13);
            this.lblVer.TabIndex = 11;
            this.lblVer.Text = "N/D";
            // 
            // fileIdNum
            // 
            this.fileIdNum.Location = new System.Drawing.Point(12, 475);
            this.fileIdNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.fileIdNum.Name = "fileIdNum";
            this.fileIdNum.Size = new System.Drawing.Size(58, 20);
            this.fileIdNum.TabIndex = 12;
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.id.Width = 35;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.FillWeight = 80F;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            // 
            // cDuration
            // 
            this.cDuration.HeaderText = "Duration";
            this.cDuration.Name = "cDuration";
            this.cDuration.ReadOnly = true;
            this.cDuration.Width = 70;
            // 
            // cAvrSpeed
            // 
            this.cAvrSpeed.HeaderText = "Avr Spd";
            this.cAvrSpeed.Name = "cAvrSpeed";
            this.cAvrSpeed.ReadOnly = true;
            this.cAvrSpeed.Width = 70;
            // 
            // size
            // 
            this.size.HeaderText = "Size (bytes)";
            this.size.Name = "size";
            this.size.Width = 80;
            // 
            // dist
            // 
            this.dist.HeaderText = "Dist (km)";
            this.dist.Name = "dist";
            this.dist.Width = 80;
            // 
            // mileage
            // 
            this.mileage.HeaderText = "Mileage (km)";
            this.mileage.Name = "mileage";
            this.mileage.Width = 80;
            // 
            // OneNodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 503);
            this.Controls.Add(this.fileIdNum);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btnGetTreks);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(666, 542);
            this.MinimumSize = new System.Drawing.Size(666, 542);
            this.Name = "OneNodeForm";
            this.Text = "OneNode";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileIdNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Button btnGetTreks;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblImei;
        private System.Windows.Forms.Label lblVer;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.NumericUpDown fileIdNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAvrSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
        private System.Windows.Forms.DataGridViewTextBoxColumn dist;
        private System.Windows.Forms.DataGridViewTextBoxColumn mileage;
    }
}