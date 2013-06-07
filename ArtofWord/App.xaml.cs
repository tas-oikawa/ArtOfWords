using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ArtOfWords
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public SplashScreen screen;
        private static readonly log4net.ILog logger　= log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void Application_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Fatal("Error: InnerException" + e.Exception.InnerException + "\r\nTrace: " + e.Exception.StackTrace);
            ShowDialogManager.ShowMessageBox(e.Exception.ToString(), "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Select the text in a TextBox when it receives focus.
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(SelectivelyIgnoreMouseButton));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotKeyboardFocusEvent,
                new RoutedEventHandler(SelectAllText));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.MouseDoubleClickEvent,
                new RoutedEventHandler(SelectAllText));
            base.OnStartup(e);
        }

        void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox)parent;

                if (textBox.TextWrapping != TextWrapping.NoWrap)
                {
                    return;
                }

                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focused, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                if (textBox.TextWrapping != TextWrapping.NoWrap)
                {
                    return;
                }
                textBox.SelectAll();
            }
        }

        private void Application_Startup_1(object sender, StartupEventArgs e)
        {
            screen = new SplashScreen("Images/splashScreen.png");
            screen.Show(true, true);
        }
    }
}
