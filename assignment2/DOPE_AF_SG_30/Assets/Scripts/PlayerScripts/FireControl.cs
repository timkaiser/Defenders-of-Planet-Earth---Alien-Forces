using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireControl : NetworkBehaviour {

    //PlayerNetworkManager netMan;
    Transform spawnpoint;

    [System.Serializable]
    public class Weapon{
        public GameObject projectile;
        public float ejectionSpeed;
        public float reloadTime;
        [HideInInspector] public float lastTimeOfFire;
    }

    public Weapon[] weapons;
    public enum WeaponType {Rocket, Dart, Laser};
    [SyncVar]int currentWeapon = 0;

    void Start () {
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].lastTimeOfFire = -weapons[i].reloadTime;
        }
        spawnpoint = HelperFunctions.getChildGameObject(gameObject, "Spawnpoint").transform;
    }
	
	void FixedUpdate() {
        if (!isLocalPlayer)
            return;
#if UNITY_EDITOR || UNITY_STANDALONE
        pcFire();
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        androidFire();
#endif
    }

    void pcFire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdFire();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CmdSetWeaponType(WeaponType.Rocket);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            CmdSetWeaponType(WeaponType.Dart);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            CmdSetWeaponType(WeaponType.Laser);
    }

    void androidFire()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
                CmdFire();
        }
    }

    [Command]
    void CmdFire()
    {
        if (Time.time - weapons[currentWeapon].lastTimeOfFire > weapons[currentWeapon].reloadTime)
        {
			try
			{
                GameObject r = Instantiate(weapons[currentWeapon].projectile, spawnpoint.position, spawnpoint.rotation) as GameObject;
                RocketBehaviour rbehave = r.GetComponent<RocketBehaviour>();
                LaserBehaviour lbehave = r.GetComponent<LaserBehaviour>();
                if(rbehave != null)
                    rbehave.sender = this.gameObject;
                weapons[currentWeapon].lastTimeOfFire = Time.time;
				NetworkServer.Spawn(r);

                //Laser Behaviour
                if (lbehave != null)
                {
                    RaycastHit hit;
                    Vector3[] positions;
                    if (Physics.Raycast(spawnpoint.position, spawnpoint.forward, out hit, 150))
                    {
                        GameObject hitObj = hit.collider.gameObject;
                        positions = new Vector3[]{spawnpoint.position, hit.point};
                        if (hitObj.tag.Equals("Enemy"))
                        {
                            CmdDamageEnemy(hitObj, 1);
                        }
                    }
                    else
                    {
                        positions = new Vector3[]{spawnpoint.position, spawnpoint.position + spawnpoint.forward * 150 };
                    }
                    CmdSetLaserPositions(r, positions);
                }
                else
                {
                    Rigidbody rRb = r.GetComponent<Rigidbody>();
                    rRb.velocity = r.transform.forward * weapons[currentWeapon].ejectionSpeed;
                }
            }
			catch(MissingReferenceException) 
			{
				Debug.Log("Error in FireControl -> CmdFire: Projectile is null!");
			}
        }
    }

    [Command]
    public void CmdSetWeaponType(WeaponType wt)
    {
        switch (wt)
        {
            case WeaponType.Rocket: currentWeapon = 0; break;
            case WeaponType.Dart: currentWeapon = 1; break;
            case WeaponType.Laser: currentWeapon = 2; break;
        }
    }

    [Command]
    void CmdDamageEnemy(GameObject enemy, int damage)
    {
        Enemy_Health eh;
        Bomb b;
        if(enemy != null && (eh = enemy.GetComponent<Enemy_Health>()) != null)
        {
            eh.receiveDamage(damage, this.gameObject);
        }
        else if(enemy != null && (b = enemy.GetComponent<Bomb>()) != null)
        {
            b.m_dead = true;
        }
    }

    [Command]
    void CmdSetLaserPositions(GameObject obj, Vector3[] positions)
    {
        obj.GetComponent<LaserBehaviour>().RpcSetPositions(positions);
        obj.GetComponent<LaserBehaviour>().positions = positions;
    }
}
