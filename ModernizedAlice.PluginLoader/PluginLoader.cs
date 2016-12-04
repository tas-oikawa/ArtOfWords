using Editor4ArtOfWords.Model;
using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace ModernizedAlice.PluginLoader
{
    public class PluginLoader
    {
        private IEditor _editor;

        public PluginLoader()
        {
        }

        public IEditor GetEditor()
        {
            return _editor;
        }

        public void Load()
        {
            _editor = new AvalonEditorViewModel();
        }
    }
}
