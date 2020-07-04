using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

[HarmonyPatch(typeof(LoadoutConfigurator))]
[HarmonyPatch("Initialize")]
public class Patch
{
    [HarmonyPostfix]
    public static void Postfix(LoadoutConfigurator __instance)
    {
        Debug.Log("Doing postfix.");
        Traverse traverse = Traverse.Create(__instance);
        GameObject gameObject = PatcherHelper.nukeObject;
        if (gameObject)
        {
            GameObject toInject = GameObject.Instantiate<GameObject>(gameObject);
            toInject.name = "Nuke";
            // GameObject equipper = toInject.transform.GetChild(1).gameObject;
            GameObject edgeTransform = toInject.transform.GetChild(0).gameObject;
            Nuke nuke = gameObject.AddComponent<Nuke>();
            nuke.edgeTransform = edgeTransform;
            nuke.equipper = GameObject.Instantiate(new GameObject());
            nuke.equipper.name = "Nuke";
            Debug.Log("Nuke added to EqInfo in postfix.");
            nuke.init();
            nuke.missileLauncher.LoadAllMissiles();
            toInject.SetActive(false);
            Debug.Log("Start eq info");
            Dictionary<string, EqInfo> unlockedWeaponPrefabs = (Dictionary<string, EqInfo>)traverse.Field("unlockedWeaponPrefabs").GetValue();
            Debug.Log("Got eq info.");
            EqInfo eq = new EqInfo(nuke.equipper, Environment.CurrentDirectory + "\\VTOLVR_ModLoader\\mods\\Nuke\\nukey");
            Debug.Log("New eq");
            /*eq.eq = nuke.HPEquipper;
            Debug.Log("Eq.eq");
            eq.eqObject = nuke.HPEquipper.gameObject;
            Debug.Log("Eq.eqObject");*/
            unlockedWeaponPrefabs.Add("B61", eq); // Sadly can't prefix a method so shitty shit shit
            Debug.Log("Add to list.");
            traverse.Field("unlockedWeaponPrefabs").SetValue(unlockedWeaponPrefabs);
            Debug.Log("Postfix 200.");
        }
        else
        {
            Debug.LogError("Nuke not found in AssetBundle.");
        }
    }
}
[HarmonyPatch(typeof(LoadoutConfigurator))]
[HarmonyPatch("AttachImmediate")]
public class Patch0
{
    [HarmonyPrefix]
    public static bool Prefix(string weaponName, int hpIdx, LoadoutConfigurator __instance)
    {
        if (weaponName != "Nuke")
        {
            Debug.Log("Prefix 0 true");
            return true;
        }
        Debug.Log("Prefix 0 false.");
        Transform transform = PatcherHelper.nukeObject.transform;
        Debug.Log("TRANSFORM");
        __instance.equips[hpIdx] = transform.GetComponent<HPEquippable>();
        // Traverse traverse = Traverse.Create<LoadoutConfigurator>().Field("hpTransforms");
        Transform[] transforms = __instance.wm.hardpointTransforms;
        transform.parent = transforms[hpIdx];
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        // __instance.equips[hpIdx].OnConfigAttach(__instance);
        /*if (__instance.OnAttachHPIdx != null)
        {
            __instance.OnAttachHPIdx(hpIdx);
        }*/
        __instance.UpdateNodes();
        return false;
    }
}
// I am so sorry that I must dump a shit ton of code in this file
[HarmonyPatch(typeof(LoadoutConfigurator))]
[HarmonyPatch("AttachRoutine")]
public class Patch69
{
	public static bool Prefix(int hpIdx, string weaponName, LoadoutConfigurator __instance)
    {
        if (weaponName != "Nuke")
        {
            Debug.Log("Prefix 69 true");
            return true;
        }
        Debug.Log("Prefix 69 false");
        GameObject instantiated = PatcherHelper.nukeObject;
        instantiated.SetActive(true);
        Nuke nuke = instantiated.AddComponent<Nuke>();
        GameObject equipper = new GameObject();
        GameObject edgeTransform = instantiated.transform.GetChild(0).gameObject;
        nuke.edgeTransform = edgeTransform;
        nuke.equipper = equipper;
        equipper.SetActive(true);
        Debug.Log("Nuke added.");
        nuke.init();
        Debug.Log("PH instantiate");
        Transform weaponTf = equipper.transform;
        nuke.missileLauncher.LoadAllMissiles();
        Debug.Log("wTf transform");
        // Traverse hpTransformsTraverse = Traverse.Create<LoadoutConfigurator>().Field("hpTransforms");
        Debug.Log("traverse hpTransforms");
        Transform[] transforms = __instance.wm.hardpointTransforms;
        Debug.Log("get");
        Transform hpTf = transforms[hpIdx];
        Debug.Log("hpTf = transforms");
        InternalWeaponBay iwb = null;
        Debug.Log("null iwb");
        // Traverse iwbAttachTraverse = Traverse.Create<LoadoutConfigurator>().Field("iwbAttach");
        MethodInfo dynMethod = __instance.GetType().GetMethod("iwbAttach", BindingFlags.NonPublic | BindingFlags.Instance);
        Debug.Log("dyn method");
        // object iwbAttach = (InternalWeaponBay)dynMethod.Invoke(__instance, null);
        Debug.Log("iwb attach");
        // object iwbAttach = iwbAttachTraverse.GetValue(__instance);
        /*for (int i = 0; i < __instance.wm.internalWeaponBays.Length; i++)
        {
            InternalWeaponBay internalWeaponBay = __instance.wm.internalWeaponBays[i];
            if (internalWeaponBay.hardpointIdx == hpIdx)
            {
                iwb = internalWeaponBay;
                iwb.RegisterOpenReq(iwbAttach);
            }
        }*/
        Debug.Log("iterator");
        __instance.equips[hpIdx] = weaponTf.GetComponent<HPEquippable>();
        Debug.Log("equips weaponTf");
        // __instance.equips[hpIdx].OnConfigAttach(__instance);
        // Debug.Log("__instance,equips onconfigtattach");
        weaponTf.rotation = hpTf.rotation;
        Debug.Log("ROTATIOENATEAITJAOTATATETED");
        Vector3 localPos = new Vector3(0f, -4f, 0f);
        Debug.Log("create localPos");
        weaponTf.position = hpTf.TransformPoint(localPos);
        Debug.Log("Position");
        __instance.UpdateNodes();
        Debug.Log("Update Nodes");
        Vector3 tgt = new Vector3(0f, 0f, 0.5f);
        Debug.Log("tgt");
        if (hpIdx == 0 || iwb)
		{
			tgt = Vector3.zero;
        }
        Debug.Log("if vector3");
        while ((localPos - tgt).sqrMagnitude > 0.01f)
		{
			localPos = Vector3.Lerp(localPos, tgt, 5f * Time.deltaTime);
			weaponTf.position = hpTf.TransformPoint(localPos);
        }
        Debug.Log("while");
        weaponTf.parent = hpTf;
        Debug.Log("parent");
        weaponTf.localPosition = tgt;
        Debug.Log("localPosition = tgt");
        weaponTf.localRotation = Quaternion.identity;
        Debug.Log("Quaternion.identity");
        __instance.vehicleRb.AddForceAtPosition(Vector3.up * __instance.equipImpulse, __instance.wm.hardpointTransforms[hpIdx].position, ForceMode.Impulse);
        Debug.Log("ADD FORCE AT POSITION! POGGERS");
        /*Traverse hpAudioSourcesTraverse = Traverse.Create<LoadoutConfigurator>().Field("hpAudioSources");
        Debug.Log("hpAudioSourcesTraverse created!");
        AudioSource[] hpAudioSources = (AudioSource[])hpTransformsTraverse.GetValue(__instance);*/
        AudioSource audioSource = new GameObject("HPAudio").AddComponent<AudioSource>();
        audioSource.transform.parent = __instance.gameObject.transform;
        audioSource.transform.position = hpTf.position;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 4f;
        audioSource.maxDistance = 1000f;
        audioSource.dopplerLevel = 0f;
        Debug.Log("hpAudioSources 200");
        audioSource.PlayOneShot(__instance.attachAudioClip);
        Debug.Log("play 1 shot");
        __instance.attachPs.transform.position = hpTf.position;
        Debug.Log("Attach p's transform");
        __instance.attachPs.FireBurst();
        Debug.Log("FireBurst()");
        while (weaponTf.localPosition.sqrMagnitude > 0.001f)
		{
			weaponTf.localPosition = Vector3.MoveTowards(weaponTf.localPosition, Vector3.zero, 4f * Time.deltaTime);
        }
        Debug.Log("While 2 or 3 i dont remember or give a fuck");
        /*if (iwb)
		{
			iwb.UnregisterOpenReq(iwbAttach);
        }
        Debug.Log("iwb unregister");*/
        weaponTf.localPosition = Vector3.zero;
        Debug.Log("Vector3.zero");
        __instance.UpdateNodes();
        Debug.Log("UpdateNodes called, we're all done! Return false time;!!!!!!!!!");
        return false;
	}
}

