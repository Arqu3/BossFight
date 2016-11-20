using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Projectile : MonoBehaviour
{
    //Public vars
    public float m_Speed = 30.0f;
    public bool m_LimitedLifetime = false;
    public float m_Lifetime = 3.0f;
    public int m_BonusDamage = 0;
    public float m_BonusMulti = 0.0f;
    public GameObject m_OnDestroySpawn;
    public int m_OnDestroyAmount = 0;

    int m_TotalDamage = 0;
    Vector3 m_Direction;

    //Component vars
    Rigidbody m_Rigidbody;

	void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TotalDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>().GetDamage() + (int)(m_BonusDamage * m_BonusMulti);

        if (transform.rotation.x != 90)
            transform.Rotate(90, 0, 0);

        m_Direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameObject.FindGameObjectWithTag("Player").transform.position;
        m_Direction.y = 0.0f;
	}
	
	void Update()
    {
        if (m_LimitedLifetime)
        {
            m_Lifetime -= Time.deltaTime;
            if (m_Lifetime <= 0.0f)
                Destroy(gameObject);
        }
        m_Rigidbody.velocity = m_Direction.normalized * m_Speed;
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (col.gameObject.GetComponent<EntityStats>())
            {
                col.gameObject.GetComponent<EntityStats>().ChangeHealth(-m_TotalDamage);
            }
        }
        if (col.gameObject.tag != "Player")
        {
            if (m_OnDestroySpawn && m_OnDestroyAmount > 0)
            {
                {
                    if (m_OnDestroySpawn != this.gameObject)
                    {
                        for (int i = 0; i < m_OnDestroyAmount; i++)
                        {
                            GameObject clone = (GameObject)Instantiate(m_OnDestroySpawn, transform.position, transform.rotation);
                        }
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
