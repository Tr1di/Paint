using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paint
{
    public partial class NewCanvasDialog : Form
    {
        public int CanvasWidth => (int)widthNumeric.Value;
        public int CanvasHeight => (int)heightNumeric.Value;
        public bool IsTransparent => transparentCheckBox.Checked;
        public Color CanvasColor => IsTransparent ? Color.Transparent : bgColor.BackColor;

        public NewCanvasDialog()
        {
            InitializeComponent();

            BackColor = Settings.Theme.Color;
            ForeColor = Settings.Theme.FontColor;

            foreach (Control control in Controls)
            {
                control.BackColor = Settings.Theme.Color;
                control.ForeColor = Settings.Theme.FontColor;
            }
        }

        private void transparentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bgColor.Enabled = !transparentCheckBox.Checked;
        }

        private void bgColor_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();

            if ( dialog.ShowDialog() == DialogResult.OK )
            {
                bgColor.BackColor = dialog.Color;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
