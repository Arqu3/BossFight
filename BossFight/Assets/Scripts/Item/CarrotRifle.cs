using UnityEngine;
using System.Collections;

public class CarrotRifle : Weapon
{
    //Public vars a
    public float m_Cooldown = 1.0f;
    public GameObject m_Projectile;
    public GameObject m_Carrot;
    public int m_MinAmount = 0;
    public int m_MaxAmount = 10;
    public int m_CarrotBonusDMG = 5;
    public float m_CarrotBonusMulti = 0.1f;

    //Cooldown vars
    bool m_IsCooldown = false;
    float m_CurCooldown;

    //Component vars
    EntityStats m_Stats;

	public override void Start ()
    {
        base.Start();
        m_CurCooldown = m_Cooldown;
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

    public int GetBonusDamage()
    {
        return (int)((m_Stats.GetDamage() + m_CarrotBonusDMG) * m_CarrotBonusMulti);
    }

    public override void SetEquiped(bool state)
    {
        base.SetEquiped(state);

        if (GetIsEquiped())
        {
            m_Stats = transform.parent.transform.parent.GetComponent<EntityStats>();
        }
    }

    void Shoot()
    {
        GameObject clone = (GameObject)Instantiate(m_Projectile, transform.position, Quaternion.identity);
        if (clone.GetComponent<Projectile>())
        {
            clone.GetComponent<Projectile>().m_TotalDamage = m_Stats.GetDamage();

            int random = Random.Range(m_MinAmount, m_MaxAmount + 1);
            clone.GetComponent<Projectile>().m_OnDestroyNum = random;

            if (m_Carrot)
                clone.GetComponent<Projectile>().m_OnDestroySpawn = m_Carrot;
        }
    }
}
