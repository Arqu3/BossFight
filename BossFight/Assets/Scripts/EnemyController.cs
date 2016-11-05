using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    //Public vars
    public Transform m_Target;

    //Component vars
    EnemyStats m_Stats;
    NavMeshAgent m_Agent;

	void Start ()
    {
        m_Stats = GetComponent<EnemyStats>();
        m_Agent = GetComponent<NavMeshAgent>();

        m_Agent.updateRotation = false;
	}
	
	void Update ()
    {
        if (m_Target)
        {
            m_Agent.SetDestination(m_Target.position);

            Vector3 dir = m_Target.position - transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = rot;
            transform.Rotate(90, 0, 0);
        }
    }
}
