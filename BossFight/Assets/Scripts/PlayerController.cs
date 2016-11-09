using UnityEngine;
using System.Collections;

public enum MovementState
{
    Idle,
    Moving,
    Dashing,
    GrapplingHook
}

public class PlayerController : MonoBehaviour
{
    //Public vars
    public MovementState m_State;

    public float m_DashCD = 1.0f;
    public float m_DashTime = 0.5f;
    public float m_DashSpeed = 20.0f;

    public float m_HookCD = 1.5f;
    public float m_HookDistance = 20.0f;
    public float m_HookSpeed = 15.0f;
    public float m_BreakHookDist = 2.0f;

    public float m_MaxCharge = 3.0f;
                
    //Component vars
	Rigidbody m_Rigidbody;
    NavMeshAgent m_Agent;
    EntityStats m_Stats;
    Healthbar m_Chargebar;

    //Movement vars
    Vector3 m_Velocity;
    float m_CurSpeed;

    //Rotation vars
    Transform m_PointerTransform;

    //Dash vars
    bool m_IsDashing = false;
    bool m_IsDashCD = false;
    float m_DashTimer = 0.0f;
    float m_CurDashCD;
    Vector3 m_DashDir;

    //Hook vars
    bool m_IsHooked = false;
    bool m_IsHookCD = false;
    float m_CurHookCD;
    Vector3 m_HookDir;
    Vector3 m_HookHitPos;
    RaycastHit m_HookHit;
    public LayerMask m_HookMask;

    //Invincible vars
    float m_InvTimer = 0.0f;

    //Attack vars
    bool m_CanAttack = true;
    float m_AttackTimer = 0.0f;
    GameObject m_AttackObj;
    GameObject m_SpecialAttackObj;
    float m_CurAttackTime = 0.0f;
    bool m_IsAttackActive = false;

    //Charge vars
    float m_CurCharge = 0.0f;
    bool m_IsCharging = false;

	void Start()
	{
        m_CurDashCD = m_DashCD;
        m_CurHookCD = m_HookCD;

        m_State = MovementState.Idle;

		m_Rigidbody = GetComponent<Rigidbody>();
        m_PointerTransform = transform.FindChild("Rotation");
        m_AttackObj = m_PointerTransform.FindChild("Hit").gameObject;
        m_SpecialAttackObj = m_PointerTransform.FindChild("SpecialHit").gameObject;
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updateRotation = false;
        m_Stats = GetComponent<EntityStats>();
        m_Chargebar = transform.FindChild("Chargebar").GetComponent<Healthbar>();

        if (m_Chargebar)
        {
            m_Chargebar.SetScale(0);
            m_Chargebar.gameObject.SetActive(false);
        }
        else
            Debug.Log("Player could not find chargebar!");

        if (m_AttackObj)
            m_AttackObj.SetActive(false);

        if (m_SpecialAttackObj)
            m_SpecialAttackObj.SetActive(false);

        m_CurSpeed = m_Stats.GetMovementSpeed();
	}
	
	void Update()
	{
        CheckState();

        MovementUpdate();

        DashUpdate();

        HookUpdate();

        RotationUpdate();

        InvincibleUpdate();

        AttackUpdate();

        ChargeUpdate();
    }

    void RotationUpdate()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(m_PointerTransform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (!m_IsAttackActive)
            m_PointerTransform.rotation = Quaternion.AngleAxis(angle, Vector3.down);

        //m_PointerTransform.Rotate(90, 0, 0);
    }

    void InvincibleUpdate()
    {
        if (!m_Stats.GetCanTakeDMG())
        {
            m_InvTimer -= Time.deltaTime;
            if (m_InvTimer <= 0.0f)
            {
                m_InvTimer = 0.0f;
                m_Stats.SetCanTakeDMG(true);
            }
        }
    }

