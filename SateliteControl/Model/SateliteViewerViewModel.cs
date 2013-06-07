using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SateliteControl.Model
{
    public class SateliteViewerViewModel : INotifyPropertyChanged
    {
        private SateliteViewer _view;

        private bool _doShowRotateControl;

        public bool DoShowRotateControl
        {
            get { return _doShowRotateControl; }
            set 
            {
                if (_doShowRotateControl != value)
                {
                    _doShowRotateControl = value;
                    OnPropertyChanged("DoShowRotateControl");
                }
            }
        }

        private bool _doShowExpandControl;

        public bool DoShowExpandControl
        {
            get { return _doShowExpandControl; }
            set
            {
                if (_doShowExpandControl != value)
                {
                    _doShowExpandControl = value;
                    OnPropertyChanged("DoShowExpandControl");
                }
            }
        }


        private bool _isExpanding;

        public bool IsExpanding
        {
            get { return _isExpanding; }
            set
            {
                if (_isExpanding != value)
                {
                    _isExpanding = value;
                    DoShowRotateControl = false;
                    OnPropertyChanged("IsExpanding");
                    OnPropertyChanged("IsNotExpanding");
                    OnPropertyChanged("DoShowRotateControl");
                    OnPropertyChanged("DoShowExpandControl");
                }
            }
        }

        private double _gridSize = 200;

        public double GridSize
        {
            get { return _gridSize; }
            set 
            {
                if (_gridSize != value)
                {
                    _gridSize = value;
                    _windowSize = GetResetSize();
                    _view.Height = _windowSize;
                    _view.Width = _windowSize;
                    OnPropertyChanged("WindowSize");
                }
            }
        }
        private double _windowSize;

        public double WindowSize
        {
            get
            {                
                return _windowSize;
            }
        }

        public bool IsNotExpanding
        {
            get { return !_isExpanding; }
        }

        public SateliteViewerViewModel(SateliteViewer viewer)
        {
            _doShowRotateControl = false;
            _windowSize = GetResetSize();
            _view = viewer;
        }

        private double GetResetSize()
        {
            if (IsExpanding)
            {
                return (GridSize * 2) + 40;
            }
            else
            {
                return GridSize + 40;
            }
        }

        private void ResetHeightSize()
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = _view.Width,
                To = GetResetSize(),
                Duration = TimeSpan.FromMilliseconds(250)
            };

            var ease = new PowerEase();
            ease.Power = 4;
            ease.EasingMode = EasingMode.EaseInOut;
            animation.EasingFunction = ease;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Height"));
            storyboard.Children.Add(animation);

            _view.BeginStoryboard(storyboard);  
        }


        private void ResetWidthSize()
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = _view.Width,
                To = GetResetSize(),
                Duration = TimeSpan.FromMilliseconds(250)
            };

            var ease = new PowerEase();
            ease.Power = 4;
            ease.EasingMode = EasingMode.EaseInOut;
            animation.EasingFunction = ease;

            storyboard.Completed += storyboardWidthChange_Completed;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            storyboard.Children.Add(animation);

            _view.BeginStoryboard(storyboard);  
        }

        private void storyboardWidthChange_Completed(object sender, EventArgs e)
        {
            ResetHeightSize();
        }

        private void ResetViewSize()
        {
            ResetWidthSize();
        }

        private void ResetControlCard()
        {
            if (IsExpanding)
            {
                _view.MyCard.SetSquareMode();
            }
            else
            {
                _view.MyCard.SetRotateMode();
            }
        }

        public void OnExpandButtonClicked()
        {
            IsExpanding = !IsExpanding;
            ResetControlCard();
            ResetViewSize();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
