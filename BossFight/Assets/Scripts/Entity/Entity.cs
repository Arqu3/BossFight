using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(EntityStats), typeof(NavMeshAgent))]
public class Entity : MonoBehaviour
{
    //Public vars
    public float m_TurnSpeed = 10.0f;
    public float m_IdleMoveDistance = 6.0f;

    //Component vars
    Rigidbody m_Rigidbody;
    EntityStats m_Stats;
    NavMeshAgent m_Agent;
    Healthbar m_Chargebar;

    //Chargebar vars
    float m_CurCharge = 0.0f;
    bool m_IsCharging = false;

    //Movement vars
    Vector3 m_MovetoPosition;
    Vector3 m_HitPosition;

    //Idle vars
    float m_CurIdle = 0.0f;

    public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Stats = GetComponent<EntityStats>();
        m_Agent = GetComponent<NavMeshAgent>();

        //Set navmeshagent values
        m_Agent.updateRotation = false;
        m_Agent.speed = GetStats().GetMovementSpeed();
        m_Agent.stoppingDistance = GetStats().GetAttackRange();

        //Find and set chargebar
        m_Chargebar = transform.FindChild("Chargebar").GetComponent<Healthbar>();

        if (m_Chargebar)
        {
            m_Chargebar.SetScale(0);
            m_Chargebar.gameObject.SetActive(false);
        }
        else
            Debug.Log("Entity " + gameObject.name + " could not find chargebar!");

        //Set rotation
        transform.eulerAngles = new Vector3(90.0f, 0, 0);
        m_MovetoPosition = transform.position;
    }

    public virtual void Update()
    {
        //Draw movetoposition
        Debug.DrawRay(m_HitPosition, Vector3.up * 2.5f, Color.green);
    }

    public virtual void MoveUpdate()
    {
        //Set "walking" speed if not moving towards attack target
        GetAgent().speed = GetStats().GetMovementSpeed() / 3.0f;
        GetAgent().stoppingDistance = 1.2f;

        //Set destination as long as distance is greater than a given value
        if (!m_Stats.GetIdle())
        {
            if (Vector3.Distance(transform.position, m_MovetoPosition) > 2)
                GetAgent().SetDestination(m_MovetoPosition);
            else
                m_Stats.SetIdle(true);
        }
        else
        {
            m_CurIdle += Time.deltaTime;
            if (m_CurIdle >= m_Stats.GetIdleTime())
            {
                //Find random point on local navmesh and set movetoposition to that point
                NavMeshHit hit;
                Vector3 randomPoint = transform.position;
                //NavMesh.SamplePosition(new Vector3(Random.Range(SceneController.m_MinX, SceneController.m_MaxX), 0, Random.Range(SceneController.m_MinZ, SceneController.m_MaxZ)), out hit, 10, 1);
                float randomX = Random.Range(-1.0f, 1.0f);
                float randomZ = Random.Range(-1.0f, 1.0f);

                randomPoint += new Vector3(randomX, 0.0f, randomZ).normalized * m_IdleMoveDistance;

                bool canMove = NavMesh.SamplePosition(randomPoint, out hit, m_IdleMoveDistance, NavMesh.AllAreas);

                if (canMove)
                    m_MovetoPosition = hit.position;

                m_HitPosition = hit.position;

                m_CurIdle = 0.0f;
                m_Stats.SetIdle(false);
            }
        }
    }

    public virtual void RotationUpdate(Transform myTransform, Vector3 towards)
    {
        //Rotate towards a given target, had to translate rotation from y to z
        Quaternion rot = Quaternion.LookRotation(towards - transform.position, Vector3.up);
        rot.z = 0.0f;

        Quaternion rot1 = new Quaternion(0.0f, 0.0f, rot.y, -rot.w);

        myTransform.localRotation = Quaternion.Lerp(myTransform.localRotation, rot1, Time.deltaTime * m_TurnSpeed);
    }

    public virtual void AttackUpdate()
    {
    }

    public virtual void ChargeUpdate(bool trueState, float maxValue)
    {
        //Activates and sets scale on chargebar if truestate is set
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
    public Vector3 GetMovetoPosition()
    {
        return m_MovetoPosition;
    }
}
