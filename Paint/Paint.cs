using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Paint
{
    public enum PaintTool
    {
        Brush,
        Line,
        Circle,
        Square,
        Eraser,
        Select
    }

    public partial class Paint : Form
    {
        private CanvasData _data;

        private readonly Pen _pen;
        private PaintTool _currentTool = PaintTool.Brush;
        private bool _isPressed;
        private Point _brushPoint;

        private readonly Pen _selectPen;
        private Rectangle _select;

        public Paint()
        {
            InitializeComponent();

            UpdateLanguage(Settings.Language);

            _pen = new Pen(Color.FromArgb(20, 20, 20), (float)brushSizeNumeric.Value);
            _pen.StartCap = _pen.EndCap = LineCap.Round;

            _selectPen = new Pen(Color.FromArgb(127, 127, 127), 3)  { DashStyle = DashStyle.Dash, DashOffset = 5 };
        }

        private void ResetSelect()
        {
            _select = new Rectangle { Size = canvas.Size };
        }

        private void CreateNewCanvas(Image img, string fileName = null)
        {
            canvas.Size = img.Size;
            canvas.Image = img;

            ResetSelect();

            _data = new CanvasData(img, Graphics.FromImage(img), fileName);
        }

        private void AskToSave(object sender, CancelEventArgs e)
        {
            if ( !_data.IsSaved )
            {
                var fileName = Path.GetFileNameWithoutExtension(_data.FileName);

                fileName ??= "untitled";

                switch (MessageBox.Show("Do you want to save to " + fileName + "?", Text, MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        save_Click(this, new EventArgs());
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            var newColor = new ColorDialog();

            if ( newColor.ShowDialog() == DialogResult.OK )
            {
                colorButton.BackColor = _pen.Color = newColor.Color;
            }
        }

        private void brushWidth_ValueChanged(object sender, EventArgs e)
        {
            _pen.Width = (float)((NumericUpDown)sender).Value;
        }

        private void undo_Click(object sender, EventArgs e)
        {
            _data.Undo();
            canvas.Refresh();
        }

        private void redo_Click(object sender, EventArgs e)
        {
            _data.Redo();
            canvas.Refresh();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            CreateNewCanvas(new Bitmap(1920, 1080));
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            _isPressed = true;

            ResetSelect();

            _brushPoint = e.Location;

            if (_currentTool != PaintTool.Eraser && _currentTool != PaintTool.Select)
            {
                _data.SaveState();
            }

            if ( _currentTool == PaintTool.Select )
            {
                _data.RestoreLast();
                canvas.Refresh();
            }
        }

        private Rectangle DrawRectangle(Pen rectPen, Point location, Size difference)
        {
            var res = new Rectangle(_brushPoint, difference);

            if (location.X >= _brushPoint.X && location.Y >= _brushPoint.Y)
            {
                _data.Canvas.DrawRectangle(rectPen, res);
            }
            else
            {
                difference.Width = Math.Abs(difference.Width);
                difference.Height = Math.Abs(difference.Height);

                var p = _brushPoint;
                if (location.X < _brushPoint.X) p.X -= difference.Width;
                if (location.Y < _brushPoint.Y) p.Y -= difference.Height;

                res = new Rectangle(p, difference);

                _data.Canvas.DrawRectangle(rectPen, res);
            }

            return res;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if ( _isPressed )
            {
                if ( _currentTool != PaintTool.Brush && _currentTool != PaintTool.Eraser )
                {
                    _data.RestoreLast();
                }

                var difference = Size.Subtract(new Size(e.Location), new Size(_brushPoint));

                switch (_currentTool)
                {
                    case PaintTool.Brush:
                        _data.Canvas.DrawLine(_pen, _brushPoint, _brushPoint = e.Location);
                        break;
                    case PaintTool.Line:
                        _data.Canvas.DrawLine(_pen, _brushPoint, e.Location);
                        break;
                    case PaintTool.Circle:
                        _data.Canvas.DrawEllipse(_pen, new Rectangle(_brushPoint - difference, difference * 2));
                        break;
                    case PaintTool.Square:
                        DrawRectangle(_pen, e.Location, difference);
                        break;
                    case PaintTool.Select:
                        _select = DrawRectangle(_selectPen, e.Location, difference);
                        break;
                    case PaintTool.Eraser:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            if ( _currentTool == PaintTool.Eraser )
            {
                if ( !_isPressed )
                {
                    _data.RestoreLast();
                }

                var x = e.Location.X - (int)brushSizeNumeric.Value;
                var y = e.Location.Y - (int)brushSizeNumeric.Value;
                var size = (int)brushSizeNumeric.Value * 2;

                var eracer = new Rectangle(new Point(x, y), new Size(size, size));

                _data.Canvas.FillRectangle(new SolidBrush(Color.Transparent), eracer);
            }

            canvas.Refresh();
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (_currentTool == PaintTool.Eraser)
            {
                _data.SaveState();
            }
            
            _isPressed = false;
        }

        private void tool_Click(object sender, EventArgs e)
        {
            PaintTool tool;

            try
            {
                tool = (PaintTool)toolsDropDownButton.DropDownItems.IndexOf((ToolStripItem)sender);
            }
            catch
            {
                tool = PaintTool.Brush;
            }

            toolsDropDownButton.Image = ((ToolStripItem)sender).Image;

            if ( _currentTool == PaintTool.Select && tool != PaintTool.Select )
            {
                _data.RestoreLast();
            }

            _currentTool = tool;

            if ( _currentTool == PaintTool.Eraser || _currentTool == PaintTool.Select)
            {
                _data.SaveState();
            }

            _data.Canvas.CompositingMode = _currentTool == PaintTool.Eraser ? CompositingMode.SourceCopy : CompositingMode.SourceOver;
        }

        private void saveAs_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "Image files (*.png, *.jpg, *.bmp)|*.png;*.jpg;*.bmp" };

            if ( _data.FileName == null )
            {
                dialog.FileName = "undefined";
                dialog.DefaultExt = "png";
            }
            else
            {
                dialog.FileName = Path.GetFileNameWithoutExtension(_data.FileName);
                dialog.DefaultExt = Path.GetExtension(_data.FileName);
            }

            if ( dialog.ShowDialog() == DialogResult.OK )
            {
                _data.Save(dialog.FileName);
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                _data.Save();
            }
            catch (ArgumentNullException)
            {
                saveAs_Click(sender, e);
            }
        }

        private void open_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Image files (*.png, *.jpg, *.bmp)|*.png;*.jpg;*.bmp" };

            if ( dialog.ShowDialog() == DialogResult.OK )
            {
                CreateNewCanvas(new Bitmap(dialog.FileName));
            }
        }

        private void canvas_DragDrop(object sender, DragEventArgs e)
        {
            var path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            try
            {
                Image img = new Bitmap(path);
                CreateNewCanvas(img, path);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("It wasn't an image.", Text);
            }
        }

        private void canvas_DragEnter(object sender, DragEventArgs e)
        {
            if ( e.Data.GetDataPresent(DataFormats.FileDrop) )
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void new_Click(object sender, EventArgs e)
        {
            var ce = new CancelEventArgs();

            AskToSave(sender, ce);

            if (ce.Cancel) return;

            var dialog = new NewCanvasDialog();

            if ( dialog.ShowDialog() == DialogResult.OK )
            {
                Image img = new Bitmap(dialog.CanvasWidth, dialog.CanvasHeight);

                var g = Graphics.FromImage(img);
                g.FillRectangle(new SolidBrush(dialog.CanvasColor), new Rectangle(new Point(0, 0), img.Size));

                CreateNewCanvas(img);

                canvas.Image = img;
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            _data.Clear();
            canvas.Refresh();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            AskToSave(sender, e);
        }

        private void printDocument(object sender, PrintPageEventArgs e) 
        {
            e.Graphics.DrawImage(canvas.Image, 0, 0);
        }

        private void print_Click(object sender, EventArgs e)
        {
            var document = new PrintDocument();
            document.PrintPage += printDocument;

            var dialog = new PrintDialog { Document = document };

            if ( dialog.ShowDialog() == DialogResult.OK )
            {
                document.Print();
            }
        }

        private void UpdateLanguage(string lang)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

            Controls.Clear();

            Load -= FormLoad;
            FormClosing -= formClosing;
            var img = canvas.Image;
            InitializeComponent();
            canvas.Image = img;
        }

        private void customize_Click(object sender, EventArgs e)
        {
            var dialog = new CustomizeDialog();

            if ( dialog.ShowDialog() == DialogResult.OK )
            {
                Settings.Theme = dialog.Theme;
                Settings.Language = dialog.Language;

                BackColor = Settings.Theme.Color;
                ForeColor = Settings.Theme.FontColor;

                UpdateLanguage(Settings.Language);

                foreach (Control control in Controls)
                {
                    if ( !control.Equals(canvas) && !control.Equals(canvasPanel) )
                    {
                        control.BackColor = Settings.Theme.Color;
                        control.ForeColor = Settings.Theme.FontColor;
                    }
                }
            }
        }

        private void CopySelect(object sender, EventArgs e)
        {
            if ( _currentTool == PaintTool.Select )
            {
                _data.RestoreLast();
            }

            Image img = new Bitmap(_select.Width, _select.Height);

            var gr = Graphics.FromImage(img);
            gr.DrawImage(canvas.Image, new Rectangle(0, 0, _select.Width, _select.Height), _select, GraphicsUnit.Pixel);

            Clipboard.SetImage(img);

            ResetSelect();
        }

        private void cut_Click(object sender, EventArgs e)
        {
            var sel = _select;

            CopySelect(sender, e);

            _data.Canvas.CompositingMode = CompositingMode.SourceCopy;
            _data.Canvas.FillRectangle(new SolidBrush(Color.Transparent), sel);
            _data.Canvas.CompositingMode = CompositingMode.SourceOver;
        }

        private void paste_Click(object sender, EventArgs e)
        {
            if ( !Clipboard.ContainsImage() )
            {
                return;
            }

            if ( _currentTool == PaintTool.Select )
            {
                _data.RestoreLast();
            }

            _data.Canvas.DrawImage(Clipboard.GetImage(), _select.X, _select.Y);

            if ( _currentTool == PaintTool.Select )
            {
                _data.SaveState();
            }

            ResetSelect();
            canvas.Refresh();
        }

        private void about_Click(object sender, EventArgs e)
        { 
            new About().ShowDialog();
        }
    }
}
