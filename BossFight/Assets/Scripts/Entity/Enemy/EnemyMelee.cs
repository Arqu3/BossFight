using UnityEngine;
using System.Collections;

public class EnemyMelee : Entity
{
    //Public vars
    public Transform m_Target;

    //Rotation vars
    Transform m_Rotation;

    //Attack vars
    bool m_CanAttack = true;
    bool m_IsAttack = false;
    float m_AttackTimer = 0.0f;
    GameObject m_AttackObj;
    float m_CurAttackTime = 0.0f;


    public override void Start()
    {
        base.Start();

        //Find rotation, attackobj
        m_Rotation = transform.FindChild("Rotation");

        m_AttackObj = m_Rotation.transform.FindChild("Hit").gameObject;
        if (m_AttackObj)
        {
            m_AttackObj.GetComponentInChildren<AttackScan>().m_Stats = GetStats();
            m_AttackObj.SetActive(false);
        }

        //Set target if none is assigned
        if (!m_Target)
            SetTarget(GameObject.Find("Player").transform);
	}
	
	public override void Update()
    {
        base.Update();
        //Update as normal if not stunned, else disable agent and reset attack
        if (!GetStats().GetIsStunned())
        {
            if (!GetAgent().enabled)
                GetAgent().enabled = true;
            TargetUpdate();
        }
        else
        {
            if (GetAgent().enabled)
                GetAgent().enabled = false;

            if (m_AttackObj.activeSelf)
            {
                m_CanAttack = true;
                m_AttackTimer = 0.0f;
                m_CurAttackTime = 0.0f;
                m_IsAttack = false;
                m_AttackObj.SetActive(false);
            }
        }
    }

    void TargetUpdate()
    {
        if (m_Target)
        {
            //Rotate towards current target if not attacking
            if (!m_IsAttack)
            {
                if (IsInAggroRange())
                    RotationUpdate(m_Rotation, m_Target.position);
                else
                    RotationUpdate(m_Rotation, GetMovetoPosition());
            }

            //Move towards player if is in aggro range and not attacking
            if (IsInAggroRange())
            {
                GetAgent().stoppingDistance = GetStats().GetAttackRange();
                GetAgent().speed = GetStats().GetMovementSpeed();
                if (!m_IsAttack)
                {
                    GetAgent().SetDestination(m_Target.position);
                }

                if (IsInAttackRange())
                    AttackUpdate();
                else if (m_IsAttack)
                    AttackUpdate();
            }
            else
            {
                MoveUpdate();
            }
        }

        //When to disable chargebar
        if ((!m_IsAttack && !IsInAttackRange()) || !IsInAggroRange())
        {
            if (GetChargebar().gameObject.activeSelf)
            {
                GetChargebar().SetScale(0);

                GetChargebar().gameObject.SetActive(false);
            }
        }
    }

    public override void AttackUpdate()
    {
        //Toggle attack
        if (m_CanAttack)
        {
            m_CanAttack = false;
            if (m_AttackTimer == 0.0f)
            {
                m_IsAttack = true;
                m_AttackObj.SetActive(true);
            }
        }

        //Attack timer
        if (!m_CanAttack)
        {
            m_AttackTimer += Time.deltaTime;
            if (m_AttackTimer >= GetStats().GetAttackSpeed())
            {
                m_CanAttack = true;
                m_AttackTimer = 0.0f;
            }
        }

        if (m_AttackObj.activeSelf)
            m_CurAttackTime += Time.deltaTime;

        //End of attack
        if (m_CurAttackTime >= GetStats().GetAttackTime())
        {
            m_IsAttack = false;
            m_CurAttackTime = 0.0f;
            m_AttackObj.SetActive(false);
        }

        //Get time between attacks and set scale on chargebar
        float attackTime = GetStats().GetAttackSpeed() - GetStats().GetAttackTime();
        ChargeUpdate(!m_IsAttack, attackTime);
    }

    bool IsInAggroRange()
    {
        return Vector3.Distance(transform.position, m_Target.position) <= GetStats().GetAggroRange();
    }
    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, m_Target.position) <= GetStats().GetAttackRange();
    }

    public void SetTarget(Transform target)
    {
        m_Target = target;
    }
}
