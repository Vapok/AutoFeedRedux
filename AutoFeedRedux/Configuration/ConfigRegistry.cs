using System;
using BepInEx.Configuration;
using Vapok.Common.Abstractions;
using Vapok.Common.Managers.Configuration;
using Vapok.Common.Shared;

namespace AutoFeedRedux.Configuration
{
    public class ConfigRegistry : ConfigSyncBase
    {
        //Configuration Entry Privates
        internal static ConfigEntry<bool> Enabled;
        internal static ConfigEntry<float> FeedRange;
        internal static ConfigEntry<bool> ProtectContainers;
        internal static ConfigEntry<bool> RequireMove;
        internal static ConfigEntry<float> MoveProximity;
        internal static ConfigEntry<string> DisallowFeed;
        internal static ConfigEntry<string> DisallowAnimal;
        
        public static Waiting Waiter;

        public ConfigRegistry(IPluginInfo mod): base(mod)
        {
            //Waiting For Startup
            Waiter = new Waiting();

            InitializeConfigurationSettings();
        }

        public sealed override void InitializeConfigurationSettings()
        {
            if (_config == null)
                return;
            
            //User Configs
            SyncedConfig("Synced Settings", "Enable Auto Feeder", false,
                new ConfigDescription("If true, will automatically feed tameables from nearby containers, if food is available.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 1 }),ref Enabled);

            SyncedConfig("Synced Settings", "Feed Range in Meters", 10f,
                new ConfigDescription("Range container must be from tameable to feed from it.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 2 }),ref FeedRange);

            SyncedConfig("Synced Settings", "Require Move to Feed", true,
                new ConfigDescription("If true, require tameable to move to container to feed.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 3 }),ref RequireMove);

            SyncedConfig("Synced Settings", "Move Proximity", 5f,
                new ConfigDescription("If move is required, distance from container before feeding.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 3 }),ref MoveProximity);

            SyncedConfig("Synced Settings", "Disallow Feed", "",
                new ConfigDescription("Types of feed to not auto feed. Comma-separated",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 3 }),ref DisallowFeed);

            SyncedConfig("Synced Settings", "Disallow Animal", "",
                new ConfigDescription("Types of animals to not auto feed. Comma-separated",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 3 }),ref DisallowAnimal);

            SyncedConfig("Synced Settings", "Protect Feed Containers", true,
                new ConfigDescription("If true, will prevent creatures from damaging containers identified as feed containers",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 2 }),ref ProtectContainers);
        }
    }
    
    public class Waiting
    {
        public void ConfigurationComplete(bool configDone)
        {
            if (configDone)
                StatusChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler StatusChanged;            
    }

}