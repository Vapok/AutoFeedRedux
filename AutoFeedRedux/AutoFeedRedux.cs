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
using Vapok.Common.Managers.Splash;
using Vapok.Common.Tools;

namespace AutoFeedRedux
{
    [BepInPlugin(_pluginId, _displayName, _version)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("com.ValheimModding.YamlDotNetDetector")]

    public class AutoFeedRedux : BaseUnityPlugin, IPluginInfo
    {
        private const string _pluginId = "vapok.mods.AutoFeedRedux";
        private const string _displayName = "AutoFeedRedux";
        private const string _version = "2.0.10";
        
        public string PluginId => _pluginId;
        public string DisplayName => _displayName;
        public string Version => _version;
        public BaseUnityPlugin Instance => _instance;
        public AutoFeeder AutoFeeder { get; set; }

        public static ILogIt Log => _log;
        public static bool ValheimAwake;
        public static Waiting Waiter;
        
        private static AutoFeedRedux _instance;
        private static ConfigSyncBase _config;
        private static ILogIt _log;
        private Harmony _harmony;
        
        [UsedImplicitly]
        private void Awake()
        {
            _instance = this;
            Waiter = new Waiting();
            
            Jotunn.Entities.CustomLocalization localization = Jotunn.Managers.LocalizationManager.Instance.GetLocalization();

            LogManager.Init(PluginId, out _log);
            Initializer.LoadManagers(localization);

            _config = new ConfigRegistry(_instance);

            Localizer.Waiter.StatusChanged += InitializeModule;
            
            _harmony = new Harmony(Info.Metadata.GUID);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            ModSplashManager.Register(new ModSplashDossier(_instance)
            {
                Tagline = "An automated feeding mod that keeps tamed creatures fed from nearby containers.",
                ShowOnStartup = ConfigRegistry.ShowSplashOnStartup,
            });
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
