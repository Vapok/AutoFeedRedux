using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using Vapok.Common.Abstractions;
using Vapok.Common.Managers.Configuration;
using Vapok.Common.Shared;

namespace AutoFeedRedux.Configuration
{
    public class ConfigRegistry : ConfigSyncBase
    {
        internal static ConfigEntry<bool> Enabled;
        internal static ConfigEntry<float> FeedRange;
        internal static ConfigEntry<bool> ProtectContainers;
        internal static ConfigEntry<bool> RequireMove;
        internal static ConfigEntry<float> MoveProximity;
        internal static ConfigEntry<string> DisallowFeed;
        internal static ConfigEntry<string> DisallowAnimal;

        public static HashSet<string> DisallowedAnimals { get; private set; } = new(StringComparer.OrdinalIgnoreCase);
        public static HashSet<string> DisallowedFoods { get; private set; } = new(StringComparer.OrdinalIgnoreCase);

        public static Waiting Waiter;

        public ConfigRegistry(IPluginInfo mod) : base(mod)
        {
            Waiter = new Waiting();

            InitializeConfigurationSettings();
        }

        public sealed override void InitializeConfigurationSettings()
        {
            if (_config == null)
                return;

            SyncedConfig("Synced Settings", "Enable Auto Feeder", true,
                new ConfigDescription("If true, will automatically feed tameables from nearby containers, if food is available.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 1 }), ref Enabled);

            SyncedConfig("Synced Settings", "Feed Range in Meters", 30f,
                new ConfigDescription("Range container must be from tameable to feed from it.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 2 }), ref FeedRange);

            SyncedConfig("Synced Settings", "Require Move to Feed", true,
                new ConfigDescription("If true, require tameable to move to container to feed.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 3 }), ref RequireMove);

            SyncedConfig("Synced Settings", "Move Proximity", 2.5f,
                new ConfigDescription("If move is required, distance from container before feeding.",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 3 }), ref MoveProximity);

            SyncedConfig("Synced Settings", "Disallow Feed", "",
                new ConfigDescription("Types of feed to not auto feed. Comma-separated",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 3 }), ref DisallowFeed);

            SyncedConfig("Synced Settings", "Disallow Animal", "",
                new ConfigDescription("Types of animals to not auto feed. Comma-separated",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 3 }), ref DisallowAnimal);

            SyncedConfig("Synced Settings", "Protect Feed Containers", true,
                new ConfigDescription("If true, will prevent creatures from damaging containers identified as feed containers",
                    null, 
                    new ConfigurationManagerAttributes { Category = "Synced Settings", Order = 2 }), ref ProtectContainers);



            UpdateDisallowedAnimals();
            UpdateDisallowedFoods();

            if (DisallowAnimal != null)
                DisallowAnimal.SettingChanged += delegate { UpdateDisallowedAnimals(); };
            if (DisallowFeed != null)
                DisallowFeed.SettingChanged += delegate { UpdateDisallowedFoods(); };
        }

        private static void UpdateDisallowedAnimals()
        {
            HashSet<string> set = new(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(DisallowAnimal?.Value))
            {
                string[] parts = DisallowAnimal.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    string trimmed = part.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        set.Add(trimmed);
                }
            }
            DisallowedAnimals = set;
        }

        private static void UpdateDisallowedFoods()
        {
            HashSet<string> set = new(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(DisallowFeed?.Value))
            {
                string[] parts = DisallowFeed.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    string trimmed = part.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        set.Add(trimmed);
                }
            }
            DisallowedFoods = set;
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