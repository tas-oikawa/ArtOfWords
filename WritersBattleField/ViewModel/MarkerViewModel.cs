using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace WritersBattleField.ViewModel
{
    public class MarkerViewModel
    {
        public ICollection<IMarkable> MarkableObjects { set; get; }

        private ICollection<MarkingButtonObject> MarkButtons { set; get; }

        private class PushButtonChangedArgs : PropertyChangedEventArgs
        {
            public bool BeTrue;
            public PushButtonChangedArgs(string str, bool beTrue) : base(str)
            {
                BeTrue = beTrue;
            }
        }
        /// <summary>
        /// MarkingButtonObj
        /// </summary>
        private class MarkingButtonObject : INotifyPropertyChanged
        {
            public IMarkable Mark;
            public ToggleButton Button;
            private bool _isPushing;
            public bool IsPushing
            {
                set
                {
                    if (_isPushing != value)
                    {
                        _isPushing = value;

                        OnPropertyChanged("IsPushing", value);
                    }
                }
                get
                {
                    return _isPushing;
                }
            }

            #region INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name, bool status)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PushButtonChangedArgs(name, status));
                }
            }

            #endregion
        }

        public ToggleButton GetMarkingButton(IMarkable markable)
        {
            ToggleButton button = new ToggleButton();

            button.Content = markable.Symbol;
            button.Background = markable.ColorBrush;
            button.Margin = new Thickness(0, 0, 10, 0);
            button.Padding = new Thickness(5, 0, 5, 0);


            return button;
        }

        public StackPanel GetMarkingPanel()
        {
            MarkButtons = new List<MarkingButtonObject>();

            StackPanel panel = new StackPanel();

            panel.Orientation = Orientation.Horizontal;
            foreach (var mark in MarkableObjects)
            {
                var btn = GetMarkingButton(mark);
                var addBtnObj = new MarkingButtonObject() { Button = btn, Mark = mark, IsPushing = false };

                // Binding設定
                Binding BindingWeight = new Binding("IsPushing");
                BindingWeight.Mode = BindingMode.TwoWay;

                btn.SetBinding(ToggleButton.IsCheckedProperty, BindingWeight);
                btn.DataContext = addBtnObj;

                addBtnObj.PropertyChanged += PushStatusChangedEventFired;
                panel.Children.Add(btn);
                MarkButtons.Add(addBtnObj);
            }

            return panel;
        }

        public IMarkable GetSelectingMark()
        {
            if (MarkButtons == null)
            {
                return null;
            }

            var markable =  from obj in MarkButtons
                            where obj.Button.IsChecked == true
                            select obj.Mark;

            if (markable.Count() == 0)
            {
                return null;
            }

            return markable.First();
        }

        public void AllButtonBeUnpushed()
        {
            foreach (var btn in MarkButtons)
            {
                btn.IsPushing = false;
            }
        }

        #region バインディングオブジェクトのイベントを拾う
        public void PushStatusChangedEventFired(object obj, PropertyChangedEventArgs arg)
        {
            var btnArg = arg as PushButtonChangedArgs;

            foreach (var btnobj in MarkButtons)
            {
                if (btnobj != obj && btnArg.BeTrue)
                {
                    btnobj.IsPushing = false;
                }
            }

            if (MarkChanged != null)
            {
                MarkChanged(obj, new SelectingMarkChangedEventArgs() { SelectingMark = GetSelectingMark() });
            }
        }

        public delegate void MarkChangedEvent(object sender, SelectingMarkChangedEventArgs e);

        public event MarkChangedEvent MarkChanged;

        #endregion
    }
}
