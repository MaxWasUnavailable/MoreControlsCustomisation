using HarmonyLib;
using UnityEngine.InputSystem;

namespace MoreControlsCustomisation.Patches;

/// <summary>
///     Harmony patches for the <c>InitializeGame</c> class.
/// </summary>
[HarmonyPatch(typeof(InitializeGame))]
public class PatchInitializeGame
{
    /// <summary>
    ///     Patch the <c>InitializeGame</c> Awake method.
    /// </summary>
    /// <param name="__instance"><c>InitializeGame</c> instance</param>
    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    public static void InvertControls(InitializeGame __instance)
    {
        Plugin.Logger.LogInfo("Patching InitializeGame...");

        var playerActions = __instance.playerActions;

        var m_Movement_Look =
            Traverse.Create(playerActions).Field<InputAction>("m_Movement_Look"); // Movement/Look[/Mouse/delta]
        var m_Movement_SwitchItem =
            Traverse.Create(playerActions)
                .Field<InputAction>("m_Movement_SwitchItem"); // Movement/SwitchItem[/Mouse/scroll/y]
        var applyBindingOverrideMethod =
            typeof(InputAction).GetMethod("ApplyBindingOverride", new[] { typeof(InputBinding) });

        Plugin.Logger.LogDebug("Applying binding overrides...");

        // If Y axis inverted, apply binding override
        if (Plugin.Instance.IsInvertYAxis.Value)
        {
            Plugin.Logger.LogDebug("Inverting Y axis...");

            applyBindingOverrideMethod.Invoke(m_Movement_Look,
                new object[] { new InputBinding { overrideProcessors = "invertVector2(invertX=false,invertY=true)" } });

            Plugin.Logger.LogDebug("Y axis inverted!");
        }

        // If scroll direction inverted, apply binding override
        if (Plugin.Instance.IsInvertScrollDirection.Value)
            applyBindingOverrideMethod.Invoke(m_Movement_SwitchItem,
                new object[] { new InputBinding { overrideProcessors = "invertVector2(invertX=false,invertY=true)" } });

        Plugin.Logger.LogInfo("Patch applied!");
    }
}