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
    public float m_MovementSpeed = 10.0f;

    public float m_DashCD = 1.0f;
    public float m_DashTime = 0.5f;
    public float m_DashSpeed = 20.0f;

    public float m_HookCD = 1.5f;
    public float m_HookDistance = 20.0f;
    public float m_HookSpeed = 15.0f;
    public float m_BreakHookDist = 2.0f;

    public int m_StartHP = 100;
    public int m_StartDMG = 15;

    public float m_AttackSpeed = 1.0f;
        
    //Component vars
	Rigidbody m_Rigidbody;
    NavMeshAgent m_Agent;

    //Movement vars
    Vector3 m_Velocity;

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
    public bool m_IsInvincible = false;
    float m_InvTimer = 0.0f;

    //Stat vars
    static int m_Health;
    static int m_Damage;

    //Healthbar vars
    Healthbar m_Healthbar;

    //Attack vars
    bool m_CanAttack = true;
    float m_AttackTimer = 0.0f;
    GameObject m_AttackBox;
    public static float m_BoxTimer = 0.2f;
    float m_CurBoxTimer = 0.0f;
    bool m_IsAttackActive = false;

    void Awake()
    {
        m_Health = m_StartHP;
        m_Damage = m_StartDMG;
    }

	//Use this for initialization
	void Start()
	{
        m_CurDashCD = m_DashCD;
        m_CurHookCD = m_HookCD;

        m_State = MovementState.Idle;

		m_Rigidbody = GetComponent<Rigidbody>();
        m_PointerTransform = transform.FindChild("Rotation");
        m_AttackBox = m_PointerTransform.FindChild("HitCollider").gameObject;
        m_Healthbar = GetComponentInChildren<Healthbar>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updateRotation = false;
        //m_Agent.updatePosition = false;

        if (m_AttackBox)
            m_AttackBox.SetActive(false);
	}
	
	//Update is called once per frame
	void Update()
	{
        CheckState();

        MovementUpdate();

        DashUpdate();

        HookUpdate();

        RotationUpdate();

        InvincibleUpdate();

        AttackUpdate();
    }

    void RotationUpdate()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(m_PointerTransform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (!m_IsAttackActive)
            m_PointerTransform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
    }

    void InvincibleUpdate()
    {
        if (m_IsInvincible)
        {
            m_InvTimer -= Time.deltaTime;
            if (m_InvTimer <= 0.0f)
            {
                m_InvTimer = 0.0f;
                m_IsInvincible = false;
            }
        }
    }

    void MovementUpdate()
    {
        if (IsMoving() && !m_IsAttackActive)
        {
            m_Velocity = new Vector3(Mathf.Lerp(0, Input.GetAxis("Horizontal") * m_MovementSpeed, 1f), 0, Mathf.Lerp(0, Input.GetAxis("Vertical") * m_MovementSpeed, 1f));
            //m_Velocity = new Vector3(Input.GetAxis("Horizontal") * m_MovementSpeed, 0, Input.GetAxis("Vertical") * m_MovementSpeed);
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Velocity, m_MovementSpeed);
        }
        else
            m_Rigidbody.velocity = Vector3.zero;
        //Debug.Log(m_Rigidbody.velocity.magnitude);
    }

    void DashUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !m_IsDashCD)
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
        if (Input.GetKeyDown(KeyCode.Q) && !m_IsHookCD && !m_IsAttackActive)
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

    public bool IsInvincible()
    {
        return m_IsInvincible;
    }

    void SetInvincible(float time)
    {
        if (!m_IsInvincible)
        {
            m_IsInvincible = true;
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
            if (m_CanAttack)
            {
                m_CanAttack = false;
                if (m_AttackTimer == 0.0f)
                {
                    m_AttackBox.SetActive(true);
                    m_IsAttackActive = true;
                }
            }
        }

        if (!m_CanAttack)
        {
            m_AttackTimer += Time.deltaTime;
            if (m_AttackTimer >= m_AttackSpeed)
            {
                m_CanAttack = true;
                m_AttackTimer = 0.0f;
            }
        }

        if (m_AttackBox.activeSelf)
            m_CurBoxTimer += Time.deltaTime;

        if (m_CurBoxTimer >= m_BoxTimer)
        {
            m_IsAttackActive = false;
            m_CurBoxTimer = 0.0f;
            m_AttackBox.SetActive(false);
        }
    }

    public static int GetHealth()
    {
        return m_Health;
    }

    public static int GetDamage()
    {
        return m_Damage;
    }

    public void ChangeHealth(int value)
    {
        if (m_Health > 1 && m_Health <= m_StartHP)
        {
            m_Health += value;
            if (m_Health > m_StartHP)
                m_Health = m_StartHP;

            if (m_Health < 1)
            {
                //Player is dead;
            }

            m_Healthbar.ChangeScale(value);
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
