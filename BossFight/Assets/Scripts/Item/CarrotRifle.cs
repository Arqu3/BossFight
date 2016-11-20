using UnityEngine;
using System.Collections;

public class CarrotRifle : Weapon
{
    //Public vars
    public float m_Cooldown = 1.0f;
    public GameObject m_Projectile;

    bool m_IsCooldown = false;
    float m_CurCooldown;

    //Component vars
    EntityStats m_Stats;

	void Start ()
    {
        m_CurCooldown = m_Cooldown;
        m_Stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
	}
	
	void Update ()
    {
        if (GetIsEquiped())
        {
            if (Input.GetKey(KeyCode.F))
            {
                if (!m_IsCooldown)
                {
                    m_IsCooldown = true;
                    Shoot();
                }
            }

            if (m_IsCooldown)
            {
                m_CurCooldown -= Time.deltaTime;
                if (m_CurCooldown <= 0.0f)
                {
                    m_CurCooldown = m_Cooldown;
                    m_IsCooldown = false;
                }
            }
        }
	}

    void Shoot()
    {
        GameObject clone = (GameObject)Instantiate(m_Projectile, m_Stats.transform.position, Quaternion.identity);
    }
}
