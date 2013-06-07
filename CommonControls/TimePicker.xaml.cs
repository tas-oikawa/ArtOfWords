using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonControls
{
    /// <summary>
    /// Timepicker.xaml の相互作用ロジック
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(TimeSpan), typeof(TimePicker), new FrameworkPropertyMetadata(new TimeSpan(00, 00, 00), new PropertyChangedCallback(OnCurrentTimeChanged)));

        public TimeSpan CurrentTime
        {
            get { return (TimeSpan)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }



        public TimePicker()
        {
            InitializeComponent();
        }

        private void SetTimeToString()
        {
            TimeStrTextBox.Text = new DateTime(CurrentTime.Ticks).ToString("HH:mm:ss");
        }

        private bool TryParse(out DateTime getDate)
        {
            string[] expectedFormats = { "H:m:s", "H:m", "HHmm", "HHmmss" };
            if (DateTime.TryParseExact(TimeStrTextBox.Text, expectedFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out getDate))
            {
                return true;
            }
            return false;
        }

        public void TrySetToDateTime()
        {
            DateTime getDate;

            if (TryParse(out getDate))
            {
                CurrentTime = new TimeSpan(getDate.Hour, getDate.Minute, getDate.Second);
            }
        }

        // 依存プロパティが変更されたときの処理
        private static void OnCurrentTimeChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePicker userControl = obj as TimePicker;

            if (userControl != null)
            {
                userControl.SetTimeToString();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TrySetToDateTime();
            SetTimeToString();
        }

        private void TimeStrTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            SetTimeToString();
        }
        private void TimeStrTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime dummy;
            if (!TryParse(out dummy))
            {
                TimeStrTextBox.Background = Brushes.Pink;
            }
            else
            {
                TimeStrTextBox.Background = Brushes.White;
            }
        }

        private void TimeStrTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TrySetToDateTime();
                SetTimeToString();
            }
        }
    }
}
