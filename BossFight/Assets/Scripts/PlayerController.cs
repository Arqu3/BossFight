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
    
    //Component vars
	Rigidbody2D m_Rigidbody;

    //Movement vars
    Vector2 m_Velocity;

    //Rotation vars
    Transform m_PointerTransform;

    //Dash vars
    bool m_IsDashing = false;
    bool m_IsDashCD = false;
    float m_DashTimer = 0.0f;
    float m_CurDashCD;
    Vector2 m_DashDir;

    //Hook vars
    bool m_IsHooked = false;
    bool m_IsHookCD = false;
    float m_CurHookCD;
    Vector2 m_HookDir;
    Vector2 m_HookHitPos;
    RaycastHit2D m_HookHit;
    public LayerMask m_HookMask;

    //Invincible vars
    public bool m_IsInvincible = false;
    float m_InvTimer = 0.0f;

	//Use this for initialization
	void Start()
	{
        m_CurDashCD = m_DashCD;
        m_CurHookCD = m_HookCD;

        m_State = MovementState.Idle;

		m_Rigidbody = GetComponent<Rigidbody2D>();
        m_PointerTransform = GameObject.FindGameObjectWithTag("PlayerRotation").transform;
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
    }

    void RotationUpdate()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(m_PointerTransform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_PointerTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void InvincibleUpdate()
    {
        if (m_IsInvincible)
        {
            m_InvTimer -= Time.deltaTime;
            if (m_InvTimer < 0.0f)
            {
                m_InvTimer = 0.0f;
                m_IsInvincible = false;
            }
        }
    }

    void MovementUpdate()
    {
        if (IsMoving())
        {
            m_Velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * m_MovementSpeed, 1f), Mathf.Lerp(0, Input.GetAxis("Vertical") * m_MovementSpeed, 1f));
            m_Rigidbody.velocity = Vector2.ClampMagnitude(m_Velocity, m_MovementSpeed);
        }
        //Debug.Log(m_Rigidbody.velocity.magnitude);
    }

    void DashUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !m_IsDashCD)
        {
            m_IsDashCD = true;
            m_IsDashing = true;
            m_DashDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
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
        if (Input.GetKeyDown(KeyCode.Q) && !m_IsHookCD)
        {
            m_IsHookCD = true;
            Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.DrawRay(transform.position, (new Vector3(temp.x, temp.y, 0) - transform.position).normalized * m_HookDistance, Color.red);
            m_HookHit = Physics2D.Raycast(transform.position, (new Vector3(temp.x, temp.y, 0) - transform.position).normalized, m_HookDistance, m_HookMask);

            if (m_HookHit.collider != null)
            {
                if (m_HookHit.collider.gameObject.tag != "Player")
                {
                    SetInvincible(0.5f);
                    m_HookHitPos = m_HookHit.point;
                    m_HookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    m_IsHooked = true;
                }
            }
        }

        if (m_IsHooked)
        {
            if (Vector2.Distance(transform.position, m_HookHitPos) > m_BreakHookDist)
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
            m_Rigidbody.velocity = Vector2.zero;
            m_Velocity = Vector2.zero;
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
}
