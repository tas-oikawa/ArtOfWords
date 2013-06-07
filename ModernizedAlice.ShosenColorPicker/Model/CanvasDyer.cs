using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ModernizedAlice.ShosenColorPicker.Model
{
    public class CanvasDyer
    {
        private double _canvasHeight;
        private double _canvasWidth;
        private double _canvasMargin = 10;

        private double _startR = 30;
        private double _hueSpan;

        public double HueSpan
        {
            get { return _hueSpan; }
            set { _hueSpan = value; }
        }
        private double _saturationSpan;

        public double SaturationSpan
        {
            get { return _saturationSpan; }
            set { _saturationSpan = value; }
        }

        private double _brightness;

        public double Brightness
        {
            get { return _brightness; }
            set { _brightness = value; }
        }


        private double MaxR()
        {
            return Math.Min(_canvasWidth, _canvasHeight) / 2;
        }

        private double RSpan()
        {
            return MaxR() / _saturationSpan;
        }

        public IEnumerable<ColorButton> Dye(Canvas canvas)
        {
            _canvasHeight = canvas.ActualHeight - (_canvasMargin * 2);
            _canvasWidth = canvas.ActualWidth - (_canvasMargin * 2);

            double maxR = MaxR() ;

            double rSpan = RSpan();

            double angleSpan = 360 / _hueSpan;

            canvas.Children.Clear();

            List<ColorButton> colorButtons = new List<ColorButton>();
            
            for (double r = _startR; r < maxR; r += rSpan)
            {
                for (double angle = 0; angle < 360; angle += angleSpan)
                {
                    var button = GenerateColorButton(angle, r);
                    colorButtons.Add(button);
                    canvas.Children.Add(button);
                }
            }

            // 原点のボタンを作る
            var btnO = GenerateOriginColorButton();
            colorButtons.Add(btnO);
            canvas.Children.Add(btnO);

            return colorButtons;
        }

        private double GetRoundWidth(double r)
        {
            return r * Math.PI * 2;
        }

        private Point GetOriginPoint()
        {
            return new Point(_canvasWidth / 2 + _canvasMargin, _canvasHeight / 2 + _canvasMargin);
        }

        private HsvColor GetColor(double angle, double r)
        {
            return new HsvColor((float)angle, (float)(r / MaxR()), (float)Brightness);
        }

        private ColorButton GenerateOriginColorButton()
        {
            double lineWidth = RSpan() - 3;

            ColorButton colorbutton = new ColorButton();
            
            colorbutton.Height = lineWidth;
            colorbutton.Width = lineWidth;
            colorbutton.ClearButtonBorder.CornerRadius = new CornerRadius(100, 100, 100, 100);

            var originPoint = GetOriginPoint();

            double x = originPoint.X - (lineWidth / 2);
            double y = originPoint.Y - (lineWidth / 2);

            Canvas.SetLeft(colorbutton, x);
            Canvas.SetTop(colorbutton, y);

            colorbutton.Color = new HsvColor(0, 0, (float)Brightness);

            return colorbutton;
        }

        private ColorButton GenerateColorButton(double angle, double r)
        {
            double lineWidth = RSpan() - 3;

            ColorButton colorbutton = new ColorButton();
            RotateTransform transform = new RotateTransform(angle);
            colorbutton.RenderTransform = transform;
            colorbutton.Height = lineWidth;
            colorbutton.Width = lineWidth;

            var originPoint = GetOriginPoint();

            double x = Math.Cos(angle / 180 * Math.PI) * r + originPoint.X;
            double y = Math.Sin(angle / 180 * Math.PI)  * r + originPoint.Y;

            Canvas.SetLeft(colorbutton, x);
            Canvas.SetTop(colorbutton, y);

            colorbutton.Color = GetColor(angle, r);

            return colorbutton;
        }
    }
}
