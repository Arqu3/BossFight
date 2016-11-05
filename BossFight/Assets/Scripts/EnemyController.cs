using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    //Public vars
    public Transform m_Target;

    //Component vars
    EnemyStats m_Stats;
    NavMeshAgent m_Agent;

    //Rotation vars
    Transform m_Rotation;

	void Start ()
    {
        m_Stats = GetComponent<EnemyStats>();
        m_Agent = GetComponent<NavMeshAgent>();

        m_Agent.updateRotation = false;
        m_Agent.speed = m_Stats.GetMovementSpeed();
        m_Agent.stoppingDistance = m_Stats.GetAttackRange();

        m_Rotation = transform.FindChild("Rotation");
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
            }
        }
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
