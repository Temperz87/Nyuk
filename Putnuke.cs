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
        // wm.resourcePath = Environment.CurrentDirectory + "\\VTOLVR_ModLoader\\mods\\AEAT\\nukey";
        Debug.Log("started hax2r");
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
            wm.EquipWeapons(loadout);
            EquipCustomWeapons(cloadout, wm);
        }
        Debug.Log("did hax0r");
        /*int i = 0;
        foreach (var equip in wm.GetCombinedEquips())
        {
            if (equip.name == "Nuke")
            {
                Nuke nyuk = equip.gameObject.AddComponent<Nuke>();
                if (nyuk.isNuke)
                {
                    Debug.Log(equip.ToString() + " is now a nuke.");
                    i++;
                }
                else
                {
                    Debug.Log(equip.ToString() + " is not a nuke.");
                    nyuk.enabled = false;
                }
            }
        }
        Debug.Log("Made " + i + " objects a nuke.");*/
    }
    public void EquipCustomWeapons(Loadout loadout, WeaponManager wm)
    {
        triggered = true;
        Traverse traverse = Traverse.Create(wm);
        HPEquippable[] equips = (HPEquippable[])traverse.Field("equips").GetValue();
        MassUpdater component = wm.vesselRB.GetComponent<MassUpdater>();
        /*for (int i = 0; i < wm.equipCount; i++)
		{
			if (equips[i] != null)
			{
				foreach (IMassObject o in equips[i].GetComponentsInChildren<IMassObject>())
				{
					component.RemoveMassObject(o);
				}
				equips[i].OnUnequip();
				/*if (wm.OnWeaponUnequippedHPIdx != null)
				{
					wm.OnWeaponUnequippedHPIdx(i);
				} // might fuck it all up
				UnityEngine.Object.Destroy(equips[i].gameObject);
				equips[i] = null;
			}
		}*/ // What's commented out is a function to remove all weapons, this will be able to just attach one
        string[] hpLoadout = loadout.hpLoadout;
        int num = 0;
        Debug.Log(resourcePath);
        var bundle = AssetBundle.LoadFromFile(resourcePath);
        while (num < wm.hardpointTransforms.Length && num < hpLoadout.Length)
        {
            if (!string.IsNullOrEmpty(hpLoadout[num]))
            {
                Debug.Log(hpLoadout[num]);
                GameObject @object = bundle.LoadAsset<GameObject>(hpLoadout[num]);
                @object.transform.localScale = new Vector3( 20f, 20f, 20f);
                if (@object)
                {
                    GameObject missileObject = @object;
                    //GameObject missileObject = Instantiate(@object, wm.hardpointTransforms[num]);
                    toCheckAgainst = wm.hardpointTransforms[num];
                    Debug.Log("Instantiated custom weapon.");
                    // Nuke nyuk = missileObject.AddComponent<Nuke>();
                    missileObject.name = hpLoadout[num];
                    Debug.Log("Changed name.");
                    missileObject.transform.localRotation = Quaternion.identity;
                    Debug.Log("Quaternion identity.");
                    missileObject.transform.localPosition = Vector3.zero;
                    Debug.Log("local position.");
                    missileObject.transform.localScale = new Vector3(20f, 20f, 20f);
                    Debug.Log("local scale.");
                    /*Actor actor = missileObject.AddComponent<Actor>();
                    actor.actorName = "B61";
                    actor.team = Teams.Allied;
                    actor.opticalTargetable = true;
                    actor.role = Actor.Roles.Missile;
                    actor.iconType = UnitIconManager.MapIconTypes.Missile;
                    actor.iconRotation = 999f;*/
                    // HPEquipBombRack component2 = nyuk.HPEquipper;
                    /*if (component2 == null)
					{
						Debug.LogError("component2 null");
					}
					Debug.Log("HPEquippable.");
					if (wm == null)
					{
						Debug.LogError("wm null");
					}*/

                    if (missileObject == null)
                    {
                        Debug.LogError("missileObject is null.");
                    }

                    Missile missile;
                    Health health;
                    SimpleDrag simpleDrag;
                    HPEquipBombRack HPEquipper;
                    missile = missileObject.AddComponent<Missile>();
                    if (missile == null)
                    {
                        Debug.LogError("missile null.");
                    }
                    missile.edgeTransform = missileObject.transform.GetChild(0);
                    Debug.Log("Got child 1 " + missile.edgeTransform.name);
                    missile.highPolyModel = missileObject;
                    missile.lowPolyModel = missileObject;
                    missile.mass = 0.565f;
                    missile.angularDrag = 0.2f;
                    missile.guidanceMode = Missile.GuidanceModes.Bomb;
                    missile.insBackup = true;
                    missile.navMode = Missile.NavModes.LeadTime;
                    missile.leadTimeMultiplier = 1f;
                    missile.boostTime = 1f;
                    Debug.Log("halfway there at nuke...");
                    missile.cruiseTime = 25f;
                    missile.maxTorque = 0.1f;
                    missile.maxTorqueSpeed = 320f;
                    missile.torqueRampUpRate = 1f;
                    missile.minTorqueSpeed = 100f;
                    missile.torqueToPrograde = 0.25f;
                    missile.maxLeadTime = 8f;
                    missile.maxBallisticOffset = 100f;
                    missile.rollUpBias = 0.4f;
                    missile.torqueKickOnLaunch = new Vector2(0.1f, 0.15f);
                    missile.decoupleSpeed = 4f;
                    missile.opticalFOV = 60f;
                    // missile.debugMissile = true;
                    Debug.Log("Almost done with nuke...");
                    Debug.Log("Missile nuke done.");
                    health = missileObject.AddComponent<Health>();
                    health.startHealth = 20f;
                    health.maxHealth = 20f;
                    health.OnDeath = new UnityEvent();
                    Hitbox hB = missile.edgeTransform.gameObject.AddComponent<Hitbox>();
                    hB.health = health;
                    CapsuleCollider cC = missile.edgeTransform.gameObject.AddComponent<CapsuleCollider>();
                    cC.radius = 0.2f;
                    cC.height = 3f;
                    Debug.Log("Health nuke done.");
                    simpleDrag = missileObject.AddComponent<SimpleDrag>();
                    simpleDrag.area = 0.0010029f;
                    simpleDrag.offsetFromCoM = new Vector3(0f, 0f, -0.15f);
                    Debug.Log("Simpledrag nuke done.");
                    GameObject equipper = missileObject.transform.GetChild(1).gameObject;
                    /*Rigidbody rB = equipper.AddComponent<Rigidbody>();
                    rB.mass = 1f;
                    rB.angularDrag = 0.05f;
                    rB.useGravity = true;
                    rB.isKinematic = true;
                    rB.collisionDetectionMode = CollisionDetectionMode.Discrete;*/
                    if (equipper.transform == null)
                    {
                        Debug.LogError("Equipper's transform is null.");
                    }
                    HPEquipper = equipper.AddComponent<HPEquipBombRack>();
                    equipper.transform.position = wm.hardpointTransforms[num].position;
                    equipper.transform.parent = wm.hardpointTransforms[num];
                    equipper.SetActive(true);
                    HPEquipper.name = "Nuke";
                    HPEquipper.fullName = "B61 Nuclear Bomb";
                    HPEquipper.shortName = "B61";
                    HPEquipper.unitCost = 100000f;
                    HPEquipper.description = "A variable yield thermonuclear dumb bomb that will annihilate everything in a 66000ft radius.";
                    HPEquipper.subLabel = "BOMB";
                    HPEquipper.jettisonable = true;
                    HPEquipper.armable = true;
                    HPEquipper.reticleIndex = 0;
                    HPEquipper.allowedHardpoints = "1,4,5,6,7,10,11,12,13";
                    HPEquipper.baseRadarCrossSection = 0.5f;
                    HPEquipper.perMissileCost = 100f;
                    Debug.Log("HPEquipper nuke done");
                    MissileLauncher missileLauncher = equipper.AddComponent<MissileLauncher>();
                    HPEquipper.ml = missileLauncher;
                    missileLauncher.missiles = new Missile[1];
                    missileLauncher.hardpoints = new Transform[1];
                    missileLauncher.hardpoints[0] = equipper.transform;
                    missileLauncher.missilePrefab = missileObject;
                    missileLauncher.useEdgeTf = false;
                    missileLauncher.hideUntilLaunch = false;
                    Debug.Log("MissileLauncher nuke done (1/2)");
                    AudioSource audioSource = equipper.AddComponent<AudioSource>();
                    AudioClip audioClip = Resources.Load<AudioClip>("AudioClip/jettison");
                    audioSource.clip = audioClip;
                    audioSource.rolloffMode = AudioRolloffMode.Linear;
                    audioSource.minDistance = 1f;
                    audioSource.maxDistance = 500f;
                    missileLauncher.launchAudioSources = new AudioSource[1];
                    missileLauncher.launchAudioSources[0] = audioSource;
                    missileLauncher.launchAudioClips = new AudioClip[1];
                    missileLauncher.launchAudioClips.Add(audioClip);
                    missileLauncher.overrideDecoupleSpeed = -1f;
                    missileLauncher.overrideDropTime = -1f;
                    // missileLauncher.SetParentRigidbody(rB);
                    Debug.Log("MissileLauncher nuke REALLY done (2/2)");


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
                    /*Nuke nuke = missileObject.AddComponent<Nuke>();

                    Debug.Log("Any minute now...");
                    nuke.missile = missile;
                    Debug.Log("Almost there...");
                    if (missile == null)
                    {
                        Debug.LogError("missile is null");
                    }
                    if (nuke == null)
                    {
                        Debug.LogError("nuke is null");
                    }
                    Debug.Log("The nuke now has teeth.");*/
                    /*if (wm.OnWeaponEquipped != null)
					{
						wm.OnWeaponEquipped(component2);
					}
					if (wm.OnWeaponEquippedHPIdx != null)
					{
						wm.OnWeaponEquippedHPIdx(num);
					}*/ // Another weird thing i had to say BYEEEEEEE to
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
                        /*if (!wm.uniqueWeapons.Contains(component2.shortName))
						{
							wm.uniqueWeapons.Add(component2.shortName);
						}*/
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
                    Debug.Log("Component is.");
                    if (HPEquipper is HPEquipIRML || HPEquipper is HPEquipRadarML)
                    {
                        if (HPEquipper.dlz)
                        {
                            traverse.Field("maxAntiAirRange").SetValue(Mathf.Max(HPEquipper.dlz.maxLaunchRange, wm.maxAGMRange));
                            // wm.maxAntiAirRange = Mathf.Max(component2.dlz.maxLaunchRange, wm.maxAntiAirRange);
                        }
                    }
                    else if (HPEquipper is HPEquipARML)
                    {
                        if (HPEquipper.dlz)
                        {
                            traverse.Field("maxAntiRadRange").SetValue(Mathf.Max(HPEquipper.dlz.maxLaunchRange, wm.maxAGMRange));
                            // wm.maxAntiRadRange = Mathf.Max(component2.dlz.maxLaunchRange, wm.maxAntiRadRange);
                        }
                    }
                    else if (HPEquipper is HPEquipOpticalML && HPEquipper.dlz)
                    {
                        traverse.Field("maxAGMRange").SetValue(Mathf.Max(HPEquipper.dlz.maxLaunchRange, wm.maxAGMRange));
                        // wm.maxAGMRange = Mathf.Max(component2.dlz.maxLaunchRange, wm.maxAGMRange);
                    }
                    Debug.Log("DLZ shit");
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
                    //missileLauncher.missiles[0] = missile;
                    Debug.Log(missileLauncher.missiles[0]);
                    //missileLauncher.missiles[0] = MissileLauncher.LoadMissile(missileLauncher.missilePrefab, missileLauncher.hardpoints[0], missileLauncher.useEdgeTf, missileLauncher.hideUntilLaunch);

                    /*
                    GameObject gameObject = Instantiate(missileLauncher.missilePrefab, missileLauncher.hardpoints[0].position, missileLauncher.hardpoints[0].rotation, missileLauncher.hardpoints[0]);
                    if (gameObject == null)
                    {
                        Debug.LogError("Gameobject null");
                    }
                    Missile component1 = gameObject.GetComponent<Missile>();
                    component1.gameObject.name = missileLauncher.missilePrefab.name;
                    if (missileLauncher.hideUntilLaunch)
                    {
                        if (component1.hiddenMissileObject)
                        {
                            gameObject.SetActive(true);
                            component1.hiddenMissileObject.SetActive(false);
                        }
                        else
                        {
                            gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        gameObject.SetActive(true);
                    }
                    gameObject.transform.localScale = Vector3.one;
                    if (component1.edgeTransform && missileLauncher.useEdgeTf)
                    {
                        component1.edgeTransform.parent = missileLauncher.hardpoints[0];
                        component1.transform.parent = component1.edgeTransform;
                        component1.edgeTransform.localPosition = Vector3.zero;
                        component1.edgeTransform.localRotation = Quaternion.identity;
                        component1.transform.parent = missileLauncher.hardpoints[0];
                        component1.edgeTransform.parent = component1.transform;
                    }
                    missileLauncher.missiles[0] = component1;*/
                    Debug.Log("HEY IT WORKED");
                    Nuke nyuk = missileLauncher.missiles[0].gameObject.AddComponent<Nuke>();
                    missileLauncher.missiles[0].OnDetonate = new UnityEvent();
                    missileLauncher.missiles[0].OnDetonate.AddListener(new UnityAction(() => {
                        Debug.Log("Nuke is now critical.");
                        nyuk.DoNuke(); }));
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
                    GameObject dummyMissile = Instantiate(@object, wm.hardpointTransforms[num]);
                    dummyMissile.transform.localScale = new Vector3(20f, 20f, 20f);
                    dummyMissile.transform.eulerAngles = new Vector3(dummyMissile.transform.eulerAngles.x, dummyMissile.transform.eulerAngles.y, dummyMissile.transform.eulerAngles.z + 90f);
                    dummyMissile.SetActive(true);
                    missileLauncher.missiles[0].transform.localScale = new Vector3(20f, 20f, 20f);
                    missileLauncher.missiles[0].transform.parent = equipper.transform;
                    missileLauncher.missiles[0].transform.position = equipper.transform.position;
                    missileLauncher.missiles[0].transform.eulerAngles = dummyMissile.transform.eulerAngles;
                    missileLauncher.missiles[0].gameObject.SetActive(true);
                    Missile.LaunchEvent launchEvent = new Missile.LaunchEvent();
                    launchEvent.delay = 0f;
                    launchEvent.launchEvent = new UnityEvent();
                    launchEvent.launchEvent.AddListener(new UnityAction(() => {
                        Debug.Log("Launch event was called.");
                        dummyMissile.SetActive(false);
                        Destroy(dummyMissile); }));
                    missileLauncher.missiles[0].launchEvents = new List<Missile.LaunchEvent>();
                    missileLauncher.missiles[0].launchEvents.Add(launchEvent);
                    toCheck = missileLauncher.missiles[0];
                    Debug.Log("Missiles loaded!");
                }
                else
                {
                    Debug.LogError(hpLoadout[num] + "not found in asset bundle.");
                }
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
    private Missile toCheck;
    private Transform toCheckAgainst;
}