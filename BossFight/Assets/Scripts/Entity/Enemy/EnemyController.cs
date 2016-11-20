using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EntityStats))]
public class EnemyController : MonoBehaviour
{
    //Public vars
    public Transform m_Target;

    //Component vars
    EntityStats m_Stats;
    NavMeshAgent m_Agent;
    Healthbar m_Chargebar;

    //Position vars
    Vector3 m_MoveToPosition;

    //Rotation vars
    Transform m_Rotation;

    //Attack vars
    bool m_CanAttack = true;
    bool m_IsAttack = false;
    float m_AttackTimer = 0.0f;
    GameObject m_AttackObj;
    float m_CurAttackTime = 0.0f;


    void Start ()
    {
        m_Stats = GetComponent<EntityStats>();
        m_Agent = GetComponent<NavMeshAgent>();

        if (transform.FindChild("Chargebar").GetComponent<Healthbar>())
            m_Chargebar = transform.FindChild("Chargebar").GetComponent<Healthbar>();

        m_Agent.updateRotation = false;
        m_Agent.speed = m_Stats.GetMovementSpeed();
        m_Agent.stoppingDistance = m_Stats.GetAttackRange();

        m_Rotation = transform.FindChild("Rotation");

        m_AttackObj = m_Rotation.transform.FindChild("Hit").gameObject;
        if (m_AttackObj)
            m_AttackObj.SetActive(false);

        if (transform.rotation.eulerAngles.x != 90)
            transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

        if (!m_Target)
            SetTarget(GameObject.Find("Player").transform);
        m_MoveToPosition = transform.position;
	}
	
	void Update ()
    {
        if (!m_Stats.GetIsStunned())
        {
            if (!m_Agent.enabled)
                m_Agent.enabled = true;
            TargetUpdate();
        }
        else
        {
            if (m_Agent.enabled)
                m_Agent.enabled = false;

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
            if (IsInAggroRange())
            {
                m_Agent.speed = m_Stats.GetMovementSpeed();
                if (!m_IsAttack)
                {
                    RotateTowards(m_Target);
                    m_Agent.SetDestination(m_Target.position);
                }

                if (IsInAttackRange())
                    AttackUpdate();
                else if (m_IsAttack)
                    AttackUpdate();
                else
                {
                    if (m_Chargebar.gameObject.activeSelf)
                    {
                        m_Chargebar.SetScale(0);

                        m_Chargebar.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (m_Chargebar.gameObject.activeSelf)
                {
                    m_Chargebar.SetScale(0);

                    m_Chargebar.gameObject.SetActive(false);
                }

                MoveToUpdate();
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
                m_IsAttack = true;
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
            m_IsAttack = false;
            m_CurAttackTime = 0.0f;
            m_AttackObj.SetActive(false);
        }

        if (!m_IsAttack)
        {
            if (!m_Chargebar.gameObject.activeSelf)
                m_Chargebar.gameObject.SetActive(true);

            if (m_Chargebar.gameObject.activeSelf)
            {
                float attackTime = m_Stats.GetAttackSpeed() - m_Stats.GetAttackTime();
                m_Chargebar.ChangeScale(Time.deltaTime / attackTime);
            }
        }
        else
        {
            if (m_Chargebar.gameObject.activeSelf)
            {
                m_Chargebar.SetScale(0);

                m_Chargebar.gameObject.SetActive(false);
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
        //m_Rotation.localRotation = Quaternion.Lerp(m_Rotation.localRotation, Quaternion.FromToRotation(transform.position, dir), 1.0f * Time.deltaTime);
        m_Rotation.Rotate(90, 0, 0);
    }

    void MoveToUpdate()
    {
        m_Agent.speed = m_Stats.GetMovementSpeed() / 3.0f;

        if (Vector3.Distance(transform.position, m_MoveToPosition) > 2)
            m_Agent.SetDestination(m_MoveToPosition);
        else
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(new Vector3(Random.Range(SceneController.m_MinX, SceneController.m_MaxX), 0, Random.Range(SceneController.m_MinZ, SceneController.m_MaxZ)), out hit, 10, 1);
            m_MoveToPosition = hit.position;
        }
    }

    public void SetTarget(Transform target)
    {
        m_Target = target;
    }
}
