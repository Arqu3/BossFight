using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    //Public vars
    public Transform m_Target;

    //Component vars
    EntityStats m_Stats;
    NavMeshAgent m_Agent;

    //Rotation vars
    Transform m_Rotation;

    //Attack vars
    bool m_CanAttack = true;
    bool m_IsAttackActive = false;
    float m_AttackTimer = 0.0f;
    GameObject m_AttackObj;
    float m_CurAttackTime = 0.0f;


    void Start ()
    {
        m_Stats = GetComponent<EntityStats>();
        m_Agent = GetComponent<NavMeshAgent>();

        m_Agent.updateRotation = false;
        m_Agent.speed = m_Stats.GetMovementSpeed();
        m_Agent.stoppingDistance = m_Stats.GetAttackRange();

        m_Rotation = transform.FindChild("Rotation");

        m_AttackObj = m_Rotation.transform.FindChild("Hit").gameObject;
        if (m_AttackObj)
            m_AttackObj.SetActive(false);
	}
	
	void Update ()
    {
        TargetUpdate();
    }

    void TargetUpdate()
    {
        if (m_Target)
        {
            if (IsInAggroRange())
            {
                m_Agent.SetDestination(m_Target.position);
                RotateTowards(m_Target);

                if (IsInAttackRange())
                {
                    AttackUpdate();
                }
                else if (m_IsAttackActive)
                {
                    AttackUpdate();
                }
            }
        }
    }

    void AttackUpdate()
    {
        if (m_CanAttack)
        {
            m_CanAttack = false;
            if (m_AttackTimer == 0.0f)
            {
                m_IsAttackActive = true;
                m_AttackObj.SetActive(true);
            }
        }

        if (!m_CanAttack)
        {
            m_AttackTimer += Time.deltaTime;
            if (m_AttackTimer >= m_Stats.GetAttackSpeed())
            {
                m_CanAttack = true;
                m_AttackTimer = 0.0f;
            }
        }

        if (m_AttackObj.activeSelf)
            m_CurAttackTime += Time.deltaTime;

        if (m_CurAttackTime >= m_Stats.GetAttackTime())
        {
            m_IsAttackActive = false;
            m_CurAttackTime = 0.0f;
            m_AttackObj.SetActive(false);
        }
    }

    void ResetAttack()
    {

    }

    bool IsInAggroRange()
    {
        return Vector3.Distance(transform.position, m_Target.position) <= m_Stats.GetAggroRange();
    }
    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, m_Target.position) <= m_Stats.GetAttackRange();
    }

    void RotateTowards(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        m_Rotation.rotation = rot;
        m_Rotation.Rotate(90, 0, 0);
    }
}
