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
    private void Awake()
    {
        /*missile = base.gameObject.AddComponent<Missile>();
        missile.edgeTransform = base.gameObject.transform.GetChild(0);
        missile.highPolyModel = gameObject;
        missile.lowPolyModel = gameObject;
        missile.mass = 0.565f;
        missile.angularDrag = 0.2f;
        missile.guidanceMode = Missile.GuidanceModes.Bomb;
        missile.insBackup = true;
        missile.navMode = Missile.NavModes.LeadTime;
        missile.leadTimeMultiplier = 1f;
        missile.boostTime = 1f;
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
        missile.OnDetonate.AddListener(new UnityAction(DoNuke));
        Debug.Log("Missile nuke done.");
        health = gameObject.AddComponent<Health>();
        health.startHealth = 20f;
        health.maxHealth = 20f;
        Debug.Log("Health nuke done.");
        simpleDrag = gameObject.AddComponent<SimpleDrag>();
        simpleDrag.area = 0.0010029f;
        simpleDrag.offsetFromCoM = new Vector3(0f, 0f, -0.15f);
        Debug.Log("Simpledrag nuke done.");
        GameObject equipper = gameObject.transform.GetChild(1).gameObject;
        HPEquipper = equipper.AddComponent<HPEquipBombRack>();
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
        missileLauncher.hardpoints = new Transform[1];
        missileLauncher.hardpoints.Add(equipper.transform);
        missileLauncher.missilePrefab = gameObject;
        Debug.Log("MissileLauncher nuke done (1/2)");
        AudioSource audioSource = equipper.AddComponent<AudioSource>();
        AudioClip audioClip = Resources.Load<AudioClip>("AudioClip/jettison");
        audioSource.clip = audioClip;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 500f;
        missileLauncher.launchAudioSources = new AudioSource[1];
        missileLauncher.launchAudioSources.Add(audioSource);
        missileLauncher.launchAudioClips = new AudioClip[1];
        missileLauncher.launchAudioClips.Add(audioClip);
        missileLauncher.overrideDecoupleSpeed = -1f;
        missileLauncher.overrideDropTime = -1f;
        Debug.Log("MissileLauncher nuke REALLY done (2/2)");*/
        missile = gameObject.GetComponent<Missile>();
        if (missile == null)
        {
            Debug.LogError("Nuke is null from the get go");
        }
        Debug.Log("Nuke armed, awaiting detonation.");
    }
    /*private void Update()
    {
        if (gameObject != null)
        {
            if (gameObject.transform != null)
            {
                Position = gameObject.transform;
                Debug.Log("Changed position transform");
            }
        }
    }*/
    // Literally baha's code
    public void DoNuke()
    {
        Debug.Log("Starting nuke routine");
        if (missile == null)
        {
            Debug.LogError("Nuke null?");
        }
        if (gameObject.transform == null)
        {
            Debug.LogError("gameObject transform null.");
        }
        if (BOOM.instance == null)
        {
            BOOM lol = new BOOM();
            if (BOOM.instance == null)
            {
                Debug.LogError("Instance is stubborn.");
            }
        }
        BOOM.instance.DoExplode(gameObject.transform);
    }
    
    private void ChangeAlpha(Material mat, float alphaVal)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        mat.SetColor("_Color", newColor);

    }
    private Missile missile;
    /*private Missile missile;
    private Health health;
    private SimpleDrag simpleDrag;
    public HPEquipBombRack HPEquipper;
    public bool isNuke = false;*/
}
class BOOM : MonoBehaviour
{
    public static BOOM instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("Set boom instance.");
        }
    }
    public void DoExplode(Transform pos)
    {
        StartCoroutine(ExplosionDamageRoutine(pos));
    }
    private IEnumerator ExplosionDamageRoutine(Transform targetPos)
    {
        if (targetPos == null)
        {
            Debug.LogError("Target pos null, nuke will not work.");
        }
        GameObject j4Ship = Instantiate(Resources.Load<GameObject>("units/enemy/mothership"));
        Debug.Log("Instantiate");
        J4Mothership jM = j4Ship.GetComponent<J4Mothership>();
        Debug.Log("Component");
        GameObject explosionObject = jM.explosionObject;
        Debug.Log("Explosion");
        explosionObject.transform.parent = null;
        Debug.Log("Parent has been nulled");
        explosionObject.transform.position = targetPos.position;
        Debug.Log("transform position");
        Transform explodeSphereTf = jM.explodeSphereTf;
        Debug.Log("explode = jm");
        explodeSphereTf.parent = explosionObject.transform;
        Debug.Log("parent transform");
        explodeSphereTf.position = targetPos.position;
        Debug.Log("same position");
        Destroy(j4Ship);
        Debug.Log("destroy");
        Debug.Log("Awaiting 6.6 seconds");
        yield return new WaitForSeconds(6.6f);
        Debug.Log("Wait done");
        explosionObject.SetActive(true);
        Debug.Log("Active");
        explosionObject.transform.parent = null;
        Debug.Log("Nulling parent");
        float r = 0f;
        Debug.Log("Nuking...");
        while (r < 20000f)
        {
            float num = r * r;
            for (int i = 0; i < TargetManager.instance.allActors.Count; i++)
            {
                Actor actor = TargetManager.instance.allActors[i];
                if (actor.alive && (actor.position - explosionObject.transform.position).sqrMagnitude < num && actor.transform != explosionObject.transform)
                {
                    Health component = actor.GetComponent<Health>();
                    if (component)
                    {
                        component.Kill();
                    }
                }
            }
            explodeSphereTf.localScale = 2f * r * Vector3.one;
            r += 343f * Time.deltaTime;
            yield return null;
        }
        /*float t = 1f;
        Renderer fadeout = explodeSphereTf.GetComponent<Renderer>();
        while (t > 0f)
        {
            ChangeAlpha(explodeSphereTf.GetComponent<Renderer>().material, t - 0.05f);
            t = t - 0.05f;
            yield return new WaitForSeconds(0.2f);
        }*/
        explodeSphereTf.gameObject.SetActive(false);
        yield break;
    }
}