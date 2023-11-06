using System.Collections.Generic;
using System.Reflection.Emit;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace MoreControlsCustomisation.Patches;

public static class PatchPlayerControllerBHelpers
{
    public static double ConditionalInvertScrollAmount(double scrollAmount)
    {
        return Plugin.Instance.IsInvertScrollDirection.Value ? -scrollAmount : scrollAmount;
    }
}

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

    /// <summary>
    ///     Patch the <c>PlayerControllerB</c> SwitchItem_performed method.
    /// </summary>
    /// <param name="instructions">An <c>IEnumerable</c> of <c>CodeInstruction</c> instances.</param>
    [HarmonyPatch("SwitchItem_performed")]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> InvertControlsSwitchItemPerformed(
        IEnumerable<CodeInstruction> instructions)
    {
        var callVirtCount = 0;
        var writeAfterX = -1;

        foreach (var instruction in instructions)
        {
            if (writeAfterX >= 0)
            {
                if (writeAfterX == 0)
                {
                    // We have (double) context.ReadValue<float>() in the stack as following instructions:
                    //  IL_00c9: ldarga.s     context (might actually be 1 instead of context)
                    //  IL_00cb: call         instance !!0/*float32*/ [Unity.InputSystem]UnityEngine.InputSystem.InputAction/CallbackContext::ReadValue<float32>()

                    // Next, we use it in the original if statement: if (ConditionalInvertScrollAmount(value on stack) > 0.0)
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(PatchPlayerControllerBHelpers), nameof(PatchPlayerControllerBHelpers.ConditionalInvertScrollAmount)));
                };
                writeAfterX--;
            }

            if (instruction.opcode == OpCodes.Callvirt && callVirtCount < 4)
            {
                callVirtCount++;
                if (callVirtCount == 4) writeAfterX = 2;
            }
            
            yield return instruction;
        }
    }
}