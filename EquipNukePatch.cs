using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

[HarmonyPatch(typeof(PlayerVehicleSetup))]
[HarmonyPatch("SetupForFlight")]
public class Patch2
{
    [HarmonyPrefix]
    public static bool Prefix(PlayerVehicle __instance)
    {
        Loadout notStaticLoadout = new Loadout();
        notStaticLoadout.hpLoadout = VehicleEquipper.loadout.hpLoadout;
        notStaticLoadout.cmLoadout = VehicleEquipper.loadout.cmLoadout;
        notStaticLoadout.normalizedFuel = VehicleEquipper.loadout.normalizedFuel;
        LOL.sloadout = notStaticLoadout;
        Debug.Log("Before prefix loadout");
        foreach (var equip in VehicleEquipper.loadout.hpLoadout)
        {
            Debug.Log(equip);
        }
        Debug.Log("Before prefix sloadout (already newed the shit)");
        foreach (var equip in LOL.sloadout.hpLoadout)
        {
            Debug.Log(equip);
        }
        Debug.Log("Changing shit.");
        for (int i = 0; i < VehicleEquipper.loadout.hpLoadout.Length; i++)
        {
            if (VehicleEquipper.loadout.hpLoadout[i] == "")
            {
                VehicleEquipper.loadout.hpLoadout[i] = null;
            }
            if (VehicleEquipper.loadout.hpLoadout[i] == "Nuke")
            {
                VehicleEquipper.loadout.hpLoadout[i] = "";
            }
        };
        Debug.Log("After prefix loadout");
        foreach (var equip in VehicleEquipper.loadout.hpLoadout)
        {
            Debug.Log(equip);
        }
        Debug.Log("After prefix sloadout");
        foreach (var equip in LOL.sloadout.hpLoadout)
        {
            Debug.Log(equip);
        }
        return true;
    }
}

[HarmonyPatch(typeof(PlayerVehicleSetup))]
[HarmonyPatch("SetupForFlight")]
public class Patch1
{
    [HarmonyPostfix]
    public static void Postfix(PlayerVehicleSetup __instance)
    {
        Debug.Log("Before postfix loadout");
        foreach (var equip in LOL.sloadout.hpLoadout)
        {
            Debug.Log(equip);
        }
        Debug.Log("Changing shit.");
        // The SHITTIEST SOLUTION EVER
        for (int i = 0; i < LOL.sloadout.hpLoadout.Length; i++)
        {
            if (LOL.sloadout.hpLoadout[i] == "" && LOL.sloadout.hpLoadout[i] != "Nuke")
            {
                Debug.Log("Changing " + LOL.sloadout.hpLoadout[i]);
                LOL.sloadout.hpLoadout[i] = "Nuke";
            }
            Debug.Log(LOL.sloadout.hpLoadout[i]);
        }
        for (int i = 0; i < LOL.sloadout.hpLoadout.Length; i++)
        {
            if (LOL.sloadout.hpLoadout[i] != "Nuke" && LOL.sloadout.hpLoadout[i] != "" || LOL.sloadout.hpLoadout[i] != null)
            {
                if (LOL.sloadout.hpLoadout[i] != "Nuke")
                {
                    Debug.Log("2 Changing " + LOL.sloadout.hpLoadout[i]);
                    LOL.sloadout.hpLoadout[i] = "";
                }
            }
            Debug.Log(LOL.sloadout.hpLoadout[i]);
        }
        Debug.Log("After postfix loadout");
        foreach (var equip in LOL.sloadout.hpLoadout)
        {
            Debug.Log(equip);
        }
    }
}
