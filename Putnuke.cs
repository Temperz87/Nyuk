using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;
class LOL : VTOLMOD
{
    private string resourcePath = Environment.CurrentDirectory + "\\VTOLVR_ModLoader\\mods\\Nuke\\nukey";
    static bool triggered = false;
    private IEnumerator main()
    {
        while (VTMapManager.fetch == null || !VTMapManager.fetch.scenarioReady || FlightSceneManager.instance.switchingScene)
        {
            yield return null;
        }
        WeaponManager wm = VTOLAPI.GetPlayersVehicleGameObject().GetComponent<WeaponManager>();
        wma = wm;
        // wm.resourcePath = Environment.CurrentDirectory + "\\VTOLVR_ModLoader\\mods\\AEAT\\nukey";
        VTOLAPI.GetPlayersVehicleGameObject().AddComponent<BOOM>();
        Loadout cloadout = new Loadout();
        cloadout.normalizedFuel = 0.65f;
        cloadout.hpLoadout = new string[]
        {
            null,
            null,
            "Nuke",
        };
        cloadout.cmLoadout = new int[]
        {
            1000,
            1000
        };
        Loadout loadout = new Loadout();
        loadout.normalizedFuel = 0.65f;
        loadout.hpLoadout = new string[]
        {
            "fa26_gun"
        };

        if (!triggered)
        {
            // wm.EquipWeapons(loadout);
            EquipCustomWeapons(sloadout, wm);
        }
        Debug.Log("did hax0r");
    }
    public void EquipCustomWeapons(Loadout loadout, WeaponManager wm)
    {
        triggered = true;
        Traverse traverse = Traverse.Create(wm);
        HPEquippable[] equips = (HPEquippable[])traverse.Field("equips").GetValue();
        MassUpdater component = wm.vesselRB.GetComponent<MassUpdater>();
        string[] hpLoadout = loadout.hpLoadout;
        int num = 0;
        Debug.Log("sLoadout\n" + loadout.hpLoadout.ToString());
        // var bundle = AssetBundle.LoadFromFile(resourcePath);
        while (num < wm.hardpointTransforms.Length && num < hpLoadout.Length)
        {
            if (!string.IsNullOrEmpty(hpLoadout[num]) && hpLoadout[num] == "Nuke")
            {
                Debug.Log(hpLoadout[num] + " will be tried to be loaded.");
                GameObject missileObject = PatcherHelper.GetAssetBundle().LoadAsset<GameObject>(hpLoadout[num]);
                Debug.Log("Got missileObject");
                //GameObject missileObject = Instantiate(@object, wm.hardpointTransforms[num]);
                // toCheckAgainst = wm.hardpointTransforms[num];
                Debug.Log("Instantiated custom weapon.");
                missileObject.name = hpLoadout[num];
                Debug.Log("Changed name.");
                missileObject.transform.localRotation = Quaternion.identity;
                Debug.Log("Quaternion identity.");
                missileObject.transform.localPosition = Vector3.zero;
                Debug.Log("local position.");
                missileObject.transform.localScale = new Vector3(20f, 20f, 20f);
                Debug.Log("local scale.");
                if (missileObject == null)
                {
                    Debug.LogError("missileObject is null.");
                }
                // missileLauncher.SetParentRigidbody(rB);
                GameObject equipper = Instantiate(new GameObject());
                GameObject edgeTransform = missileObject.transform.GetChild(0).gameObject;
                Nuke nuke = missileObject.AddComponent<Nuke>();
                nuke.edgeTransform = edgeTransform;
                nuke.equipper = equipper;
                equipper.SetActive(true);
                Debug.Log("Nuke added.");
                nuke.init();
                Debug.Log("Nuke inited.");
                if (equipper == null)
                {
                    Debug.LogError("Equipper is null.");
                }
                if (equipper.transform.position == null)
                {
                    Debug.LogError("Equipper transform posiiton null");
                }
                if (wm.hardpointTransforms[num].position == null)
                {
                    Debug.LogError("wm hardopint transforms position null");
                }
                equipper.transform.position = wm.hardpointTransforms[num].position;
                Debug.Log("Equipper transform.");
                equipper.transform.parent = wm.hardpointTransforms[num];
                Debug.Log("Equipper transform parent.");
                HPEquipBombRack HPEquipper = nuke.HPEquipper;
                Debug.Log("HPEquipper inited.");
                HPEquipper.SetWeaponManager(wm);
                Debug.Log("Weapon Manager.");
                equips[num] = HPEquipper;
                Debug.Log("equips = component.");
                HPEquipper.wasPurchased = true;
                Debug.Log("was purchased.");
                HPEquipper.hardpointIdx = num;
                Debug.Log("hardpointIDX.");
                HPEquipper.Equip();
                Debug.Log("Equip().");
                Debug.Log("Tipping nuke");
                if (HPEquipper.jettisonable)
                {
                    Rigidbody component3 = HPEquipper.GetComponent<Rigidbody>();
                    if (component3)
                    {
                        component3.interpolation = RigidbodyInterpolation.None;
                    }
                }
                Debug.Log("jettisonable.");
                if (HPEquipper.armable)
                {
                    HPEquipper.armed = true;
                    wm.RefreshWeapon();
                }
                Debug.Log("RefrshWeapon().");
                missileObject.SetActive(true);
                foreach (Component component4 in HPEquipper.gameObject.GetComponentsInChildren<Component>())
                {
                    if (component4 is IParentRBDependent)
                    {
                        ((IParentRBDependent)component4).SetParentRigidbody(wm.vesselRB);
                    }
                    if (component4 is IRequiresLockingRadar)
                    {
                        ((IRequiresLockingRadar)component4).SetLockingRadar(wm.lockingRadar);
                    }
                    if (component4 is IRequiresOpticalTargeter)
                    {
                        ((IRequiresOpticalTargeter)component4).SetOpticalTargeter(wm.opticalTargeter);
                    }
                }
                Debug.Log("DLZ shit");
                MissileLauncher missileLauncher = nuke.missileLauncher;
                if (missileLauncher.missilePrefab == null)
                {
                    Debug.LogError("MissilePrefab is null");
                }
                if (missileLauncher.missilePrefab.GetComponent<Missile>() == null)
                {
                    Debug.LogError("Missile not found on prefab");
                }
                if (missileLauncher.hardpoints[0] == null)
                {
                    Debug.LogError("Hardpoints null");
                }
                missileLauncher.LoadAllMissiles();
                Debug.Log(missileLauncher.missiles[0]);
                Debug.Log("HEY IT WORKED");
                Warhead nyuk = missileLauncher.missiles[0].gameObject.AddComponent<Warhead>();
                missileLauncher.missiles[0].OnDetonate = new UnityEvent();
                missileLauncher.missiles[0].OnDetonate.AddListener(new UnityAction(() =>
                {
                    Debug.Log("Nuke is now critical.");
                    nyuk.DoNuke();
                }));
                missileLauncher.missiles[0].enabled = true;
                if (missileLauncher.missiles[0].transform == null)
                {
                    Debug.LogError("Missile[0] null");
                }
                Debug.Log("Nuke should now have teeth");
                if (missileLauncher.overrideDecoupleSpeed > 0f)
                {
                    missileLauncher.missiles[0].decoupleSpeed = missileLauncher.overrideDecoupleSpeed;
                }
                if (missileLauncher.overrideDecoupleDirections != null && missileLauncher.overrideDecoupleDirections.Length > 0 && missileLauncher.overrideDecoupleDirections[0] != null)
                {
                    missileLauncher.missiles[0].overrideDecoupleDirTf = missileLauncher.overrideDecoupleDirections[0];
                }
                if (missileLauncher.overrideDropTime >= 0f)
                {
                    missileLauncher.missiles[0].thrustDelay = missileLauncher.overrideDropTime;
                }
                // int missileCount = missileLauncher.missileCount;
                /*GameObject dummyMissile = Instantiate(@object, wm.hardpointTransforms[num]);
                dummyMissile.transform.localScale = new Vector3(20f, 20f, 20f);
                dummyMissile.transform.eulerAngles = new Vector3(dummyMissile.transform.eulerAngles.x, dummyMissile.transform.eulerAngles.y, dummyMissile.transform.eulerAngles.z + 90f);
                dummyMissile.SetActive(true);*/
                missileLauncher.missiles[0].transform.localScale = new Vector3(20f, 20f, 20f);
                missileLauncher.missiles[0].transform.parent = equipper.transform;
                missileLauncher.missiles[0].transform.position = equipper.transform.position;
                missileLauncher.missiles[0].transform.eulerAngles = new Vector3(missileLauncher.missiles[0].transform.eulerAngles.x, missileLauncher.missiles[0].transform.eulerAngles.y, missileLauncher.missiles[0].transform.eulerAngles.z + 90f);
                missileLauncher.missiles[0].gameObject.SetActive(true);
                // Destroy(dummyMissile);
                Missile.LaunchEvent launchEvent = new Missile.LaunchEvent();
                launchEvent.delay = 0f;
                launchEvent.launchEvent = new UnityEvent();
                launchEvent.launchEvent.AddListener(new UnityAction(() =>
                {
                    Debug.Log("Launch event was called.");
                        // dummyMissile.SetActive(false);
                        /*Destroy(dummyMissile); */
                }));
                missileLauncher.missiles[0].launchEvents = new List<Missile.LaunchEvent>();
                missileLauncher.missiles[0].launchEvents.Add(launchEvent);
                toCheck = missileLauncher.missiles[0];
                Debug.Log("Missiles loaded!");
            }
            else
            {
                Debug.LogError(hpLoadout[num] + " is null or empty.");
            }
            num++;
        }
        if (wm.vesselRB)
        {
            wm.vesselRB.ResetInertiaTensor();
        }
        Debug.Log("intertia tensor.");
        if (loadout.cmLoadout != null)
        {
            CountermeasureManager componentInChildren = GetComponentInChildren<CountermeasureManager>();
            if (componentInChildren)
            {
                int num2 = 0;
                while (num2 < componentInChildren.countermeasures.Count && num2 < loadout.cmLoadout.Length)
                {
                    componentInChildren.countermeasures[num2].count = Mathf.Clamp(loadout.cmLoadout[num2], 0, componentInChildren.countermeasures[num2].maxCount);
                    componentInChildren.countermeasures[num2].UpdateCountText();
                    num2++;
                }
            }
        }
        traverse.Field("weaponIdx").SetValue(0);
        Debug.Log("weaponIDX.");
        wm.ToggleMasterArmed();
        wm.ToggleMasterArmed();
        if (wm.OnWeaponChanged != null)
        {
            wm.OnWeaponChanged.Invoke();
        }
        component.UpdateMassObjects();
        traverse.Field("rcsAddDirty").SetValue(true);
        // wm.ReattachWeapons();
        Debug.Log("Should be working now...");
        wm.RefreshWeapon();
        foreach (var equip in wm.GetCombinedEquips())
        {
            Debug.Log(equip);
        }
    }
    /*private void Update()
    {
        if (toCheck != null)
        {
            Debug.Log(toCheck.gameObject.activeSelf + " " + toCheck.gameObject.transform.position + " " + toCheckAgainst.position + " " + toCheck.gameObject.transform.localScale + " Same: " + (toCheck.transform.position == toCheckAgainst.position));
        }
    }*/
    public void DoMain()
    {
        StartCoroutine(main());
    }
    private void Start()
    {
        HarmonyInstance instance = HarmonyInstance.Create("TEMPY.NUKE.B61");
        instance.PatchAll(Assembly.GetExecutingAssembly());
        ModLoaded();
    }
    public override void ModLoaded()
    {
        VTOLAPI.SceneLoaded += SceneChanged; // So when the scene is changed SceneChanged is called
        VTOLAPI.MissionReloaded += DoMain; // So when the mission is reloaded DoMain is called
        base.ModLoaded();
    }
    private void SceneChanged(VTOLScenes scenes)
    {
        if (scenes == VTOLScenes.Akutan || scenes == VTOLScenes.CustomMapBase) // If inside of a scene that you can fly in
        {
            StartCoroutine(main());
        }
    }
    public static WeaponManager wma;
    private Missile toCheck;
    private Transform toCheckAgainst;
    public static Loadout sloadout;
}