public static class PatcherHelper
{
    private static AssetBundle assetBundle = AssetBundle.LoadFromFile(Environment.CurrentDirectory + "\\VTOLVR_ModLoader\\mods\\Nuke\\nukey");
    public static GameObject nukeObject = assetBundle.LoadAsset<GameObject>("Nuke");
    public static AssetBundle GetAssetBundle()
    {
        return assetBundle;
    }
}




/*
[HarmonyPatch(typeof(EqInfo))]
[HarmonyPatch("GetInstantiated")]
public class Patch0
{
    [HarmonyPrefix]
    public static bool Prefix(EqInfo __instance, ref GameObject __result)
    {
        if (__instance.prefabPath != Environment.CurrentDirectory + "\\VTOLVR_ModLoader\\mods\\Nuke\\nukey")
        { 
            Debug.Log("Prefix true");
            return true;
        }
        Debug.Log("Prefix false");
        GameObject gameObject = GameObject.Instantiate(AssetBundle.LoadFromFile(Environment.CurrentDirectory + "\\VTOLVR_ModLoader\\mods\\Nuke\\nukey").LoadAsset<GameObject>("Nuke"));
        GameObject equipper = gameObject.transform.GetChild(1).gameObject;
        GameObject edgeTransform = gameObject.transform.GetChild(0).gameObject;
        Nuke nuke = gameObject.AddComponent<Nuke>();
        nuke.edgeTransform = edgeTransform;
        nuke.equipper = equipper;
        Debug.Log("Nuke added to EqInfo in prefix.");
        nuke.init();
        equipper.SetActive(true);
        gameObject.name = __instance.eqObject.name;
        /*__instance.eq = nuke.HPEquipper;
        __instance.eqObject = gameObject;
        __instance.prefabPath = 
        gameObject.SetActive(true);
        __result = gameObject;
        Debug.Log("Prefix sucessful.");
        return false;
    }
}*/