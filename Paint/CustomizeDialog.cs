using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paint
{
    public partial class CustomizeDialog : Form
    {
        private Theme theme = Theme.White;
        public Theme Theme => theme;
        public string Language { get; private set; }

        public CustomizeDialog()
        {
            InitializeComponent();

            BackColor = Settings.Theme.Color;
            ForeColor = Settings.Theme.FontColor;

            foreach (Control control in Controls)
            {
                control.BackColor = Settings.Theme.Color;
                control.ForeColor = Settings.Theme.FontColor;
            }

            themeComboBox.SelectedIndex = 0;
            languageComboBox.SelectedIndex = 1;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void color_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();

            if ( dialog.ShowDialog() == DialogResult.OK)
            {
                theme.Color = dialog.Color;
            }
        }

        private void fontColor_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                theme.FontColor = dialog.Color;
            }
        }

        private void theme_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (themeComboBox.SelectedItem)
            {
            case 0:
                theme = Theme.White;
                colorPanel.Enabled = fontColorPanel.Enabled = false;
                break;
            case 1:
                theme = Theme.Black;
                colorPanel.Enabled = fontColorPanel.Enabled = false;
                break;
            default:
                colorPanel.Enabled = fontColorPanel.Enabled = true;
                break;
            }

            colorPanel.BackColor = theme.Color;
            fontColorPanel.BackColor = theme.FontColor;
        }

        private void language_SelectedIndexChanged(object sender, EventArgs e)
        {
            Language = languageComboBox.SelectedIndex switch
            {
                0 => Settings.EN,
                1 => Settings.RU,
                _ => default
            };
        }
    }
}
