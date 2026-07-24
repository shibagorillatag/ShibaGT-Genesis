using HarmonyLib;
using UnityEngine;
using Viveport;

[HarmonyPatch(typeof(LegalAgreements))]
[HarmonyPatch("Update", MethodType.Normal)]
internal class TOSPatch : MonoBehaviour
{
    public static bool turnedthefuckon;

    private static bool Prefix(LegalAgreements __instance)
    {
        if (turnedthefuckon)
        {
            Traverse.Create(__instance).Field("controllerBehaviour").Field("buttonDown").SetValue(true);
            Traverse.Create(__instance).Field("scrollTime").SetValue(0.1f);
            return false;
        }
        return true;
    }
}