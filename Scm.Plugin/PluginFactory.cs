using Com.Scm.Utils;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Com.Scm.Plugin
{
    public class PluginFactory
    {
        private static List<Manifest> _Plugins = new List<Manifest>();

        public static void LoadPlugin(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            var dirs = Directory.GetDirectories(path);
            if (dirs.Length < 1)
            {
                return;
            }

            foreach (var dir in dirs)
            {
                var file = Path.Combine(dir, "manifest.json");
                if (!File.Exists(file))
                {
                    continue;
                }

                var json = File.ReadAllText(file);
                var manifest = json.AsJsonObject<Manifest>();
                if (manifest == null)
                {
                    continue;
                }

                manifest.Parse();

                if (!manifest.sys)
                {
                    var asmFile = Path.Combine(dir, manifest.dll);
                    if (!File.Exists(asmFile))
                    {
                        continue;
                    }
                }

                manifest.dir = dir;
                _Plugins.Add(manifest);

                //// 利用反射,构造DLL文件的实例
                //Assembly asm = Assembly.LoadFile(asmFile);
                //object obj = Activator.CreateInstance(asmFile, manifest.type);
                //if (obj == null)
                //{
                //    continue;
                //}
            }
        }

        public static T GetPlugin<T>(string name)
        {
            var type = typeof(T);

            var plugin = GetManifest(type.Name, name);
            if (plugin == null)
            {
                return default(T);
            }

            var assembly = plugin.assembly;
            if (assembly == null)
            {
                var path = Path.Combine(plugin.dir, plugin.dll);
                assembly = Assembly.LoadFile(path);
                plugin.assembly = assembly;
            }

            if (!plugin.singleton)
            {
                var obj1 = assembly.CreateInstance(plugin.uri);
                return (T)obj1;
            }

            if (plugin.instance != null)
            {
                return (T)plugin.instance;
            }

            var obj2 = assembly.CreateInstance(plugin.uri);
            plugin.instance = obj2;

            return (T)obj2;
        }

        public static T GetPluginByUri<T>(string uri)
        {
            var type = typeof(T);

            var plugin = GetManifest(type.Name, uri);
            if (plugin == null)
            {
                return default(T);
            }

            var assembly = plugin.assembly;
            if (assembly == null)
            {
                var path = Path.Combine(plugin.dir, plugin.dll);
                assembly = Assembly.LoadFile(path);
                plugin.assembly = assembly;
            }

            if (!plugin.singleton)
            {
                var obj1 = assembly.CreateInstance(plugin.entry);
                return (T)obj1;
            }

            if (plugin.instance != null)
            {
                return (T)plugin.instance;
            }

            var obj2 = assembly.CreateInstance(plugin.entry);
            plugin.instance = obj2;

            return (T)obj2;
        }

        private static Manifest GetManifest(string type, string name)
        {
            type = type.ToLower();
            foreach (var plugin in _Plugins)
            {
                if (plugin.type == type && plugin.name == name)
                {
                    return plugin;
                }
            }
            return null;
        }
    }
}
