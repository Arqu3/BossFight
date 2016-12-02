using UnityEngine;
using System.Collections;

public class Sword : Weapon
{
    //Attack vars
    bool m_CanAttack = true;
    float m_AttackTimer = 0.0f;
    GameObject m_AttackObj;
    GameObject m_SpecialAttackObj;
    float m_CurAttackTime = 0.0f;

    //Rotation vars
    Transform m_RotationTransform;

    public override void Start()
    {
        base.Start();

        m_AttackObj = transform.FindChild("Hit").gameObject;
        m_SpecialAttackObj = transform.FindChild("SpecialHit").gameObject;
        m_RotationTransform = GameObject.FindGameObjectWithTag("PlayerRotation").transform;

        if (m_AttackObj)
        {
            m_AttackObj.GetComponentInChildren<AttackScan>().m_Stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
            m_AttackObj.SetActive(false);
        }

        if (m_SpecialAttackObj)
            m_SpecialAttackObj.SetActive(false);
    }

    public override void Attack()
    {
        if (m_CanAttack && !m_IsAttack && !m_IsSpecialAttack)
        {
            m_CanAttack = false;
            if (m_AttackTimer == 0.0f)
            {
                m_AttackObj.SetActive(true);
                m_IsAttack = true;
            }
        }
    }
    public override void AttackUpdate(float attackSpeed, float attackTime)
    {
        if (!m_CanAttack)
        {
            m_AttackTimer += Time.deltaTime;
            if (m_AttackTimer >= attackSpeed)
            {
                m_CanAttack = true;
                m_AttackTimer = 0.0f;
            }
        }

        if (m_AttackObj.activeSelf)
        {
            m_CurAttackTime += Time.deltaTime;
        }

        if (m_CurAttackTime >= attackTime)
        {
            m_IsAttack = false;
            m_CurAttackTime = 0.0f;
            m_AttackObj.SetActive(false);
        }
    }

    public override void SpecialAttack(float charge)
    {
        if (!m_AttackObj.activeSelf && !m_IsAttack)
        {
            if (!GetSpecialButton() && charge > 0.0f)
            {
                m_SpecialAttackObj.SetActive(true);

                m_IsSpecialAttack = true;
            }
        }
    }
    public override void SpecialAttackUpdate(float attackSpeed, float attackTime)
    {
        if (m_SpecialAttackObj.activeSelf)
            m_CurAttackTime += Time.deltaTime;

        if (m_CurAttackTime >= attackTime)
        {
            m_IsSpecialAttack = false;
            m_CurAttackTime = 0.0f;
            m_SpecialAttackObj.SetActive(false);
        }
    }

    public override void SetEquiped(bool state)
    {
        base.SetEquiped(state);

        if (GetIsEquiped())
        {
            m_AttackObj.transform.SetParent(m_RotationTransform);
            m_AttackObj.transform.localScale = new Vector3(1, 1, 1);
            m_AttackObj.transform.localPosition = new Vector3(0, 0, 0);

            m_SpecialAttackObj.transform.SetParent(m_RotationTransform);
            m_SpecialAttackObj.transform.localScale = new Vector3(1, 1, 1);
            m_SpecialAttackObj.transform.localPosition = new Vector3(0, 0, 0);
            m_SpecialAttackObj.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            m_AttackObj.transform.parent = transform;
            m_SpecialAttackObj.transform.parent = transform;
            m_AttackObj.SetActive(false);
            m_SpecialAttackObj.SetActive(false);
            m_CurAttackTime = 0.0f;
            m_IsAttack = false;
            m_IsSpecialAttack = false;
        }
    }

    public override bool AttackObjActive()
    {
        return m_AttackObj.activeSelf;
    }
    public override bool SpecialObjActive()
    {
        return m_SpecialAttackObj.activeSelf;
    }
    //public override bool GetSpecialButton()
    //{
    //    return !Input.GetMouseButton(1);
    //}

}
