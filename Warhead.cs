using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

class Warhead : MonoBehaviour
{
    private void Awake()
    {
        missile = base.gameObject.GetComponent<Missile>();
    }
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
            _ = new BOOM();
            if (BOOM.instance == null)
            {
                Debug.LogError("Instance not working lmao.");
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