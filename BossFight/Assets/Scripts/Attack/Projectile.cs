using UnityEngine;
using System.Collections;

public enum ProjectileMode
{
    Player,
    Enemy
}

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    //Public vars
    public ProjectileMode m_Mode = ProjectileMode.Player;
    public float m_Speed = 30.0f;
    public bool m_LimitedLifetime = false;
    public float m_Lifetime = 3.0f;
    public int m_BonusDamage = 0;
    public float m_BonusMulti = 0.0f;
    public GameObject m_OnDestroySpawn;
    public int m_OnDestroyNum = 0;

    public int m_TotalDamage = 0;
    public Vector3 m_Direction;

    //Component vars
    Rigidbody m_Rigidbody;

	public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TotalDamage += (int)(m_BonusDamage * m_BonusMulti);

        transform.localEulerAngles = new Vector3(90, 0.0f, 0.0f);

        m_Direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameObject.FindGameObjectWithTag("Player").transform.position;
        m_Direction.y = 0.0f;
	}
	
	public virtual void Update()
    {
        if (m_LimitedLifetime)
        {
            m_Lifetime -= Time.deltaTime;
            if (m_Lifetime <= 0.0f)
                Destroy(gameObject);
        }
        m_Rigidbody.velocity = m_Direction.normalized * m_Speed;
	}

    public virtual void OnTriggerEnter(Collider col)
    {
        string s = "";
        string s1 = "";
        if (m_Mode.Equals(ProjectileMode.Player))
        {
            s = "Enemy";
            s1 = "Player";
        }
        else
        {
            s = "Player";
            s1 = "Enemy";
        }

        if (col.gameObject.tag == s)
        {
            if (col.gameObject.GetComponent<EntityStats>())
            {
                col.gameObject.GetComponent<EntityStats>().ChangeHealth(-m_TotalDamage);
            }
        }
        if (col.gameObject.tag != s1 && col.gameObject.tag != "Projectile" && col.gameObject.tag != "HitCollider")
        {
            if (m_OnDestroySpawn && m_OnDestroyNum > 0)
            {
                if (m_OnDestroySpawn != this.gameObject)
                {
                    for (int i = 0; i < m_OnDestroyNum; i++)
                    {
                        GameObject clone = (GameObject)Instantiate(m_OnDestroySpawn, transform.position, transform.rotation);
                        if (clone.GetComponent<Projectile>())
                            clone.GetComponent<Projectile>().m_Mode = m_Mode;
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
