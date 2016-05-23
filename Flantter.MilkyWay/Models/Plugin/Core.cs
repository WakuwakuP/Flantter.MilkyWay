﻿using Flantter.MilkyWay.Plugin;
using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Flantter.MilkyWay.Models.Plugin
{
    public class Core
    {
        private static Core _Instance = new Core();

        public static Core Instance
        {
            get { return _Instance; }
        }

        private Core()
        {
            _Plugins = new Dictionary<string, Plugin>();
        }

        private Dictionary<string, Plugin> _Plugins;

        public async Task Initialize()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Plugin system initializing...");
#endif

            var folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();
            StorageFolder pluginFolder = null;
            if (!folders.Any(x => x.Name == "plugins"))
                pluginFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("plugins");
            else
                pluginFolder = folders.First(x => x.Name == "plugins");

            foreach (var file in await pluginFolder.GetFilesAsync())
            {
                if (file.FileType != ".js")
                    continue;

                var name = file.DisplayName;
                var script = System.IO.File.ReadAllText(file.Path);
                var engine = new Engine(clr => clr
                    .AllowClr()
                    .AllowClr(typeof(Debug).GetTypeInfo().Assembly));

                _Plugins[name] = new Plugin() { Engine = engine };

                engine.SetValue("registerPlugin", new Action<string, string, string>((pname, description, version) =>
                {
                    if (!_Plugins.ContainsKey(pname))
                        return;

                    _Plugins[pname].Name = pname;
                    _Plugins[pname].Description = description;
                    _Plugins[pname].Version = version;

                    try
                    {
                        _Plugins[pname].Engine.Invoke("load");
                    }
                    catch
                    {
                    }
                }));

                engine.Execute(script);
            }
        }
    }

    public class Plugin
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Version { get; set; }

        public Engine Engine { get; set; }
    }
}
