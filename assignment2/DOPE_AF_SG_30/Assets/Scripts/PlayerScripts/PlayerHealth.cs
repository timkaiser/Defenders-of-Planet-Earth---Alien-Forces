using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{

    int health = 30;
    int maxHealth;
    float downedTime;
    Coroutine hitCoroutine;
    Coroutine deathCoroutine;
    GameObject cam;
    GameObject weaponCamera;
    public bool invincible = true;
    public Image bloodyScreen;
    public Image deathBarTop;
    public Image deathBarBottom;
    public GameObject crosshair;
    RectTransform canvas;
    Color bscolour;
    public Font youDiedFont;
    public enum HealthState { alive, downed, dead };
    HealthState state = HealthState.alive;
    public Texture2D greenHealth;
    public Texture2D redHealth;
    public Texture2D backgroundHealth;

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        cam = HelperFunctions.getChildGameObject(this.gameObject, "Main Camera");
        weaponCamera = HelperFunctions.getChildGameObject(this.gameObject, "WeaponCamera");
        maxHealth = health;
        if (bloodyScreen == null)
            bloodyScreen = GameObject.Find("Bloody Screen So Real").GetComponent<Image>();
        if (deathBarTop == null)
            deathBarTop = GameObject.Find("DeathBarTop").GetComponent<Image>();
        if (deathBarBottom == null)
            deathBarBottom = GameObject.Find("DeathBarBottom").GetComponent<Image>();
        if (crosshair == null)
            crosshair = GameObject.Find("CrosshairPackage");
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        deathBarTop.rectTransform.localPosition = new Vector3(0, canvas.sizeDelta.y / 2, 0);
        deathBarBottom.rectTransform.localPosition = new Vector3(0, -canvas.sizeDelta.y / 2, 0);
        bscolour = bloodyScreen.color;
    }

    [ClientRpc]
    public void RpcReceiveDamage(int dmg, GameObject byObject)
    {
        if (health <= 0 || !isLocalPlayer)
            return;

        if (!invincible)
            health -= dmg;
        if (hitCoroutine != null)
            StopCoroutine(hitCoroutine);
        if (health <= 0)
        {
            health = 0;
            CmdSetHealthState(HealthState.downed);
            deathCoroutine = StartCoroutine(die());
        }
        else if(isLocalPlayer)
        {
            hitCoroutine = StartCoroutine(hitEffect());
        }

    }

    IEnumerator hitEffect()
    {
        Vector3 newRot = new Vector3(Random.Range(-15.0f, 5.0f), Random.Range(-10.0f, 10.0f), 0);
        cam.transform.eulerAngles = weaponCamera.transform.eulerAngles = newRot;
        float startT = Time.time;
        for (float i = 0; i <= 1 / 3.0f; i = Time.time - startT)
        {
            cam.transform.localEulerAngles = weaponCamera.transform.localEulerAngles = Vector3.Slerp(newRot, Vector3.zero, i * 3);
            yield return new WaitForEndOfFrame();
            bscolour.a = Mathf.Lerp(1, 0, i * 3);
            bloodyScreen.color = bscolour;
        }
        cam.transform.localEulerAngles = weaponCamera.transform.localEulerAngles = Vector3.zero;
        bscolour.a = 0;
        bloodyScreen.color = bscolour;
        yield return null;
    }

    IEnumerator die()
    {
        Vector3 newRot = new Vector3(Random.Range(-15.0f, 5.0f), Random.Range(-10.0f, 10.0f), 0);
        disableScripts();
        if (isLocalPlayer)
        {
            crosshair.SetActive(false);
            //Hit Effect
            cam.transform.eulerAngles = weaponCamera.transform.eulerAngles = newRot;
        }
        float startT = Time.time;
        for (float i = 0; i <= 1 / 3.0f; i = Time.time - startT)
        {
            if (isLocalPlayer)
            {
                cam.transform.localEulerAngles = weaponCamera.transform.localEulerAngles = Vector3.Slerp(newRot, Vector3.zero, i * 3);
                bscolour.a = Mathf.Lerp(1, 0, i * 3);
                bloodyScreen.color = bscolour;
            }
            yield return new WaitForEndOfFrame();
        }
        if (isLocalPlayer)
        {
            cam.transform.localEulerAngles = weaponCamera.transform.localEulerAngles = Vector3.zero;
            bscolour.a = 0;
            bloodyScreen.color = bscolour;
        }

        //Down animation
        startT = Time.time;
        Color blackOverlayColour = Color.black;
        Vector3 startPos = transform.position;
        for (float i = 0; i <= 20; i = Time.time - startT)
        {
            float t = i / 20.0f;
            transform.position = Vector3.Lerp(startPos, new Vector3(startPos.x, 0, startPos.z), t);
            transform.eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(-90, 0, 0), t);
            if (isLocalPlayer)
            {
                Vector2 rectTSize = new Vector2(1920, Mathf.Lerp(0, canvas.sizeDelta.y, t));
                deathBarBottom.rectTransform.sizeDelta = deathBarTop.rectTransform.sizeDelta = rectTSize;
            }
            yield return new WaitForEndOfFrame();
        }
        if(isLocalPlayer)
            deathBarBottom.rectTransform.sizeDelta = deathBarTop.rectTransform.sizeDelta = new Vector2(1920, canvas.sizeDelta.y);

        CmdSetHealthState(HealthState.dead);

        if (isLocalPlayer)
        {
            GameObject youDied = new GameObject();
            youDied.transform.parent = canvas.transform;
            Text youDiedText = youDied.AddComponent<Text>();
            youDiedText.text = "YOU DIED.";
            youDiedText.rectTransform.localPosition = Vector3.zero;
            youDiedText.rectTransform.sizeDelta = new Vector2(1600, 250);
            youDiedText.alignment = TextAnchor.MiddleCenter;
            youDiedText.color = new Color32(200, 0, 0, 255);
            youDiedText.fontSize = 200;
            youDiedText.font = youDiedFont;
            youDiedText.rectTransform.localScale = new Vector3(0, 0.5f, 0);

            yield return new WaitForSeconds(0.25f);
            startT = Time.time;
            for (float i = 0; i < 4; i = Time.time - startT)
            {
                youDiedText.rectTransform.localScale = Vector3.Lerp(new Vector3(0, 0.5f, 0), new Vector3(1, 1, 1), i / 4);
                yield return new WaitForEndOfFrame();
            }
            youDiedText.rectTransform.localScale = new Vector3(1, 1, 1);
        }
        yield return null;
    }

    [Command]
    void CmdSetHealthState(HealthState hs)
    {
        RpcSetHealthState(hs);
    }

    [ClientRpc]
    void RpcSetHealthState(HealthState hs)
    {
        state = hs;
    }

    [Command]
    public void CmdRevive()
    {
        RpcRevive();
    }

    [ClientRpc]
    public void RpcRevive()
    {
        if (state == HealthState.downed)
        {
            CmdSetHealthState(HealthState.alive);
            enableScripts();
            if(isLocalPlayer)
                crosshair.SetActive(true);
            if (deathCoroutine != null)
                StopCoroutine(deathCoroutine);
            StartCoroutine(reviveAnim());
            health = 20;
        }
        else if(state == HealthState.alive)
        {
            health = 20;
        }
    }

    IEnumerator reviveAnim()
    {
        float startT = Time.time;
        float startSize = 0;
        Vector3 startPos = transform.position;
        Vector3 startRot = transform.eulerAngles;
        if (isLocalPlayer)
        {
            bloodyScreen.color = bscolour;
            cam.transform.localEulerAngles = weaponCamera.transform.localEulerAngles = Vector3.zero;
            startSize = deathBarBottom.rectTransform.sizeDelta.y;
        }
        for (float i = 0; i < 0.5f; i = Time.time - startT)
        {
            float t = i * 2;
            if(isLocalPlayer)
                deathBarBottom.rectTransform.sizeDelta = deathBarTop.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(1920, startSize), new Vector2(1920, 0), t);
            transform.position = Vector3.Lerp(startPos, new Vector3(startPos.x, 1, startPos.z), t);
            transform.eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, 0), t);
            yield return new WaitForEndOfFrame();
        }
        if(isLocalPlayer)
            deathBarBottom.rectTransform.sizeDelta = deathBarTop.rectTransform.sizeDelta = new Vector2(1920, 0);
        yield return null;
    }

    void disableScripts()
    {
        FireControl fc = GetComponent<FireControl>();
        GPSWalker mov = GetComponent<GPSWalker>();
        fc.enabled = false;
        if(mov != null)
            mov.enabled = false;
    }
    void enableScripts()
    {
        FireControl fc = GetComponent<FireControl>();
        GPSWalker mov = GetComponent<GPSWalker>();
        fc.enabled = true;
        if(mov != null)
            mov.enabled = true;
    }

    public HealthState getHealthState()
    {
        return state;
    }

    void OnGUI()
    {
        if (!isLocalPlayer)
            return;

        for (int i = 0; i < maxHealth; i++)
        {
            if (i < health)
                GUI.Label(new Rect(15 + i * 10, Screen.height - 40, 10, 25), greenHealth);
            else
                GUI.Label(new Rect(15 + i * 10, Screen.height - 40, 10, 25), redHealth);
        }
    }
}