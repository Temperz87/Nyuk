using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

class Loadouting : VTOLMOD
{
    private void Start()
    {
        HarmonyInstance instance = HarmonyInstance.Create("TEMPY.NUKE.B61");
        instance.PatchAll(Assembly.GetExecutingAssembly());
    }
}

[HarmonyPatch(typeof(LoadoutConfigurator))]
[HarmonyPatch("Initialize")]
public class Patch
{
    [HarmonyPostfix]
    public static void Postfix(LoadoutConfigurator __instance)
    {
        Traverse traverse = Traverse.Create(typeof(VTResources));
        Dictionary<string, WingmanVoiceProfile> stockWingmen = (Dictionary<string, WingmanVoiceProfile>)traverse.Field("wingmanVoices").GetValue();

    }
}