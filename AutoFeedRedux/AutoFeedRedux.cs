/* AutoFeedRedux by Vapok */
using System;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using JetBrains.Annotations;
using AutoFeedRedux.Components;
using AutoFeedRedux.Configuration;
using Vapok.Common.Abstractions;
using Vapok.Common.Managers;
using Vapok.Common.Managers.Configuration;
using Vapok.Common.Managers.LocalizationManager;
using Vapok.Common.Tools;

namespace AutoFeedRedux
{
    [BepInPlugin(_pluginId, _displayName, _version)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("com.ValheimModding.YamlDotNetDetector")]

    public class AutoFeedRedux : BaseUnityPlugin, IPluginInfo
    {
        //Module Constants
        private const string _pluginId = "vapok.mods.AutoFeedRedux";
        private const string _displayName = "AutoFeedRedux";
        private const string _version = "1.1.4";
        
        //Interface Properties
        public string PluginId => _pluginId;
        public string DisplayName => _displayName;
        public string Version => _version;
        public BaseUnityPlugin Instance => _instance;
        public AutoFeeder AutoFeeder { get; set; }

        
        //Class Properties
        public static ILogIt Log => _log;
        public static bool ValheimAwake;
        public static Waiting Waiter;
        //public static AutoFeedRedux Main => _instance;
        
        //Class Privates
        private static AutoFeedRedux _instance;
        private static ConfigSyncBase _config;
        private static ILogIt _log;
        private Harmony _harmony;
        
        [UsedImplicitly]
        // This the main function of the mod. BepInEx will call this.
        private void Awake()
        {
            //I'm awake!
            _instance = this;
            
            //Waiting For Startup
            Waiter = new Waiting();
            
            //Jotunn Localization
            var localization = Jotunn.Managers.LocalizationManager.Instance.GetLocalization();

            //Register Logger
            LogManager.Init(PluginId,out _log);
            
            //Initialize Managers
            Initializer.LoadManagers(localization);

            //Register Configuration Settings
            _config = new ConfigRegistry(_instance);

            Localizer.Waiter.StatusChanged += InitializeModule;
            
            //Patch Harmony
            _harmony = new Harmony(Info.Metadata.GUID);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            //???

            //Profit
        }

        public void InitializeModule(object send, EventArgs args)
        {
            if (ValheimAwake)
                return;
            
            ConfigRegistry.Waiter.ConfigurationComplete(true);

            ValheimAwake = true;
        }
        
        private void OnDestroy()
        {
            _instance = null;
        }

        public class Waiting
        {
            public void ValheimIsAwake(bool awakeFlag)
            {
                if (awakeFlag)
                    StatusChanged?.Invoke(this, EventArgs.Empty);
            }
            public event EventHandler StatusChanged;            
        }
    }
}