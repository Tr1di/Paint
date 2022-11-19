using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Paint
{
    struct CanvasData
    {
        private List<Image> states;
        private int currentState;

        public Graphics Canvas { get; private set; }
        public Image CanvasImage { get; private set; }
        public string FileName { get; private set; }
        public bool IsSaved { get; private set; }

        public CanvasData(Image image, Graphics initCanvas = null, string fileName = null)
        {
            if ( initCanvas != null )
            {
                Canvas = initCanvas;
            }
            else
            {
                Canvas = Graphics.FromImage(image);
            }

            Canvas.SmoothingMode = SmoothingMode.HighQuality;

            CanvasImage = image;
            
            FileName = fileName;
            IsSaved = true;

            currentState = 0;

            states = new List<Image>();
        }

        public void SaveState()
        {
            if ( currentState < states.Count)
            {
                states.RemoveRange(currentState, states.Count - currentState);
            }

            Image img = new Bitmap(CanvasImage);

            if ( !states.Contains(img) )
            {
                states.Add(img);
            }

            currentState = states.Count;

            IsSaved = false;
        }

        public void RestoreLast()
        {
            if ( states.Count != 0 )
            {
                int state = currentState - 1 > -1 ? currentState - 1 : 0;

                Clear();
                Canvas.DrawImage(states[state], new Point(0, 0));
            }
        }

        public void Undo()
        {
            if ( --currentState >= 0 ) 
            {
                Image image = states[currentState];

                if ( currentState == states.Count - 1 )
                {
                    states.Add(new Bitmap(CanvasImage));
                }

                Clear();
                Canvas.DrawImage(image, new Point(0, 0));
            }

            if ( currentState < 1 ) currentState = 1;

            IsSaved = false;
        }

        public void Redo()
        {
            if ( ++currentState < states.Count )
            {
                Image image = states[currentState];

                if ( currentState == states.Count - 1 )
                {
                    states.RemoveAt(currentState - 1);
                }

                Clear();
                Canvas.DrawImage(image, new Point(0, 0));
            }

            if ( currentState > states.Count ) currentState = states.Count;

            IsSaved = false;
        }

        public void Save(string path = null, Image background = null)
        {
            if ( path == null )
            {
                if ( FileName == null)
                {
                    throw new ArgumentNullException("No path for file");
                }
                path = FileName;
            }

            if ( path != FileName )
            {
                FileName = path;
            }

            CanvasImage.Save(path);

            IsSaved = true;
        }

        public void Clear()
        {
            Canvas.Clear(Color.Transparent);
        }
    }
}
