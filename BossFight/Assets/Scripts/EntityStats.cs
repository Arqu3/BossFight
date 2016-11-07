﻿using UnityEngine;
using System.Collections;

public class EntityStats : MonoBehaviour
{
    //Public vars
    public int m_Health = 100;
    public int m_Damage = 10;
    public float m_MovementSpeed = 8.0f;
    public float m_AttackSpeed = 1.0f;
    public float m_AttackTime = 0.4f;
    public float m_AggroRange = 10.0f;
    public float m_AttackRange = 2.0f;
    public float m_DamagedTimer = 0.2f;

    //Health vars
    int m_CurHealth;
    Color m_Color;
    float m_CurDMGTimer = 0.0f;
    bool m_IsDamaged = false;
    SpriteRenderer m_Renderer;

    //Component vars
    Healthbar m_Healthbar;

    void Awake()
    {
        m_CurHealth = m_Health;
    }

    void Start()
    {
        m_Healthbar = transform.FindChild("Healthbar").GetComponent<Healthbar>();

        if (!m_Healthbar)
            Debug.Log("Entity does not have healthbar!");

        if (GetComponent<SpriteRenderer>())
            m_Renderer = GetComponent<SpriteRenderer>();
        else if (GetComponentInChildren<SpriteRenderer>())
            m_Renderer = GetComponentInChildren<SpriteRenderer>();
        else
            Debug.Log("Entity " + gameObject.name + " has no known spriterenderer!");

        m_Color = m_Renderer.color;
    }

    void Update()
    {
        ColorUpdate();
    }

    public void SetDamage(int val)
    {
        m_Damage = val;
    }
    public int GetDamage()
    {
        return m_Damage;
    }

    void ColorUpdate()
    {
        if (m_IsDamaged)
        {
            m_CurDMGTimer += Time.deltaTime;
            if (m_CurDMGTimer >= m_DamagedTimer)
            {
                m_CurDMGTimer = 0.0f;
                m_IsDamaged = false;
                m_Renderer.color = m_Color;
            }
        }
    }

    public void ChangeHealth(int val)
    {
        if (m_CurHealth > 0 && m_CurHealth <= m_Health)
        {
            m_IsDamaged = true;
            m_Renderer.color = Color.red;

            m_CurHealth += val;
            if (m_CurHealth > m_Health)
                m_CurHealth = m_Health;

            if (m_CurHealth < 1)
            {
                //What happens when entity dies
                //Destroy(this.gameObject);
            }

            if (m_Healthbar)
                m_Healthbar.ChangeScale(val);

            Debug.Log("Entity " + gameObject.name + " has " + m_CurHealth + " health left");
        }
    }
    public void SetHealth(int val)
    {
        m_CurHealth = val;
    }
    public int GetHealth()
    {
        return m_CurHealth;
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