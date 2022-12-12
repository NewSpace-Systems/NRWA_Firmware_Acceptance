namespace NRWA_Communication_Acceptance
{
    partial class NRWA_FirmVer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NRWA_FirmVer));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbEdgeCases = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.cbAppTelData = new System.Windows.Forms.CheckBox();
            this.cbInitial = new System.Windows.Forms.CheckBox();
            this.cbAll = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cbPokeIncorrectData = new System.Windows.Forms.CheckBox();
            this.cbAppTellDataCast = new System.Windows.Forms.CheckBox();
            this.cbFalseCRC = new System.Windows.Forms.CheckBox();
            this.cbPokeOutsideData = new System.Windows.Forms.CheckBox();
            this.cbPokeOutsideAd = new System.Windows.Forms.CheckBox();
            this.cbPeekOutsideAd = new System.Windows.Forms.CheckBox();
            this.cbSystemTelemetryDataCast = new System.Windows.Forms.CheckBox();
            this.cbPeekPoke = new System.Windows.Forms.CheckBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbCrc = new System.Windows.Forms.CheckBox();
            this.btnTXsend = new System.Windows.Forms.Button();
            this.dgvTX = new System.Windows.Forms.DataGridView();
            this.FEND_Start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Destination_Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source_Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Command = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CRC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FEND_End = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rtbUser = new System.Windows.Forms.RichTextBox();
            this.cbPorts = new System.Windows.Forms.ComboBox();
            this.btnPortCon = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.lblLog = new System.Windows.Forms.Label();
            this.cbNRWA = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTX)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 68);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1711, 751);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbEdgeCases);
            this.tabPage1.Controls.Add(this.progressBar);
            this.tabPage1.Controls.Add(this.cbAppTelData);
            this.tabPage1.Controls.Add(this.cbInitial);
            this.tabPage1.Controls.Add(this.cbAll);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.cbPokeIncorrectData);
            this.tabPage1.Controls.Add(this.cbAppTellDataCast);
            this.tabPage1.Controls.Add(this.cbFalseCRC);
            this.tabPage1.Controls.Add(this.cbPokeOutsideData);
            this.tabPage1.Controls.Add(this.cbPokeOutsideAd);
            this.tabPage1.Controls.Add(this.cbPeekOutsideAd);
            this.tabPage1.Controls.Add(this.cbSystemTelemetryDataCast);
            this.tabPage1.Controls.Add(this.cbPeekPoke);
            this.tabPage1.Controls.Add(this.rtbLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(1703, 722);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Automatic";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbEdgeCases
            // 
            this.cbEdgeCases.AutoSize = true;
            this.cbEdgeCases.Location = new System.Drawing.Point(25, 381);
            this.cbEdgeCases.Name = "cbEdgeCases";
            this.cbEdgeCases.Size = new System.Drawing.Size(104, 20);
            this.cbEdgeCases.TabIndex = 14;
            this.cbEdgeCases.Text = "Edge Cases";
            this.cbEdgeCases.UseVisualStyleBackColor = true;
            this.cbEdgeCases.CheckedChanged += new System.EventHandler(this.cbEdgeCases_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(860, 676);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(815, 26);
            this.progressBar.TabIndex = 13;
            // 
            // cbAppTelData
            // 
            this.cbAppTelData.AutoSize = true;
            this.cbAppTelData.Location = new System.Drawing.Point(25, 325);
            this.cbAppTelData.Name = "cbAppTelData";
            this.cbAppTelData.Size = new System.Drawing.Size(255, 20);
            this.cbAppTelData.TabIndex = 12;
            this.cbAppTelData.Text = "Application Telemetry Data Validation";
            this.cbAppTelData.UseVisualStyleBackColor = true;
            this.cbAppTelData.CheckedChanged += new System.EventHandler(this.cbAppTelData_CheckedChanged);
            // 
            // cbInitial
            // 
            this.cbInitial.AutoSize = true;
            this.cbInitial.Location = new System.Drawing.Point(26, 98);
            this.cbInitial.Name = "cbInitial";
            this.cbInitial.Size = new System.Drawing.Size(104, 20);
            this.cbInitial.TabIndex = 11;
            this.cbInitial.Text = "Initial Values";
            this.cbInitial.UseVisualStyleBackColor = true;
            this.cbInitial.CheckedChanged += new System.EventHandler(this.cbInitial_CheckedChanged);
            // 
            // cbAll
            // 
            this.cbAll.AutoSize = true;
            this.cbAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAll.Location = new System.Drawing.Point(25, 69);
            this.cbAll.Margin = new System.Windows.Forms.Padding(4);
            this.cbAll.Name = "cbAll";
            this.cbAll.Size = new System.Drawing.Size(95, 20);
            this.cbAll.TabIndex = 10;
            this.cbAll.Text = "Select All";
            this.cbAll.UseVisualStyleBackColor = true;
            this.cbAll.CheckedChanged += new System.EventHandler(this.cbAll_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(31, 20);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 33);
            this.button1.TabIndex = 9;
            this.button1.Text = "RUN";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // cbPokeIncorrectData
            // 
            this.cbPokeIncorrectData.AutoSize = true;
            this.cbPokeIncorrectData.Location = new System.Drawing.Point(25, 353);
            this.cbPokeIncorrectData.Margin = new System.Windows.Forms.Padding(4);
            this.cbPokeIncorrectData.Name = "cbPokeIncorrectData";
            this.cbPokeIncorrectData.Size = new System.Drawing.Size(168, 20);
            this.cbPokeIncorrectData.TabIndex = 8;
            this.cbPokeIncorrectData.Text = "Poke With Corrupt Data";
            this.cbPokeIncorrectData.UseVisualStyleBackColor = true;
            // 
            // cbAppTellDataCast
            // 
            this.cbAppTellDataCast.AutoSize = true;
            this.cbAppTellDataCast.Location = new System.Drawing.Point(25, 296);
            this.cbAppTellDataCast.Margin = new System.Windows.Forms.Padding(4);
            this.cbAppTellDataCast.Name = "cbAppTellDataCast";
            this.cbAppTellDataCast.Size = new System.Drawing.Size(240, 20);
            this.cbAppTellDataCast.TabIndex = 7;
            this.cbAppTellDataCast.Text = "Application Telemetry Data Casting";
            this.cbAppTellDataCast.UseVisualStyleBackColor = true;
            // 
            // cbFalseCRC
            // 
            this.cbFalseCRC.AutoSize = true;
            this.cbFalseCRC.Location = new System.Drawing.Point(25, 267);
            this.cbFalseCRC.Margin = new System.Windows.Forms.Padding(4);
            this.cbFalseCRC.Name = "cbFalseCRC";
            this.cbFalseCRC.Size = new System.Drawing.Size(160, 20);
            this.cbFalseCRC.TabIndex = 6;
            this.cbFalseCRC.Text = "False CRC Response";
            this.cbFalseCRC.UseVisualStyleBackColor = true;
            // 
            // cbPokeOutsideData
            // 
            this.cbPokeOutsideData.AutoSize = true;
            this.cbPokeOutsideData.Location = new System.Drawing.Point(25, 239);
            this.cbPokeOutsideData.Margin = new System.Windows.Forms.Padding(4);
            this.cbPokeOutsideData.Name = "cbPokeOutsideData";
            this.cbPokeOutsideData.Size = new System.Drawing.Size(186, 20);
            this.cbPokeOutsideData.TabIndex = 5;
            this.cbPokeOutsideData.Text = "Poke Outside Data Range";
            this.cbPokeOutsideData.UseVisualStyleBackColor = true;
            // 
            // cbPokeOutsideAd
            // 
            this.cbPokeOutsideAd.AutoSize = true;
            this.cbPokeOutsideAd.Location = new System.Drawing.Point(25, 211);
            this.cbPokeOutsideAd.Margin = new System.Windows.Forms.Padding(4);
            this.cbPokeOutsideAd.Name = "cbPokeOutsideAd";
            this.cbPokeOutsideAd.Size = new System.Drawing.Size(208, 20);
            this.cbPokeOutsideAd.TabIndex = 4;
            this.cbPokeOutsideAd.Text = "Poke Outside Address Range";
            this.cbPokeOutsideAd.UseVisualStyleBackColor = true;
            // 
            // cbPeekOutsideAd
            // 
            this.cbPeekOutsideAd.AutoSize = true;
            this.cbPeekOutsideAd.Location = new System.Drawing.Point(25, 182);
            this.cbPeekOutsideAd.Margin = new System.Windows.Forms.Padding(4);
            this.cbPeekOutsideAd.Name = "cbPeekOutsideAd";
            this.cbPeekOutsideAd.Size = new System.Drawing.Size(208, 20);
            this.cbPeekOutsideAd.TabIndex = 3;
            this.cbPeekOutsideAd.Text = "Peek Outside Address Range";
            this.cbPeekOutsideAd.UseVisualStyleBackColor = true;
            // 
            // cbSystemTelemetryDataCast
            // 
            this.cbSystemTelemetryDataCast.AutoSize = true;
            this.cbSystemTelemetryDataCast.Location = new System.Drawing.Point(25, 154);
            this.cbSystemTelemetryDataCast.Margin = new System.Windows.Forms.Padding(4);
            this.cbSystemTelemetryDataCast.Name = "cbSystemTelemetryDataCast";
            this.cbSystemTelemetryDataCast.Size = new System.Drawing.Size(218, 20);
            this.cbSystemTelemetryDataCast.TabIndex = 2;
            this.cbSystemTelemetryDataCast.Text = "System Telemetry Data Casting";
            this.cbSystemTelemetryDataCast.UseVisualStyleBackColor = true;
            this.cbSystemTelemetryDataCast.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // cbPeekPoke
            // 
            this.cbPeekPoke.AutoSize = true;
            this.cbPeekPoke.Location = new System.Drawing.Point(25, 126);
            this.cbPeekPoke.Margin = new System.Windows.Forms.Padding(4);
            this.cbPeekPoke.Name = "cbPeekPoke";
            this.cbPeekPoke.Size = new System.Drawing.Size(96, 20);
            this.cbPeekPoke.TabIndex = 1;
            this.cbPeekPoke.Text = "Peek Poke";
            this.cbPeekPoke.UseVisualStyleBackColor = true;
            this.cbPeekPoke.CheckedChanged += new System.EventHandler(this.cbPeekPoke_CheckedChanged);
            // 
            // rtbLog
            // 
            this.rtbLog.Location = new System.Drawing.Point(860, 42);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(815, 629);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbCrc);
            this.tabPage2.Controls.Add(this.btnTXsend);
            this.tabPage2.Controls.Add(this.dgvTX);
            this.tabPage2.Controls.Add(this.rtbUser);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(1703, 722);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "User Defined";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbCrc
            // 
            this.cbCrc.AutoSize = true;
            this.cbCrc.Location = new System.Drawing.Point(21, 74);
            this.cbCrc.Margin = new System.Windows.Forms.Padding(4);
            this.cbCrc.Name = "cbCrc";
            this.cbCrc.Size = new System.Drawing.Size(57, 20);
            this.cbCrc.TabIndex = 7;
            this.cbCrc.Text = "CRC";
            this.cbCrc.UseVisualStyleBackColor = true;
            // 
            // btnTXsend
            // 
            this.btnTXsend.Location = new System.Drawing.Point(21, 26);
            this.btnTXsend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTXsend.Name = "btnTXsend";
            this.btnTXsend.Size = new System.Drawing.Size(148, 33);
            this.btnTXsend.TabIndex = 6;
            this.btnTXsend.Text = "SEND";
            this.btnTXsend.UseVisualStyleBackColor = true;
            this.btnTXsend.Click += new System.EventHandler(this.btnTXsend_Click);
            // 
            // dgvTX
            // 
            this.dgvTX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTX.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FEND_Start,
            this.Destination_Address,
            this.Source_Address,
            this.Command,
            this.Data,
            this.CRC,
            this.FEND_End});
            this.dgvTX.Location = new System.Drawing.Point(21, 114);
            this.dgvTX.Margin = new System.Windows.Forms.Padding(4);
            this.dgvTX.Name = "dgvTX";
            this.dgvTX.RowHeadersWidth = 5;
            this.dgvTX.Size = new System.Drawing.Size(821, 103);
            this.dgvTX.TabIndex = 2;
            this.dgvTX.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // FEND_Start
            // 
            this.FEND_Start.HeaderText = "FEND";
            this.FEND_Start.MinimumWidth = 6;
            this.FEND_Start.Name = "FEND_Start";
            this.FEND_Start.Width = 125;
            // 
            // Destination_Address
            // 
            this.Destination_Address.HeaderText = "Destination Address";
            this.Destination_Address.MinimumWidth = 6;
            this.Destination_Address.Name = "Destination_Address";
            this.Destination_Address.Width = 125;
            // 
            // Source_Address
            // 
            this.Source_Address.HeaderText = "Source Address";
            this.Source_Address.MinimumWidth = 6;
            this.Source_Address.Name = "Source_Address";
            this.Source_Address.Width = 125;
            // 
            // Command
            // 
            this.Command.HeaderText = "Command";
            this.Command.MinimumWidth = 6;
            this.Command.Name = "Command";
            this.Command.Width = 125;
            // 
            // Data
            // 
            this.Data.HeaderText = "Data";
            this.Data.MinimumWidth = 6;
            this.Data.Name = "Data";
            this.Data.Width = 125;
            // 
            // CRC
            // 
            this.CRC.HeaderText = "CRC";
            this.CRC.MinimumWidth = 6;
            this.CRC.Name = "CRC";
            this.CRC.Width = 125;
            // 
            // FEND_End
            // 
            this.FEND_End.HeaderText = "FEND";
            this.FEND_End.MinimumWidth = 6;
            this.FEND_End.Name = "FEND_End";
            this.FEND_End.Width = 125;
            // 
            // rtbUser
            // 
            this.rtbUser.Location = new System.Drawing.Point(856, 31);
            this.rtbUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbUser.Name = "rtbUser";
            this.rtbUser.Size = new System.Drawing.Size(815, 658);
            this.rtbUser.TabIndex = 1;
            this.rtbUser.Text = "";
            // 
            // cbPorts
            // 
            this.cbPorts.FormattingEnabled = true;
            this.cbPorts.Location = new System.Drawing.Point(133, 28);
            this.cbPorts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbPorts.Name = "cbPorts";
            this.cbPorts.Size = new System.Drawing.Size(116, 24);
            this.cbPorts.TabIndex = 1;
            this.cbPorts.SelectedIndexChanged += new System.EventHandler(this.cbPorts_SelectedIndexChanged);
            // 
            // btnPortCon
            // 
            this.btnPortCon.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnPortCon.FlatAppearance.BorderSize = 4;
            this.btnPortCon.Location = new System.Drawing.Point(273, 23);
            this.btnPortCon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPortCon.Name = "btnPortCon";
            this.btnPortCon.Size = new System.Drawing.Size(148, 33);
            this.btnPortCon.TabIndex = 3;
            this.btnPortCon.Text = "Connect";
            this.btnPortCon.UseVisualStyleBackColor = true;
            this.btnPortCon.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLog
            // 
            this.btnLog.Location = new System.Drawing.Point(1571, 23);
            this.btnLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(148, 33);
            this.btnLog.TabIndex = 4;
            this.btnLog.Text = "Log";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(1300, 31);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(0, 16);
            this.lblLog.TabIndex = 5;
            // 
            // cbNRWA
            // 
            this.cbNRWA.FormattingEnabled = true;
            this.cbNRWA.Items.AddRange(new object[] {
            "DRV110",
            "T32",
            "T6"});
            this.cbNRWA.Location = new System.Drawing.Point(626, 28);
            this.cbNRWA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbNRWA.Name = "cbNRWA";
            this.cbNRWA.Size = new System.Drawing.Size(116, 24);
            this.cbNRWA.TabIndex = 6;
            this.cbNRWA.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(473, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Select NRWA Varient";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Select Port";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(91, 23);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(38, 37);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // NRWA_FirmVer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1388, 664);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbNRWA);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.btnPortCon);
            this.Controls.Add(this.cbPorts);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "NRWA_FirmVer";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.NRWA_FirmVer_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cbPorts;
        private System.Windows.Forms.Button btnPortCon;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Button btnTXsend;
        private System.Windows.Forms.CheckBox cbAppTellDataCast;
        private System.Windows.Forms.CheckBox cbFalseCRC;
        private System.Windows.Forms.CheckBox cbPokeOutsideData;
        private System.Windows.Forms.CheckBox cbPokeOutsideAd;
        private System.Windows.Forms.CheckBox cbPeekOutsideAd;
        private System.Windows.Forms.CheckBox cbSystemTelemetryDataCast;
        private System.Windows.Forms.CheckBox cbPeekPoke;
        private System.Windows.Forms.CheckBox cbPokeIncorrectData;
        private System.Windows.Forms.DataGridView dgvTX;
        private System.Windows.Forms.RichTextBox rtbUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn FEND_Start;
        private System.Windows.Forms.DataGridViewTextBoxColumn Destination_Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source_Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Command;
        private System.Windows.Forms.DataGridViewTextBoxColumn Data;
        private System.Windows.Forms.DataGridViewTextBoxColumn CRC;
        private System.Windows.Forms.DataGridViewTextBoxColumn FEND_end;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn FEND_End;
        private System.Windows.Forms.CheckBox cbCrc;
        private System.Windows.Forms.CheckBox cbAll;
        private System.Windows.Forms.CheckBox cbInitial;
        private System.Windows.Forms.CheckBox cbAppTelData;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox cbNRWA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbEdgeCases;
        private System.Windows.Forms.Button btnRefresh;
    }
}

