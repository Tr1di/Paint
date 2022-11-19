using System;
using System.Windows.Forms;

namespace Paint
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            foreach ( Control control in Controls )
            {
                control.BackColor = Settings.Theme.Color;
                control.ForeColor = Settings.Theme.FontColor;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
