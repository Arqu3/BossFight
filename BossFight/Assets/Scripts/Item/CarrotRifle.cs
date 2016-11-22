using UnityEngine;
using System.Collections;

public class CarrotRifle : Weapon
{
    //Public vars a
    public float m_ReloadTime = 1.0f;
    public float m_FireRate = 0.3f;
    public int m_Ammo = 5;
    public int m_HealthAmount = 20;
    public GameObject m_Projectile;
    public GameObject m_Carrot;
    public int m_MinAmount = 0;
    public int m_MaxAmount = 10;
    public int m_CarrotBonusDMG = 5;
    public float m_CarrotBonusMulti = 0.1f;

    //Cooldown vars
    bool m_IsReloading = false;
    float m_CurReload;
    float m_CurFire;
    int m_CurAmmo;

    //Component vars
    EntityStats m_Stats;
    Healthbar m_AmmoBar;

	public override void Start ()
    {
        base.Start();
        m_CurReload = m_ReloadTime;
        m_CurAmmo = m_Ammo;
        m_CurFire = m_FireRate;
        m_AmmoBar = GetComponentInChildren<Healthbar>();
        m_AmmoBar.ChangeScaleFactor(m_Ammo);
        m_AmmoBar.ChangeScale(m_CurAmmo);
        m_AmmoBar.gameObject.SetActive(false);
	}
	
	void Update ()
    {
        if (GetIsEquiped())
        {
            if (!m_IsReloading)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    m_AmmoBar.gameObject.SetActive(true);
                    if (m_CurFire >= m_FireRate && m_CurAmmo > 0)
                    {
                        Shoot();
                        m_CurAmmo--;
                        m_CurFire = 0.0f;
                        m_AmmoBar.ChangeScale(-1);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.G))
                {
                    m_AmmoBar.gameObject.SetActive(true);
                    if (m_CurAmmo > 0)
                    {
                        Eat();
                        m_CurAmmo--;
                        m_AmmoBar.ChangeScale(-1);
                    }
                }
                else
                    m_AmmoBar.gameObject.SetActive(false);
            }
            else
                m_AmmoBar.gameObject.SetActive(false);

            if (m_CurAmmo <= 0)
                m_IsReloading = true;

            if (m_IsReloading)
            {
                m_CurReload -= Time.deltaTime;
                if (m_CurReload <= 0.0f)
                {
                    m_CurReload = m_ReloadTime;
                    m_IsReloading = false;
                    m_CurAmmo = m_Ammo;
                    m_AmmoBar.SetScale(m_CurAmmo);
                }
            }

            if (m_CurFire < m_FireRate)
                m_CurFire += Time.deltaTime;
            m_CurFire = Mathf.Clamp(m_CurFire, 0.0f, m_FireRate);

            Debug.Log(m_CurAmmo);
        }
	}

    public int GetBonusDamage()
    {
        return (int)((m_Stats.GetDamage() + m_CarrotBonusDMG) * m_CarrotBonusMulti);
    }

    public override void SetEquiped(bool state)
    {
        base.SetEquiped(state);

        if (GetIsEquiped() && !m_Stats)
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
            clone.GetComponent<Projectile>().m_Mode = ProjectileMode.Player;

            int random = Random.Range(m_MinAmount, m_MaxAmount + 1);
            clone.GetComponent<Projectile>().m_OnDestroyNum = random;

            if (m_Carrot)
                clone.GetComponent<Projectile>().m_OnDestroySpawn = m_Carrot;
        }
    }
    void Eat()
    {
        m_Stats.ChangeHealth(m_HealthAmount);
    }
}