    void MovementUpdate()
    {
        if (IsMoving() && !m_IsAttackActive)
        {
            m_Velocity = new Vector3(Mathf.Lerp(0, Input.GetAxis("Horizontal") * m_CurSpeed, 1f), 0, Mathf.Lerp(0, Input.GetAxis("Vertical") * m_CurSpeed, 1f));
            //m_Velocity = new Vector3(Input.GetAxis("Horizontal") * m_MovementSpeed, 0, Input.GetAxis("Vertical") * m_MovementSpeed);
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Velocity, m_CurSpeed);
        }
        else
            m_Rigidbody.velocity = Vector3.zero;
        //Debug.Log(m_Rigidbody.velocity.magnitude);
    }

    void DashUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !m_IsDashCD) //&& !m_IsAttackActive && !m_IsCharging)
        {
            m_IsDashCD = true;
            m_IsDashing = true;
            //m_DashDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            //m_DashDir.y = 0;
            m_DashDir = m_Velocity;
            SetInvincible(m_DashTime);
        }

        if (m_IsDashing)
        {
            m_DashTimer += Time.deltaTime;
            if (m_DashTimer > m_DashTime)
            {
                m_DashTimer = 0.0f;
                m_IsDashing = false;
            }

            m_Rigidbody.velocity = m_DashDir.normalized * m_DashSpeed;
        }

        if (m_IsDashCD)
        {
            m_CurDashCD -= Time.deltaTime;
            if (m_CurDashCD < 0.0f)
            {
                m_CurDashCD = m_DashCD;
                m_IsDashCD = false;
            }
        }
    }

    void HookUpdate()
    {
        //Vector3 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.DrawRay(transform.position, (new Vector3(temp1.x, 0, temp1.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized * m_HookDistance);
        if (Input.GetKeyDown(KeyCode.Q) && !m_IsHookCD && !m_IsAttackActive && !m_IsCharging)
        {
            m_IsHookCD = true;
            Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Physics.Raycast(transform.position, (new Vector3(temp.x, 0, temp.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized, out m_HookHit, m_HookDistance, m_HookMask))
            {
                if (m_HookHit.collider.gameObject.tag != "Player")
                {
                    SetInvincible(0.5f);
                    m_HookHitPos = m_HookHit.point;
                    m_HookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    m_HookDir.y = 0;
                    m_IsHooked = true;
                }
            }
        }

        if (m_IsHooked)
        {
            if (Vector3.Distance(transform.position, m_HookHitPos) > m_BreakHookDist)
            {
                m_Rigidbody.velocity = m_HookDir.normalized * m_HookSpeed;
            }
            else
                m_IsHooked = false;
        }

        if (m_IsHookCD)
        {
            m_CurHookCD -= Time.deltaTime;
            if (m_CurHookCD < 0.0f)
            {
                m_CurHookCD = m_HookCD;
                m_IsHookCD = false;
            }
        }
    }

    void SetState(MovementState state)
    {
        m_State = state;
    }

    void CheckState()
    {
        if (m_IsDashing)
        {
            SetState(MovementState.Dashing);
        }
        else if (m_IsHooked)
        {
            SetState(MovementState.GrapplingHook);
        }
        else if (IsMoving())
        {
            SetState(MovementState.Moving);
        }
        else if (m_Velocity.magnitude < 2)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Velocity = Vector3.zero;
            SetState(MovementState.Idle);
        }
    }

    bool IsMoving()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            return true;

        return false;
    }

    public bool IsInCombatArea()
    {
        //Debug.DrawRay(transform.position, Vector3.down * 5.0f);
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 5.0f, m_HookMask);
        if (hit.transform.root.gameObject.tag == "CombatArea")
            return true;
        return false;
    }

    void SetInvincible(float time)
    {
        if (m_Stats.GetCanTakeDMG())
        {
            m_Stats.SetCanTakeDMG(false);
            m_InvTimer = time;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    void AttackUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (m_CanAttack && !m_IsCharging && !m_IsAttackActive)
            {
                m_CanAttack = false;
                if (m_AttackTimer == 0.0f)
                {
                    m_AttackObj.SetActive(true);
                    m_IsAttackActive = true;
                }
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

    void ChargeUpdate()
    {
        if (Input.GetMouseButton(1) && m_CurCharge < m_MaxCharge && !m_AttackObj.activeSelf && !m_IsAttackActive)
        {
            m_Chargebar.gameObject.SetActive(true);
            m_IsCharging = true;
            m_CurCharge += Time.deltaTime;
            m_Chargebar.ChangeScale(Time.deltaTime / m_MaxCharge);

            m_CurSpeed = m_Stats.GetMovementSpeed() / 2;
        }
        else if (Input.GetMouseButtonUp(1) && m_CurCharge > 0.0f)
        {
            if (m_CurCharge > m_MaxCharge)
                m_CurCharge = m_MaxCharge;

            m_Stats.SetCurrentCharge(m_CurCharge);
            m_SpecialAttackObj.SetActive(true);

            m_CurCharge = 0.0f;
            m_IsCharging = false;
            m_Chargebar.SetScale(0);
            m_Chargebar.gameObject.SetActive(false);

            m_IsAttackActive = true;

            m_CurSpeed = m_Stats.GetMovementSpeed();
        }

        if (m_SpecialAttackObj.activeSelf)
            m_CurAttackTime += Time.deltaTime;

        if (m_CurAttackTime >= m_Stats.GetAttackTime())
        {
            m_IsAttackActive = false;
            m_CurAttackTime = 0.0f;
            m_Stats.SetCurrentCharge(0.0f);
            m_SpecialAttackObj.SetActive(false);
        }
    }


    public bool GetIsHookReady()
    {
        return !m_IsHookCD;
    }

    public float[] GetCooldowns()
    {
        return new float[] { m_CurHookCD, m_CurDashCD };
    }
}
