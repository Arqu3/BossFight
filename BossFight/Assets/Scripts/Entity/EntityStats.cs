using UnityEngine;
using System.Collections;

public class EntityStats : MonoBehaviour
{
    //Public vars
    public int m_MaxHealth = 100;
    public float m_HealthMulti = 1.0f;
    public int m_MinDamage = 5;
    public int m_MaxDamage = 10;
    public float m_DamageMulti = 1.0f;
    public float m_MovementSpeed = 8.0f;
    public float m_MovementMulti = 1.0f;
    public float m_AttackSpeed = 1.0f;
    public float m_AttackSpeedMulti = 1.0f;
    public float m_AggroRange = 10.0f;
    public float m_AttackRange = 2.0f;
    public float m_KnockbackForce = 3.0f;
    public float m_KnockbackMulti = 1.0f;
    public bool m_CanBeKnockedback = true;
    public float m_DamagedTimer = 0.2f;
    public bool m_CanDie = true;
    public bool m_CanTakeDamage = true;
    public bool m_CanBeStunned = true;
    public float m_StunAmount = 1.0f;
    public float m_StunReduction = 0.0f;
    public float m_MovementIdleTime = 3.0f;

    //Health vars
    int m_CurHealth;
    Color m_Color;
    float m_CurDMGTimer = 0.0f;
    bool m_IsDamaged = false;
    SpriteRenderer m_Renderer;

    //Component vars
    Healthbar m_Healthbar;

    //Charge vars
    float m_CurrentCharge = 0.0f;

    //Stun vars
    bool m_IsStunned = false;
    float m_CurStunDur = 0.0f;
    float m_StunDuration = 0.0f;

    //Idle move vars
    bool m_IsIdle = false;

    void Awake()
    {
        m_CurHealth = m_MaxHealth;
    }

    void Start()
    {
        m_MinDamage = Mathf.Clamp(m_MinDamage, 0, m_MaxDamage);

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
        StunnedUpdate();
    }

