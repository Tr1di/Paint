namespace Paint
{
    partial class CustomizeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomizeDialog));
            this.themeComboBox = new System.Windows.Forms.ComboBox();
            this.colorPanel = new System.Windows.Forms.Panel();
            this.fontColorPanel = new System.Windows.Forms.Panel();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.themeLabel = new System.Windows.Forms.Label();
            this.colorLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fontLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // themeComboBox
            // 
            this.themeComboBox.FormattingEnabled = true;
            this.themeComboBox.Items.AddRange(new object[] {
            resources.GetString("themeComboBox.Items"),
            resources.GetString("themeComboBox.Items1"),
            resources.GetString("themeComboBox.Items2")});
            resources.ApplyResources(this.themeComboBox, "themeComboBox");
            this.themeComboBox.Name = "themeComboBox";
            this.themeComboBox.SelectedIndexChanged += new System.EventHandler(this.theme_SelectedIndexChanged);
            // 
            // colorPanel
            // 
            this.colorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.colorPanel, "colorPanel");
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Click += new System.EventHandler(this.color_Click);
            // 
            // fontColorPanel
            // 
            this.fontColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fontColorPanel, "fontColorPanel");
            this.fontColorPanel.Name = "fontColorPanel";
            this.fontColorPanel.Click += new System.EventHandler(this.fontColor_Click);
            // 
            // languageComboBox
            // 
            this.languageComboBox.DisplayMember = "2";
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Items.AddRange(new object[] {
            resources.GetString("languageComboBox.Items"),
            resources.GetString("languageComboBox.Items1")});
            resources.ApplyResources(this.languageComboBox, "languageComboBox");
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.SelectedIndexChanged += new System.EventHandler(this.language_SelectedIndexChanged);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // themeLabel
            // 
            resources.ApplyResources(this.themeLabel, "themeLabel");
            this.themeLabel.Name = "themeLabel";
            // 
            // colorLabel
            // 
            resources.ApplyResources(this.colorLabel, "colorLabel");
            this.colorLabel.Name = "colorLabel";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // fontLabel
            // 
            resources.ApplyResources(this.fontLabel, "fontLabel");
            this.fontLabel.Name = "fontLabel";
            // 
            // CustomizeDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fontLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.colorLabel);
            this.Controls.Add(this.themeLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.languageComboBox);
            this.Controls.Add(this.fontColorPanel);
            this.Controls.Add(this.colorPanel);
            this.Controls.Add(this.themeComboBox);
            this.Name = "CustomizeDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox themeComboBox;
        private System.Windows.Forms.Panel colorPanel;
        private System.Windows.Forms.Panel fontColorPanel;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label themeLabel;
        private System.Windows.Forms.Label colorLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label fontLabel;
    }
}