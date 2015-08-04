namespace ImageScan
{
    partial class ImageScanMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageScanMainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblInfoText4 = new System.Windows.Forms.Label();
            this.lblInfoText3 = new System.Windows.Forms.Label();
            this.btnSavePicture = new System.Windows.Forms.Button();
            this.lblInfoText2 = new System.Windows.Forms.Label();
            this.btnScanPicture = new System.Windows.Forms.Button();
            this.lblInfoText1 = new System.Windows.Forms.Label();
            this.lblInfoTextHeading = new System.Windows.Forms.Label();
            this.btnScanBarCode = new System.Windows.Forms.Button();
            this.picImageVideo = new System.Windows.Forms.PictureBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.StatusStripInfo = new System.Windows.Forms.StatusStrip();
            this.StripStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lnkInfo = new System.Windows.Forms.LinkLabel();
            this.btnExit = new System.Windows.Forms.Button();
            this.chkVideoViewFinderEnable = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.lblImage = new System.Windows.Forms.Label();
            this.lblBarCode = new System.Windows.Forms.Label();
            this.txtParcelNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timerDateTime = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImageVideo)).BeginInit();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.StatusStripInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Lavender;
            this.panel1.Controls.Add(this.lblInfoText4);
            this.panel1.Controls.Add(this.lblInfoText3);
            this.panel1.Controls.Add(this.btnSavePicture);
            this.panel1.Controls.Add(this.lblInfoText2);
            this.panel1.Controls.Add(this.btnScanPicture);
            this.panel1.Controls.Add(this.lblInfoText1);
            this.panel1.Controls.Add(this.lblInfoTextHeading);
            this.panel1.Controls.Add(this.btnScanBarCode);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 744);
            this.panel1.TabIndex = 0;
            // 
            // lblInfoText4
            // 
            this.lblInfoText4.AutoSize = true;
            this.lblInfoText4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoText4.Location = new System.Drawing.Point(12, 715);
            this.lblInfoText4.Name = "lblInfoText4";
            this.lblInfoText4.Size = new System.Drawing.Size(277, 18);
            this.lblInfoText4.TabIndex = 8;
            this.lblInfoText4.Text = "Avbryt ved å bruke Avbryt eller Esc tasten";
            // 
            // lblInfoText3
            // 
            this.lblInfoText3.AutoSize = true;
            this.lblInfoText3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoText3.Location = new System.Drawing.Point(30, 496);
            this.lblInfoText3.Name = "lblInfoText3";
            this.lblInfoText3.Size = new System.Drawing.Size(82, 18);
            this.lblInfoText3.TabIndex = 7;
            this.lblInfoText3.Text = "lblInfoText3";
            // 
            // btnSavePicture
            // 
            this.btnSavePicture.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSavePicture.Location = new System.Drawing.Point(59, 546);
            this.btnSavePicture.Name = "btnSavePicture";
            this.btnSavePicture.Size = new System.Drawing.Size(196, 48);
            this.btnSavePicture.TabIndex = 2;
            this.btnSavePicture.Text = "Bildet er i orden";
            this.btnSavePicture.UseVisualStyleBackColor = true;
            this.btnSavePicture.Click += new System.EventHandler(this.btnSavePicture_Click);
            // 
            // lblInfoText2
            // 
            this.lblInfoText2.AutoSize = true;
            this.lblInfoText2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoText2.Location = new System.Drawing.Point(30, 273);
            this.lblInfoText2.Name = "lblInfoText2";
            this.lblInfoText2.Size = new System.Drawing.Size(82, 18);
            this.lblInfoText2.TabIndex = 5;
            this.lblInfoText2.Text = "lblInfoText2";
            // 
            // btnScanPicture
            // 
            this.btnScanPicture.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanPicture.Location = new System.Drawing.Point(59, 395);
            this.btnScanPicture.Name = "btnScanPicture";
            this.btnScanPicture.Size = new System.Drawing.Size(196, 48);
            this.btnScanPicture.TabIndex = 1;
            this.btnScanPicture.Text = "Ta &bilde av etikett";
            this.btnScanPicture.UseVisualStyleBackColor = true;
            this.btnScanPicture.Click += new System.EventHandler(this.btnScanPicture_Click);
            // 
            // lblInfoText1
            // 
            this.lblInfoText1.AutoSize = true;
            this.lblInfoText1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoText1.Location = new System.Drawing.Point(30, 51);
            this.lblInfoText1.Name = "lblInfoText1";
            this.lblInfoText1.Size = new System.Drawing.Size(82, 18);
            this.lblInfoText1.TabIndex = 3;
            this.lblInfoText1.Text = "lblInfoText1";
            // 
            // lblInfoTextHeading
            // 
            this.lblInfoTextHeading.AutoSize = true;
            this.lblInfoTextHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoTextHeading.Location = new System.Drawing.Point(25, 12);
            this.lblInfoTextHeading.Name = "lblInfoTextHeading";
            this.lblInfoTextHeading.Size = new System.Drawing.Size(68, 25);
            this.lblInfoTextHeading.TabIndex = 2;
            this.lblInfoTextHeading.Text = "Klar...";
            // 
            // btnScanBarCode
            // 
            this.btnScanBarCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanBarCode.Location = new System.Drawing.Point(59, 177);
            this.btnScanBarCode.Name = "btnScanBarCode";
            this.btnScanBarCode.Size = new System.Drawing.Size(196, 48);
            this.btnScanBarCode.TabIndex = 0;
            this.btnScanBarCode.Text = "Les av &strekkode";
            this.btnScanBarCode.UseVisualStyleBackColor = true;
            this.btnScanBarCode.Click += new System.EventHandler(this.btnScanBarCode_Click);
            // 
            // picImageVideo
            // 
            this.picImageVideo.BackColor = System.Drawing.Color.Gainsboro;
            this.picImageVideo.Location = new System.Drawing.Point(335, 138);
            this.picImageVideo.Name = "picImageVideo";
            this.picImageVideo.Size = new System.Drawing.Size(589, 551);
            this.picImageVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImageVideo.TabIndex = 2;
            this.picImageVideo.TabStop = false;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.StatusStripInfo);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.BackColor = System.Drawing.Color.White;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.lnkInfo);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnExit);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.chkVideoViewFinderEnable);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnCancel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.lblDateTime);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.lblImage);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.lblBarCode);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.txtParcelNumber);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.label1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.picImageVideo);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(952, 746);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(952, 768);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // StatusStripInfo
            // 
            this.StatusStripInfo.Dock = System.Windows.Forms.DockStyle.None;
            this.StatusStripInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripStatusMessage});
            this.StatusStripInfo.Location = new System.Drawing.Point(0, 0);
            this.StatusStripInfo.Name = "StatusStripInfo";
            this.StatusStripInfo.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.StatusStripInfo.Size = new System.Drawing.Size(952, 22);
            this.StatusStripInfo.TabIndex = 1;
            // 
            // StripStatusMessage
            // 
            this.StripStatusMessage.Name = "StripStatusMessage";
            this.StripStatusMessage.Size = new System.Drawing.Size(40, 17);
            this.StripStatusMessage.Text = "Klar...";
            // 
            // lnkInfo
            // 
            this.lnkInfo.AutoSize = true;
            this.lnkInfo.Location = new System.Drawing.Point(844, 52);
            this.lnkInfo.Name = "lnkInfo";
            this.lnkInfo.Size = new System.Drawing.Size(80, 13);
            this.lnkInfo.TabIndex = 15;
            this.lnkInfo.TabStop = true;
            this.lnkInfo.Text = "Om ImageScan";
            this.lnkInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkInfo_LinkClicked);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(804, 704);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(120, 30);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "Avslutt";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // chkVideoViewFinderEnable
            // 
            this.chkVideoViewFinderEnable.AutoSize = true;
            this.chkVideoViewFinderEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVideoViewFinderEnable.Location = new System.Drawing.Point(338, 714);
            this.chkVideoViewFinderEnable.Name = "chkVideoViewFinderEnable";
            this.chkVideoViewFinderEnable.Size = new System.Drawing.Size(146, 20);
            this.chkVideoViewFinderEnable.TabIndex = 12;
            this.chkVideoViewFinderEnable.Text = "Aktiver videomodus";
            this.chkVideoViewFinderEnable.UseVisualStyleBackColor = true;
            this.chkVideoViewFinderEnable.CheckedChanged += new System.EventHandler(this.chkVideoViewFinderEnable_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(661, 704);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Avbryt";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateTime.Location = new System.Drawing.Point(783, 13);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(46, 18);
            this.lblDateTime.TabIndex = 10;
            this.lblDateTime.Text = "Klar...";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImage.Location = new System.Drawing.Point(335, 117);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(44, 18);
            this.lblImage.TabIndex = 9;
            this.lblImage.Text = "Bilde:";
            // 
            // lblBarCode
            // 
            this.lblBarCode.AutoSize = true;
            this.lblBarCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBarCode.Location = new System.Drawing.Point(335, 52);
            this.lblBarCode.Name = "lblBarCode";
            this.lblBarCode.Size = new System.Drawing.Size(80, 18);
            this.lblBarCode.TabIndex = 8;
            this.lblBarCode.Text = "Strekkode:";
            // 
            // txtParcelNumber
            // 
            this.txtParcelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParcelNumber.Location = new System.Drawing.Point(338, 73);
            this.txtParcelNumber.Name = "txtParcelNumber";
            this.txtParcelNumber.Size = new System.Drawing.Size(586, 35);
            this.txtParcelNumber.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(330, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Sendingens tollinformasjon";
            // 
            // timerDateTime
            // 
            this.timerDateTime.Enabled = true;
            this.timerDateTime.Interval = 1000;
            this.timerDateTime.Tick += new System.EventHandler(this.timerDateTime_Tick);
            // 
            // ImageScanMainForm
            // 
            this.AcceptButton = this.btnScanBarCode;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(952, 768);
            this.Controls.Add(this.toolStripContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ImageScanMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageScan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageScanMainForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImageVideo)).EndInit();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.StatusStripInfo.ResumeLayout(false);
            this.StatusStripInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picImageVideo;
        private System.Windows.Forms.Button btnScanBarCode;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.Label lblInfoTextHeading;
        private System.Windows.Forms.Label lblInfoText1;
        private System.Windows.Forms.Label lblInfoText2;
        private System.Windows.Forms.Button btnScanPicture;
        private System.Windows.Forms.TextBox txtParcelNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInfoText3;
        private System.Windows.Forms.Button btnSavePicture;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.Label lblBarCode;
        private System.Windows.Forms.Timer timerDateTime;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.StatusStrip StatusStripInfo;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox chkVideoViewFinderEnable;
        private System.Windows.Forms.ToolStripStatusLabel StripStatusMessage;
        private System.Windows.Forms.Label lblInfoText4;
        private System.Windows.Forms.LinkLabel lnkInfo;
    }
}

