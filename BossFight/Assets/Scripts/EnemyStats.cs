using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour
{
    //Public vars
    public int m_Health = 100;
    public int m_Damage = 10;
    public float m_MovementSpeed = 8.0f;
    public float m_AttackSpeed = 1.0f;
    public float m_AttackTime = 0.4f;
    public float m_AggroRange = 10.0f;
    public float m_AttackRange = 2.0f;

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

            if (m_Health < 1)
            {
                //What happens when entity dies
                //Destroy(this.gameObject);
            }

            if (m_Healthbar)
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

    public float GetMovementSpeed()
    {
        return m_MovementSpeed;
    }

    public float GetAttackSpeed()
    {
        return m_AttackSpeed;
    }
    public float GetAttackTime()
    {
        return m_AttackTime;
    }

    public float GetAggroRange()
    {
        return m_AggroRange;
    }
    public float GetAttackRange()
    {
        return m_AttackRange;
    }
}
