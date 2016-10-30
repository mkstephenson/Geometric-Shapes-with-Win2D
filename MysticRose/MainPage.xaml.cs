using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MysticRose
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ConcurrentStack<Line> segments = new ConcurrentStack<Line>();
        private ICanvasBrush brush;
        private int numberOfTopLevelLines = 12;
        private float lineThickness = 3;
        private Size currentSize;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void CanvasAnimatedControl_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            brush = new CanvasSolidColorBrush(sender, Colors.Navy);
        }

        private void CanvasAnimatedControl_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            foreach (var line in segments)
            {

            }
        }

        private void CanvasAnimatedControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            foreach (var line in segments)
            {
                args.DrawingSession.DrawLine(line.StartPoint, line.EndPoint, brush, lineThickness);
            }
        }

        private void CanvasAnimatedControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            currentSize = e.NewSize;
            ResetCircle();
        }

        private void ResetCircle()
        {
            segments.Clear();

            /// Create the initial point set
            var listOfPointsOnCircle = new Vector2[numberOfTopLevelLines];
            var center = new Vector2((float)currentSize.Width / 2, (float)currentSize.Height / 2);
            var radius = (Math.Min(currentSize.Width, currentSize.Height) / 2) - 50;

            for (int i = 0; i < numberOfTopLevelLines; i++)
            {
                var theta = ((2 * Math.PI) / numberOfTopLevelLines) * i;

                var x = center.X + radius * Math.Cos(theta);
                var y = center.Y + radius * Math.Sin(theta);

                listOfPointsOnCircle[i] = new Vector2((float)x, (float)y);
            }

            List<Line> rose = new List<Line>();

            foreach (var firstPoint in listOfPointsOnCircle)
            {
                foreach (var secondPoint in listOfPointsOnCircle)
                {
                    if (firstPoint != secondPoint)
                    {
                        rose.Add(new Line
                        {
                            StartPoint = firstPoint,
                            EndPoint = secondPoint
                        });
                    }
                }
            }

            segments.PushRange(rose.ToArray());
        }

        private void numSegments_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            numberOfTopLevelLines = (int)Math.Round(e.NewValue, 0);
            ResetCircle();
        }

        private void lineThickness_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            lineThickness = (float)Math.Round(e.NewValue, 0);
            ResetCircle();
        }
    }

    public struct Line
    {
        public Vector2 StartPoint { get; set; }
        public Vector2 EndPoint { get; set; }
    }
}
