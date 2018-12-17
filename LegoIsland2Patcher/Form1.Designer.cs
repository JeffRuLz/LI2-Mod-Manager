namespace LegoIsland2Patcher
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
            System.Windows.Forms.LinkLabel linkLabel1;
            this.btnApply = new System.Windows.Forms.Button();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.clbPatches = new System.Windows.Forms.CheckedListBox();
            this.rtbDescription = new System.Windows.Forms.RichTextBox();
            this.gbResolution = new System.Windows.Forms.GroupBox();
            this.cbEnableAspect = new System.Windows.Forms.CheckBox();
            this.tbHeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbEnableResolution = new System.Windows.Forms.CheckBox();
            this.lblExeVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLoadFix = new System.Windows.Forms.CheckBox();
            this.cbNoVideos = new System.Windows.Forms.CheckBox();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.gbResolution.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new System.Drawing.Point(544, 277);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(138, 13);
            linkLabel1.TabIndex = 6;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "www.rockraidersunited.com";
            linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApply.Location = new System.Drawing.Point(12, 296);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // linkLabel2
            // 
            this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(460, 301);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(222, 13);
            this.linkLabel2.TabIndex = 8;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "www.github.com/JeffRuLz/LI2-Mod-Manager";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // clbPatches
            // 
            this.clbPatches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbPatches.FormattingEnabled = true;
            this.clbPatches.HorizontalScrollbar = true;
            this.clbPatches.Location = new System.Drawing.Point(12, 12);
            this.clbPatches.Name = "clbPatches";
            this.clbPatches.Size = new System.Drawing.Size(477, 199);
            this.clbPatches.TabIndex = 9;
            this.clbPatches.SelectedIndexChanged += new System.EventHandler(this.clbPatches_SelectedIndexChanged);
            // 
            // rtbDescription
            // 
            this.rtbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbDescription.Location = new System.Drawing.Point(12, 217);
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.ReadOnly = true;
            this.rtbDescription.Size = new System.Drawing.Size(477, 73);
            this.rtbDescription.TabIndex = 10;
            this.rtbDescription.Text = "";
            // 
            // gbResolution
            // 
            this.gbResolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbResolution.Controls.Add(this.cbEnableAspect);
            this.gbResolution.Controls.Add(this.tbHeight);
            this.gbResolution.Controls.Add(this.label3);
            this.gbResolution.Controls.Add(this.tbWidth);
            this.gbResolution.Controls.Add(this.label2);
            this.gbResolution.Enabled = false;
            this.gbResolution.Location = new System.Drawing.Point(507, 64);
            this.gbResolution.Name = "gbResolution";
            this.gbResolution.Size = new System.Drawing.Size(175, 95);
            this.gbResolution.TabIndex = 11;
            this.gbResolution.TabStop = false;
            this.gbResolution.Text = "Custom Resolution";
            this.gbResolution.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cbEnableAspect
            // 
            this.cbEnableAspect.AutoSize = true;
            this.cbEnableAspect.Location = new System.Drawing.Point(9, 69);
            this.cbEnableAspect.Name = "cbEnableAspect";
            this.cbEnableAspect.Size = new System.Drawing.Size(112, 17);
            this.cbEnableAspect.TabIndex = 5;
            this.cbEnableAspect.Text = "Widescreen Hack";
            this.cbEnableAspect.UseVisualStyleBackColor = true;
            // 
            // tbHeight
            // 
            this.tbHeight.Location = new System.Drawing.Point(52, 43);
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(115, 20);
            this.tbHeight.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Height";
            // 
            // tbWidth
            // 
            this.tbWidth.Location = new System.Drawing.Point(52, 17);
            this.tbWidth.Name = "tbWidth";
            this.tbWidth.Size = new System.Drawing.Size(115, 20);
            this.tbWidth.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Width";
            // 
            // cbEnableResolution
            // 
            this.cbEnableResolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEnableResolution.AutoSize = true;
            this.cbEnableResolution.Location = new System.Drawing.Point(507, 41);
            this.cbEnableResolution.Name = "cbEnableResolution";
            this.cbEnableResolution.Size = new System.Drawing.Size(139, 17);
            this.cbEnableResolution.TabIndex = 12;
            this.cbEnableResolution.Text = "Use a custom resolution";
            this.cbEnableResolution.UseVisualStyleBackColor = true;
            this.cbEnableResolution.CheckedChanged += new System.EventHandler(this.cbEnableResolution_CheckedChanged);
            // 
            // lblExeVersion
            // 
            this.lblExeVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExeVersion.AutoSize = true;
            this.lblExeVersion.Location = new System.Drawing.Point(497, 25);
            this.lblExeVersion.Name = "lblExeVersion";
            this.lblExeVersion.Size = new System.Drawing.Size(69, 13);
            this.lblExeVersion.TabIndex = 13;
            this.lblExeVersion.Text = "[Exe Version]";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(497, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Game Version:";
            // 
            // cbLoadFix
            // 
            this.cbLoadFix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLoadFix.AutoSize = true;
            this.cbLoadFix.Location = new System.Drawing.Point(507, 165);
            this.cbLoadFix.Name = "cbLoadFix";
            this.cbLoadFix.Size = new System.Drawing.Size(103, 17);
            this.cbLoadFix.TabIndex = 15;
            this.cbLoadFix.Text = "Load Screen Fix";
            this.cbLoadFix.UseVisualStyleBackColor = true;
            // 
            // cbNoVideos
            // 
            this.cbNoVideos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbNoVideos.AutoSize = true;
            this.cbNoVideos.Location = new System.Drawing.Point(507, 188);
            this.cbNoVideos.Name = "cbNoVideos";
            this.cbNoVideos.Size = new System.Drawing.Size(99, 17);
            this.cbNoVideos.TabIndex = 16;
            this.cbNoVideos.Text = "No Intro Videos";
            this.cbNoVideos.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 331);
            this.Controls.Add(this.cbNoVideos);
            this.Controls.Add(this.cbLoadFix);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblExeVersion);
            this.Controls.Add(this.cbEnableResolution);
            this.Controls.Add(this.gbResolution);
            this.Controls.Add(this.rtbDescription);
            this.Controls.Add(this.clbPatches);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(linkLabel1);
            this.Controls.Add(this.btnApply);
            this.MinimumSize = new System.Drawing.Size(370, 280);
            this.Name = "Form1";
            this.Text = "LI2 Mod Manager (v1.23)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbResolution.ResumeLayout(false);
            this.gbResolution.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.CheckedListBox clbPatches;
        private System.Windows.Forms.RichTextBox rtbDescription;
        private System.Windows.Forms.GroupBox gbResolution;
        private System.Windows.Forms.CheckBox cbEnableResolution;
        private System.Windows.Forms.TextBox tbHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbEnableAspect;
        private System.Windows.Forms.Label lblExeVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbLoadFix;
        private System.Windows.Forms.CheckBox cbNoVideos;
    }
}

