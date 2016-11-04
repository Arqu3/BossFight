using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour
{
    //Public vars
    public int m_Health = 100;
    public int m_Damage = 10;

    //Health vars
    int m_StartHealth;

    //Component vars
    Healthbar m_Healthbar;

    void Start()
    {
        m_Healthbar = transform.FindChild("Healthbar").GetComponent<Healthbar>();

        if (!m_Healthbar)
            Debug.Log("Entity does not have healthbar!");

        m_StartHealth = m_Health;
    }

    void Update()
    {

    }

    public void SetDamage(int val)
    {
        m_Damage = val;
    }
    public int GetDamage()
    {
        return m_Damage;
    }

    public void ChangeHealth(int val)
    {
        if (m_Health > 0 && m_Health <= m_StartHealth)
        {
            m_Health += val;
            if (m_Health > m_StartHealth)
                m_Health = m_StartHealth;

            m_Healthbar.ChangeScale(val);
        }
    }
    public void SetHealth(int val)
    {
        m_Health = val;
    }
    public int GetHealth()
    {
        return m_Health;
    }
}
