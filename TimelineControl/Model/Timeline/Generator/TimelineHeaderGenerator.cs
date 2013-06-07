using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimelineControl.Model
{
    public class TimelineHeaderGenerator
    {
        private ICollection<TimelineAxis> AxisDataCollection;

        public TimelineHeaderGenerator(ICollection<TimelineAxis> axis)
        {
            AxisDataCollection = axis;
        }


        private Border VacantBorder(double width)
        {
            return new Border()
            {
                Width = width,
            };
        }

        private Border GenerateBorder(TimelineAxis model, double x, double y)
        {
            var border = VacantBorder(model.Width);
            border.DataContext = model;
            Canvas.SetTop(border, y);
            Canvas.SetLeft(border, x);
            

            TextBlock block = new TextBlock()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Foreground = Brushes.Black,
                Text = model.HeaderName,
            };

            border.Child = block;

            return border;
        }

        public void Generate(Canvas canvas)
        {
            double currentX = 0.0;
            foreach (var data in AxisDataCollection)
            {
                if (data.IsDisplayed == false)
                {
                    continue;
                }

                canvas.Children.Add(GenerateBorder(data, currentX, 0.0));
                currentX += data.Width;
            }
        }
    }
}
