using UnityEngine;
using System.Collections;

public class EnemyMelee : Entity
{
    //Public vars
    public Transform m_Target;

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


    public override void Start()
    {
        base.Start();

        m_Rotation = transform.FindChild("Rotation");

        m_AttackObj = m_Rotation.transform.FindChild("Hit").gameObject;
        if (m_AttackObj)
            m_AttackObj.SetActive(false);

        if (!m_Target)
            SetTarget(GameObject.Find("Player").transform);
        m_MoveToPosition = transform.position;
	}
	
	public override void Update()
    {
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
            if (!m_IsAttack)
                RotationUpdate(m_Rotation, m_Target);

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
            if (m_AttackTimer >= GetStats().GetAttackSpeed())
            {
                m_CanAttack = true;
                m_AttackTimer = 0.0f;
            }
        }

        if (m_AttackObj.activeSelf)
            m_CurAttackTime += Time.deltaTime;

        if (m_CurAttackTime >= GetStats().GetAttackTime())
        {
            m_IsAttack = false;
            m_CurAttackTime = 0.0f;
            m_AttackObj.SetActive(false);
        }

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

    public override void MoveUpdate()
    {
        GetAgent().speed = GetStats().GetMovementSpeed() / 3.0f;
        GetAgent().stoppingDistance = 0.5f;

        if (Vector3.Distance(transform.position, m_MoveToPosition) > 2)
            GetAgent().SetDestination(m_MoveToPosition);
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
