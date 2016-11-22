using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(EntityStats), typeof(NavMeshAgent))]
public class Entity : MonoBehaviour
{
    //Public vars
    public float m_TurnSpeed = 10.0f;

    //Component vars
    Rigidbody m_Rigidbody;
    EntityStats m_Stats;
    NavMeshAgent m_Agent;
    Healthbar m_Chargebar;

    //Chargebar vars
    float m_CurCharge = 0.0f;
    bool m_IsCharging = false;

	public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Stats = GetComponent<EntityStats>();
        m_Agent = GetComponent<NavMeshAgent>();

        m_Agent.updateRotation = false;
        m_Agent.speed = GetStats().GetMovementSpeed();
        m_Agent.stoppingDistance = GetStats().GetAttackRange();

        m_Chargebar = transform.FindChild("Chargebar").GetComponent<Healthbar>();

        if (m_Chargebar)
        {
            m_Chargebar.SetScale(0);
            m_Chargebar.gameObject.SetActive(false);
        }
        else
            Debug.Log("Entity " + gameObject.name + " could not find chargebar!");

        transform.eulerAngles = new Vector3(90.0f, 0, 0);
    }

    public virtual void Update()
    {
	}

    public virtual void MoveUpdate()
    {
    }

    public virtual void RotationUpdate(Transform myTransform, Transform towards)
    {
        Quaternion rot = Quaternion.LookRotation(towards.position - transform.position, Vector3.up);
        rot.z = 0.0f;

        Quaternion rot1 = new Quaternion(0.0f, 0.0f, rot.y, -rot.w);

        myTransform.localRotation = Quaternion.Lerp(myTransform.localRotation, rot1, Time.deltaTime * m_TurnSpeed);
    }

    public virtual void AttackUpdate()
    {
    }

    public virtual void ChargeUpdate(bool trueState, float maxValue)
    {
        if (m_Chargebar)
        {
            if (trueState)
            {
                m_Chargebar.gameObject.SetActive(true);
                m_Chargebar.ChangeScale(Time.deltaTime / maxValue);
                m_CurCharge += Time.deltaTime;
                m_CurCharge = Mathf.Clamp(m_CurCharge, 0.0f, maxValue);
                m_IsCharging = true;
            }
            else
            {
                m_IsCharging = false;
                m_CurCharge = 0.0f;
                m_Chargebar.SetScale(0);
                m_Chargebar.gameObject.SetActive(false);
            }
        }
    }
    public virtual float GetCurrentCharge()
    {
        return m_CurCharge;
    }
    public virtual bool GetIsCharging()
    {
        return m_IsCharging;
    }
    public virtual Healthbar GetChargebar()
    {
        return m_Chargebar;
    }

    public Rigidbody GetRigidbody()
    {
        return m_Rigidbody;
    }
    public EntityStats GetStats()
    {
        return m_Stats;
    }
    public NavMeshAgent GetAgent()
    {
        return m_Agent;
    }
}
