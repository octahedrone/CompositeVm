using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sandbox.Tests
{
    public static class TestWindowLauncher
    {
        public static void ShowModal(this object dataContext)
        {
            ShowModal(dataContext, null);
        }

        public static void ShowModal(this object dataContext, ResourceDictionary dictionary)
        {
            var window = new Window { Content = dataContext };

            if (dictionary != null)
                window.Resources.MergedDictionaries.Add(dictionary);

            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.InvalidateVisual();

            window.ShowDialog();
            window.Dispatcher.InvokeShutdown();
        }

        public static void ShowModal(this Control content)
        {
            ShowModal(content, null);
        }

        public static void ShowModal(this Control content, ResourceDictionary dictionary)
        {
            var window = content as Window;

            if (window == null)
            {
                window = new Window {Content = content};
            }

            if (dictionary != null)
                window.Resources.MergedDictionaries.Add(dictionary);

            window.InvalidateVisual();

            window.ShowDialog();
            window.Dispatcher.InvokeShutdown();
        }
    }
}