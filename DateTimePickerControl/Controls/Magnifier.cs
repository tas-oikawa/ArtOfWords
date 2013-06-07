using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Windows.Threading;

namespace AC.AvalonControlsLibrary.Controls
{
    /// <summary>
    /// Control that acts like a normal slider yet has the functionality of pushing new values while holding the thumb
    /// </summary>
    public class Magnifier : Slider
    {
        /// <summary>
        /// Raises when the slider is being zoom in or out
        /// </summary>
        public event EventHandler<MagnifyEventArgs> Magnify;

        private TimeSpan centeredAnimationDuration;

        /// <summary>
        /// The duration of the animation to animate the slider back to the center point
        /// </summary>
        public TimeSpan CenteredAnimationDuration
        {
            get { return centeredAnimationDuration; }
            set { centeredAnimationDuration = value; }
        }

        private TimeSpan magnifyInterval;
        /// <summary>
        /// The interval rate to raise the magnify event while user is holding the magnify
        /// </summary>
        public TimeSpan MagnifyInterval
        {
            get { return magnifyInterval; }
            set
            {
                magnifyInterval = value;
                timer.Interval = value;
            }
        }

        private double midPoint;

        /// <summary>
        /// Gets the mid point of the slider
        /// </summary>
        public double MidPoint
        {
            get { return midPoint; }
        }

        //timer used to raise the magnify events
        DispatcherTimer timer;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Magnifier()
        {
            //set default duration
            centeredAnimationDuration = TimeSpan.FromMilliseconds(500);
            magnifyInterval = TimeSpan.FromMilliseconds(250);

            EventHandler handler = delegate { CalculateMidPoint(); };
            //hook to maximum and minum value change
            DependencyPropertyDescriptor.FromProperty(MaximumProperty, typeof(Slider)).AddValueChanged(this, handler);
            DependencyPropertyDescriptor.FromProperty(MinimumProperty, typeof(Slider)).AddValueChanged(this, handler);

            //register to events of drag completed to animate back to the mid point
            EventManager.RegisterClassHandler(typeof(Magnifier), Thumb.DragCompletedEvent, new RoutedEventHandler(DragCompleted));

            //timer for the zoom slider to expand the range selected
            timer = new DispatcherTimer();
            timer.Interval = magnifyInterval;
            timer.Tick += delegate
            {
                if (Value != midPoint)
                    OnMagnify(new MagnifyEventArgs(Value - midPoint));
            };
            timer.Start();
        }

        //calculates the mid point
        private void CalculateMidPoint()
        {
            midPoint = Math.Max(Maximum - Minimum, 1) / 2;
            Value = midPoint;
        }

        //animate the zoom slider back to the mid point
        private void DragCompleted(object sender, RoutedEventArgs e)
        {
            DoubleAnimation zoomSlideranimation = new DoubleAnimation(Value, midPoint,
                new Duration(CenteredAnimationDuration), FillBehavior.Stop);
            //begin animation for slider to slide back in the mid point
            BeginAnimation(Slider.ValueProperty, zoomSlideranimation);
            Value = midPoint;
        }

        /// <summary>
        /// Raises the magnify event
        /// </summary>
        /// <param name="magnifyChange">The change to pass</param>
        protected void OnMagnify(MagnifyEventArgs magnifyChange)
        {
            if (Magnify != null)
                Magnify(this, magnifyChange);
        }
    }

    /// <summary>
    /// Event arguments for the magnify event
    /// </summary>
    public class MagnifyEventArgs : EventArgs
    {
        private double magnifyBy;

        /// <summary>
        /// Returns the value to magnify by
        /// </summary>
        public double MagnifiedBy
        {
            get { return magnifyBy; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="magnifyBy">The value to magnify by</param>
        public MagnifyEventArgs(double magnifyBy)
        {
            this.magnifyBy = magnifyBy;
        }
    }
}
