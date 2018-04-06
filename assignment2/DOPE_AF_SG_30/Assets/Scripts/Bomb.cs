using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour
{
    public GameObject m_bombPartcile;
    public GameObject m_explosionPrefab;

    private float m_time_to_explode;
    private float m_time;

    public int m_bombradius;

    private bool m_burning;
    [SyncVar]public bool m_dead;

	// Use this for initialization
	void Start ()
    {
        m_time_to_explode = 5.0f;


        m_time = Time.fixedTime;
        m_burning = false;
    }
	
    [ServerCallback]
	void FixedUpdate ()
    {
        if (!m_dead)
        {
            if (Time.fixedTime - m_time > m_time_to_explode)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, m_bombradius);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].tag.Equals("Player"))
                    {
                        PlayerHealth ph = colliders[i].gameObject.GetComponent<PlayerHealth>();
                        ph.RpcReceiveDamage(5, this.gameObject);
                    }
                }
                m_dead = true;
            }
            else if (!m_burning && Time.fixedTime - m_time > m_time_to_explode / 2)
            {
                NetworkServer.Spawn(Instantiate(m_bombPartcile, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity));
                m_burning = true;
            }
        }
        else
        {
            NetworkServer.Spawn(Instantiate(m_explosionPrefab, gameObject.transform.position, Quaternion.identity));
            NetworkServer.Destroy(gameObject);
        }  
	}
}
