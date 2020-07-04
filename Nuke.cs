using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;
class Nuke : MonoBehaviour
{
    public void init()
    {
        Debug.Log("Nuke awoken.");
        GameObject missileObject = base.gameObject;
        missile = missileObject.AddComponent<Missile>();
        if (missile == null)
        {
            Debug.LogError("missile null.");
        }
        missile.edgeTransform = missileObject.transform.GetChild(0);
        Debug.Log("Got child 1 " + missile.edgeTransform.name);
        missile.highPolyModel = missileObject;
        missile.lowPolyModel = missileObject;
        missile.mass = 0.640f;
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
        if (equipper.transform == null)
        {
            Debug.LogError("Equipper's transform is null.");
        }
        HPEquipper = equipper.AddComponent<HPEquipBombRack>();
        equipper.SetActive(true);
        HPEquipper.name = "Nuke";
        HPEquipper.fullName = "B61 Mod 16 Nuclear Bomb";
        HPEquipper.shortName = "B61";
        HPEquipper.unitCost = 20000f;
        HPEquipper.description = "A thermonuclear dumb bomb that yields 2.4 megatons and annihilate everything in a 66000ft radius.";
        HPEquipper.subLabel = "BOMB";
        HPEquipper.jettisonable = true;
        HPEquipper.armable = true;
        HPEquipper.reticleIndex = 0;
        HPEquipper.allowedHardpoints = "1,4,5,6,7,10,11,12,13";
        HPEquipper.baseRadarCrossSection = 0.5f;
        HPEquipper.perMissileCost = 0f;
        Debug.Log("HPEquipper nuke done");
        missileLauncher = equipper.AddComponent<MissileLauncher>();
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
        missile = gameObject.GetComponent<Missile>();
        if (missile == null)
        {
            Debug.LogError("Nuke is null from the get go");
        }
        Debug.Log("Nuke armed, awaiting detonation.");
    }
    public GameObject edgeTransform;
    public GameObject equipper;
    public Missile missile;
    public MissileLauncher missileLauncher;
    public Health health;
    public SimpleDrag simpleDrag;
    public HPEquipBombRack HPEquipper;
}