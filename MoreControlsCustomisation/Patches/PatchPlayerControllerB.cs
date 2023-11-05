using System.Collections.Generic;
using System.Reflection.Emit;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreControlsCustomisation.Patches;

// public static class PatchPlayerControllerBHelpers
// {
//     public static double ConditionalInvertScrollAmount(double scrollAmount)
//     {
//         Plugin.Logger.LogInfo($"Scroll amount: {scrollAmount}");
//         if (!Plugin.Instance.IsInvertScrollDirection.Value) return scrollAmount;
//
//         scrollAmount *= -1;
//         Plugin.Logger.LogInfo($"Inverted scroll amount: {scrollAmount}");
//
//
//         return scrollAmount;
//     }
// }

/// <summary>
///     Harmony patches for the <c>PlayerControllerB</c> class.
/// </summary>
[HarmonyPatch(typeof(PlayerControllerB))]
public class PatchPlayerControllerB
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

    // /// <summary>
    // ///     Patch the <c>PlayerControllerB</c> SwitchItem_performed method.
    // /// </summary>
    // /// <param name="instructions">An <c>IEnumerable</c> of <c>CodeInstruction</c> instances.</param>
    // [HarmonyPatch("SwitchItem_performed")]
    // [HarmonyTranspiler]
    // public static IEnumerable<CodeInstruction> InvertControlsSwitchItemPerformed(
    //     IEnumerable<CodeInstruction> instructions)
    // {
    //     var callVirtCount = 0;
    //     var writeNewCode = false;
    //     var skipNextX = 0;
    //
    //     foreach (var instruction in instructions)
    //     {
    //         if (writeNewCode)
    //         {
    //             // First we store the value of the scroll wheel in a local variable
    //             yield return new CodeInstruction(OpCodes.Ldarga_S, 1);
    //             yield return new CodeInstruction(OpCodes.Call,
    //                 AccessTools.Method(typeof(InputAction.CallbackContext),
    //                     nameof(InputAction.CallbackContext.ReadValue)));
    //             // yield return new CodeInstruction(OpCodes.Stloc_S, "scrollWheelValue");
    //
    //             // Next line is this: if ((double) context.ReadValue<float>() > 0.0)
    //             // We need to replace this with: if (ConditionalInvertScrollAmount(scrollWheelValue) > 0.0)
    //             // yield return new CodeInstruction(OpCodes.Ldloc_S, "scrollWheelValue");
    //             // yield return new CodeInstruction(OpCodes.Call,
    //             //     AccessTools.Method(typeof(PatchPlayerControllerBHelpers), nameof(PatchPlayerControllerBHelpers.ConditionalInvertScrollAmount)));
    //
    //             // We leave the > 0.0 and branch instructions as they are, skipping just the initial two instructions that load context and call ReadValue<float>()
    //
    //             writeNewCode = false;
    //             skipNextX = 2;
    //         }
    //
    //         if (instruction.opcode == OpCodes.Callvirt && callVirtCount < 4)
    //         {
    //             callVirtCount++;
    //             if (callVirtCount == 4) writeNewCode = true;
    //         }
    //
    //         if (skipNextX > 0)
    //         {
    //             skipNextX--;
    //             continue;
    //         }
    //
    //         yield return instruction;
    //     }
    // }
}