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
        private CompositionContainer _container;
        private string _pluginPath;

        public PluginLoader()
        {
            var assembly = Assembly.GetEntryAssembly();
            var dirPath = Path.GetDirectoryName(assembly.Location);

            _pluginPath = dirPath += "\\Plugin";
        }

        public PluginLoader(string path)
        {
            _pluginPath = path;
        }

        public CompositionContainer GetContainer()
        {
            return _container;
        }

        public void Load()
        {
            // 現在のアセンブリのカタログ
            var assm = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            // Extensionsフォルダにあるアセンブリのカタログ
            var extensions = new DirectoryCatalog(_pluginPath);
            // 2つのカタログを束ねる
            var agg = new AggregateCatalog(assm, extensions);
            // カタログをもとにコンテナを作る
            _container = new CompositionContainer(agg);
        }
    }
}
