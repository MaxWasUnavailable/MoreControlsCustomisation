using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreControlsCustomisation.Patches;

/// <summary>
///     Harmony patches for the <c>PlayerControllerB</c> class.
/// </summary>
[HarmonyPatch(typeof(PlayerControllerB))]
public class PatchInitializeGame
{
    /// <summary>
    ///     Patch the <c>PlayerControllerB</c> CalculateSmoothLookingInput method.
    /// </summary>
    /// <param name="inputVector"><c>Vector2</c> instance with the input vector.</param>
    [HarmonyPatch("CalculateSmoothLookingInput")]
    [HarmonyPrefix]
    public static void InvertControlsCalculateSmoothLookingInput(ref Vector2 inputVector)
    {
        inputVector.y *= Plugin.Instance.IsInvertYAxis.Value ? -1 : 1;
    }

    /// <summary>
    ///     Patch the <c>PlayerControllerB</c> CalculateNormalLookingInput method.
    /// </summary>
    /// <param name="inputVector"><c>Vector2</c> instance with the input vector.</param>
    [HarmonyPatch("CalculateNormalLookingInput")]
    [HarmonyPrefix]
    public static void InvertControlsCalculateNormalLookingInput(ref Vector2 inputVector)
    {
        inputVector.y *= Plugin.Instance.IsInvertYAxis.Value ? -1 : 1;
    }
}