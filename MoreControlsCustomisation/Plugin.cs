﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace MoreControlsCustomisation
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static Plugin _instance;
        public static Plugin Instance => _instance ??= FindObjectOfType<Plugin>();
        
        internal new static ManualLogSource Logger;

        public ConfigEntry<bool> IsInvertYAxis { get; private set; }

        public ConfigEntry<bool> IsInvertScrollDirection { get; private set; }

        private Harmony _harmony;
        private bool _isPatched;
        
        private void Awake()
        {
            // Set instance
            _instance = this;
            
            // Init logger
            Logger = base.Logger;
            
            // Read config
            IsInvertYAxis = Config.Bind("General", "InvertYAxis", false, "Invert Y axis");
            Logger.LogInfo($"Invert Y axis loaded from config: {IsInvertYAxis.Value}");
            
            IsInvertScrollDirection = Config.Bind("General", "InvertScrollDirection", false, "Invert scroll direction");
            Logger.LogInfo($"Invert scroll direction loaded from config: {IsInvertScrollDirection.Value}");
            
            // Patch using Harmony
            PatchAll();
            
            // Report plugin is loaded
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        public void PatchAll()
        {
            if (_isPatched)
            {
                Logger.LogWarning("Already patched!");
                return;
            }
            
            Logger.LogDebug("Patching...");

            _harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            _harmony.PatchAll();
            _isPatched = true;
            
            Logger.LogDebug("Patched!");
        }
        
        public void UnpatchAll()
        {
            if (!_isPatched)
            {
                Logger.LogWarning("Not patched!");
                return;
            }
            
            Logger.LogDebug("Unpatching...");
            
            _harmony.UnpatchSelf();
            _isPatched = false;
            
            Logger.LogDebug("Unpatched!");
        }
    }
}