    void ColorUpdate()
    {
        //Toggle color when damage is taken
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

    //Damage vars
    public void SetDamage(int val)
    {
        m_MaxDamage = val;
    }
    public void AddMinDamage(int val)
    {
        m_MinDamage += val;
    }
    public int GetMinDamage()
    {
        return (int)(m_MinDamage * m_DamageMulti); 
    }
    public void AddMaxDamage(int val)
    {
        m_MaxDamage += val;
    }
    public int GetMaxDamage()
    {
        return (int)(m_MaxDamage * m_DamageMulti);
    }
    public int GetDamage()
    {
        m_MaxDamage = Mathf.Clamp(m_MaxDamage, 0, 10000);
        m_MinDamage = Mathf.Clamp(m_MinDamage, 0, m_MaxDamage);
        int damage = (int)(Random.Range(m_MinDamage, m_MaxDamage + 1) * m_DamageMulti);
        //Debug.Log(damage);
        return damage;
    }

    //Health functions
    public void ChangeHealth(int val)
    {
        //Changes health, clamps it from 0 to maxvalue
        if (m_CurHealth > 0 && m_CurHealth <= m_MaxHealth)
        {
            m_IsDamaged = true;
            m_Renderer.color = Color.red;

            int temp = m_CurHealth + val;
            bool lost = false;
            if (temp < m_CurHealth)
                lost = true;

            //If entity took damage and cantakedamage is true, change health
            if (lost && m_CanTakeDamage)
            {
                m_CurHealth += val;
                if (m_Healthbar)
                    m_Healthbar.ChangeScale(val);
            }
            //If value was positive, add health
            else if (!lost)
            {
                m_CurHealth += val;
                if (m_Healthbar)
                    m_Healthbar.ChangeScale(val);
            }

            //Clamp health
            m_CurHealth = Mathf.Clamp(m_CurHealth, 0, GetMaxHealth());

            //What happens when player dies
            if (m_CurHealth < 1 && GetCanDie())
            {
                if (gameObject.tag == "Player")
                    Debug.Log("Player is dead!");
            }
            //Debug.Log("Entity " + gameObject.name + " has " + m_CurHealth + " health left");
        }
    }
    public void SetHealth(int val)
    {
        m_CurHealth = val;
        m_Healthbar.SetScale(val);
    }
    public int GetHealth()
    {
        m_CurHealth = Mathf.Clamp(m_CurHealth, 0, GetMaxHealth());
        return m_CurHealth;
    }
    public int GetMaxHealth()
    {
        return (int)(m_MaxHealth * m_HealthMulti);
    }
    public void AddMaxHealth(int value)
    {
        m_MaxHealth += value;
        if (m_CurHealth > m_MaxHealth)
            m_CurHealth = m_MaxHealth;

        m_Healthbar.ChangeScaleFactor(GetMaxHealth());
        m_Healthbar.SetScale(GetHealth() / GetMaxHealth());
        m_Healthbar.ChangeScale(GetHealth());
    }

    //Movementspeed functions
    public float GetMovementSpeed()
    {
        return m_MovementSpeed * m_MovementMulti;
    }
    public void SetMovementSpeed(float value)
    {
        m_MovementSpeed = value;
    }
    public void AddMovementSpeed(float value)
    {
        m_MovementSpeed += value;
    }

    //Idle functions
    public bool GetIdle()
    {
        return m_IsIdle;
    }
    public void SetIdle(bool state)
    {
        m_IsIdle = state;
    }
    public float GetIdleTime()
    {
        return m_MovementIdleTime;
    }
    public void SetIdleTime(float time)
    {
        m_MovementIdleTime = time;
    }


    //Attackspeed functions
    public float GetAttackSpeed()
    {
        return m_AttackSpeed * m_AttackSpeedMulti;
    }
    public float GetAttackTime()
    {
        return GetAttackSpeed() * 0.85f;
    }
    public void SetAttackSpeed(float value)
    {
        m_AttackSpeed = value;
    }
    public void AddAttackSpeed(float value)
    {
        m_AttackSpeed += value;
    }

    //Aggro functions
    public float GetAggroRange()
    {
        return m_AggroRange;
    }
    public float GetAttackRange()
    {
        return m_AttackRange;
    }

    //Knockback functions
    public float GetKnockbackForce()
    {
        return m_KnockbackForce * m_KnockbackMulti;
    }
    public void SetKnockbackForce(float value)
    {
        m_KnockbackForce = value;
    }
    public void AddKnockbackForce(float value)
    {
        m_KnockbackForce += value;
    }
    public bool GetCanBeKnockedback()
    {
        return m_CanBeKnockedback;
    }
    public void ToggleKnockback()
    {
        m_CanBeKnockedback = !m_CanBeKnockedback;
    }
    public void SetKnockback(bool state)
    {
        m_CanBeKnockedback = state;
    }

    //Can die functions
    public bool GetCanDie()
    {
        return m_CanDie;
    }
    public void SetCanDie(bool state)
    {
        m_CanDie = state;
    }

    //Take damage functions
    public bool GetCanTakeDMG()
    {
        return m_CanTakeDamage;
    }
    public void SetCanTakeDMG(bool state)
    {
        m_CanTakeDamage = state;
    }

    //Charge functions
    public float GetCurrentCharge()
    {
        return m_CurrentCharge;
    }
    public void SetCurrentCharge(float value)
    {
        m_CurrentCharge = value;
    }

    //Stun functions
    public bool GetCanBeStunned()
    {
        return m_CanBeStunned;
    }
    public bool GetIsStunned()
    {
        return m_IsStunned;
    }
    public float GetStunDuration()
    {
        return m_CurStunDur;
    }
    public void SetStunned(float duration)
    {
        if (m_CanBeStunned)
        {
            m_StunReduction = Mathf.Clamp01(m_StunReduction);
            duration = duration - (duration * m_StunReduction);
            duration = Mathf.Clamp(duration, 0.0f, 100.0f);
            m_IsStunned = true;
            m_StunDuration = duration;
        }

    }
    public float GetStunAmount()
    {
        return m_StunAmount;
    }
    public void SetStunAmount(float value)
    {
        m_StunAmount = value;
    }
    void StunnedUpdate()
    {
        if (m_IsStunned)
        {
            if (m_CurStunDur < m_StunDuration)
            {
                m_CurStunDur += Time.deltaTime;
                m_Renderer.color = Color.black;
            }
            else
            {
                m_Renderer.color = m_Color;
                m_IsStunned = false;
                m_CurStunDur = 0.0f;
                m_StunDuration = 0.0f;
            }
        }
    }

    //Multipliers functions
    public void SetHealthMulti(float value)
    {
        m_HealthMulti = value;
        m_Healthbar.ChangeScaleFactor(GetMaxHealth());
        m_Healthbar.ChangeScale(GetHealth());
    }
    public void AddHealthMulti(float value)
    {
        m_HealthMulti += value;
        m_Healthbar.ChangeScaleFactor(GetMaxHealth());
        m_Healthbar.SetScale(GetHealth() / GetMaxHealth());
        m_Healthbar.ChangeScale(GetHealth());
    }

    public void SetDamageMulti(float value)
    {
        m_DamageMulti = value;
    }
    public void AddDamageMulti(float value)
    {
        m_DamageMulti += value;
    }

    public void SetMovementMulti(float value)
    {
        m_MovementMulti = value;
    }
    public void AddMovementMulti(float value)
    {
        m_MovementMulti += value;
    }

    public void SetAttackSpeedMulti(float value)
    {
        m_AttackSpeedMulti = value;
    }
    public void AddAttackSpeedMulti(float value)
    {
        m_AttackSpeedMulti += value;
    }

    public void SetKnockbackMulti(float value)
    {
        m_KnockbackMulti = value;
    }
    public void AddKnockbackMulti(float value)
    {
        m_KnockbackMulti += value;
    }
}
