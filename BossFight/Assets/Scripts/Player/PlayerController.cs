using UnityEngine;
using System.Collections;

public enum MovementState
{
    Idle,
    Moving,
    Dashing,
    GrapplingHook
}

public class PlayerController : Entity
{
    //Public vars
    public MovementState m_State;

    public float m_DashCD = 1.0f;
    public float m_DashTime = 0.5f;
    public float m_DashSpeed = 20.0f;

    public float m_HookCD = 1.5f;
    public float m_HookDistance = 20.0f;
    public float m_HookSpeed = 15.0f;
    public float m_RopeTravelSpeed = 50.0f;
    public float m_BreakHookDist = 2.0f;

    public float m_MaxCharge = 3.0f;

    public Weapon m_Weapon;

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
    bool m_HasHookHit = false;
    bool m_IsHookUsed = false;
    float m_CurHookCD;
    Vector3 m_HookDir;
    Vector3 m_HookHitPos;
    RaycastHit m_HookHit;
    GameObject m_HookRope;
    public LayerMask m_LayerMask;

    //Invincible vars
    float m_InvTimer = 0.0f;

    //Attack vars
    bool m_IsAttack = false;
    bool m_IsSpecialAttack = false;

    //Charge vars
    float m_SpecialCharge = 0.0f;

    public override void Start()
	{
        base.Start();

        m_CurDashCD = m_DashCD;
        m_CurHookCD = m_HookCD;

        m_State = MovementState.Idle;

        m_PointerTransform = transform.FindChild("Rotation");
        m_HookRope = m_PointerTransform.FindChild("HookRope").gameObject;
        m_HookRope.SetActive(false);
        GetAgent().updateRotation = false;

        m_CurSpeed = GetStats().GetMovementSpeed();
	}
	
	public override void Update()
	{
        CheckState();

        MovementUpdate();

        DashUpdate();

        HookUpdate();

        RotationUpdate(m_PointerTransform, Vector3.zero);

        InvincibleUpdate();

        AttackUpdate();
    }

    public override void RotationUpdate(Transform myTransform, Vector3 towards)
    {
        //Rotate towards mouseposition
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(myTransform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (!m_IsAttack && !m_IsSpecialAttack && !m_HasHookHit)
            myTransform.rotation = Quaternion.AngleAxis(angle, Vector3.down);

        //m_PointerTransform.Rotate(90, 0, 0);
    }

    //Timer for invincibility
    void InvincibleUpdate()
    {
        if (!GetStats().GetCanTakeDMG())
        {
            m_InvTimer -= Time.deltaTime;
            if (m_InvTimer <= 0.0f)
            {
                m_InvTimer = 0.0f;
                GetStats().SetCanTakeDMG(true);
            }
        }
    }

    //How and when to move
    void MovementUpdate()
    {
        if (IsMoving() && !m_IsSpecialAttack && !m_IsHookUsed)
        {
            m_CurSpeed = Mathf.Clamp(m_CurSpeed, 0.0f, GetStats().GetMovementSpeed());
            m_Velocity = new Vector3(Mathf.Lerp(0, Input.GetAxis("Horizontal") * m_CurSpeed, 1f), 0, Mathf.Lerp(0, Input.GetAxis("Vertical") * m_CurSpeed, 1f));
            GetRigidbody().velocity = Vector3.ClampMagnitude(m_Velocity, m_CurSpeed);
        }
        else
            GetRigidbody().velocity = Vector3.zero;
        //Debug.Log(m_Rigidbody.velocity.magnitude);
    }

    //Dash in movement-direction when pressing space
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

            GetRigidbody().velocity = m_DashDir.normalized * m_DashSpeed;
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

    //Shoot a hook in mouse-direction when pressing Q
    void HookUpdate()
    {
        //Vector3 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.DrawRay(transform.position, (new Vector3(temp1.x, 0, temp1.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized * m_HookDistance);
        if (Input.GetKeyDown(KeyCode.Q) && !m_IsHookCD && !m_IsAttack && !GetIsCharging())
        {
            m_IsHookCD = true;
            Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Physics.Raycast(transform.position, (new Vector3(temp.x, 0, temp.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized, out m_HookHit, m_HookDistance, m_LayerMask))
            {
                if (m_HookHit.collider.gameObject.tag != "Player")
                {
                    SetInvincible(0.5f);
                    m_HookHitPos = m_HookHit.point;
                    m_HookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    m_HookDir.y = 0;
                    m_HasHookHit = true;
                    m_IsHookUsed = true;

                    //Apply stun if enemy is hit
                    if (m_HookHit.collider.gameObject.tag == "Enemy")
                        m_HookHit.collider.gameObject.GetComponent<EntityStats>().SetStunned(GetStats().GetStunAmount());
                }
            }
        }

        if (m_HasHookHit)
        {
            m_HookRope.SetActive(true);
            float dist = Vector3.Distance(transform.position, m_HookHitPos);
            if (m_HookRope.transform.localScale.x < dist)
            {
                m_HookRope.transform.localScale += new Vector3(m_RopeTravelSpeed, 0.0f, 0.0f) * Time.deltaTime;
                m_HookRope.transform.localPosition = new Vector3(m_HookRope.transform.localScale.x / 2f, 0, 0);
            }
            else
            {
                m_IsHooked = true;
                m_IsHookUsed = false;
            }

            if (m_IsHooked)
            {
                if (Vector3.Distance(transform.position, m_HookHitPos) > m_BreakHookDist)
                {
                    GetRigidbody().velocity = m_HookDir.normalized * m_HookSpeed;

                    m_HookRope.transform.localScale = new Vector3(dist, 0.1f, 0.1f);
                    m_HookRope.transform.localPosition = new Vector3(dist / 2f, 0, 0);

                }
                else
                {
                    m_IsHooked = false;
                    m_HookRope.SetActive(false);
                    m_HasHookHit = false;
                }
            }
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
        else if (m_IsHooked || m_HasHookHit)
        {
            SetState(MovementState.GrapplingHook);
        }
        else if (IsMoving())
        {
            SetState(MovementState.Moving);
        }
        else if (m_Velocity.magnitude < 2)
        {
            GetRigidbody().velocity = Vector3.zero;
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

    //Check if player is currently in a non-friendly area
    public bool IsInCombatArea()
    {
        //Debug.DrawRay(transform.position, Vector3.down * 5.0f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5.0f, m_LayerMask))
        {
            if (hit.transform.root.gameObject.tag == "CombatArea")
                return true;
        }
        return false;
    }

    void SetInvincible(float time)
    {
        if (GetStats().GetCanTakeDMG())
        {
            GetStats().SetCanTakeDMG(false);
            m_InvTimer = time;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public override void AttackUpdate()
    {
        if (GetWeapon())
        {
            GetWeapon().AttackUpdate(GetStats().GetAttackSpeed(), GetStats().GetAttackTime());
            GetWeapon().SpecialAttackUpdate(GetStats().GetAttackSpeed(), GetStats().GetAttackTime());

            if (GetWeapon().AttackObjActive())
                m_CurSpeed = GetStats().GetMovementSpeed() / 3f;
            else
                m_CurSpeed = GetStats().GetMovementSpeed();

            m_IsAttack = GetWeapon().GetIsAttack();
            m_IsSpecialAttack = GetWeapon().GetIsSpecial();
                
            if (GetWeapon().GetBasicButton() && !GetIsCharging())
                GetWeapon().Attack();
            else if (!GetWeapon().GetSpecialButton())
                GetWeapon().SpecialAttack(GetCurrentCharge());

            if (GetWeapon().GetCanCharge())
                ChargeUpdate(Input.GetMouseButton(1), m_MaxCharge);
            else
                ChargeUpdate(false, 0.0f);                
        }
        else
        {
            m_IsAttack = false;
            m_IsSpecialAttack = false;
        }
    }

    public override void ChargeUpdate(bool trueState, float maxValue)
    {
        if (!GetWeapon().AttackObjActive() && !m_IsAttack)
        {
            if (trueState)
            {
                m_SpecialCharge = GetCurrentCharge();
                m_CurSpeed = GetStats().GetMovementSpeed() / 2.0f;
            }

            if (!trueState && GetCurrentCharge() > 0.0f)
            {
                GetStats().SetCurrentCharge(m_SpecialCharge);
                m_CurSpeed = GetStats().GetMovementSpeed();
            }

            base.ChargeUpdate(trueState, maxValue);
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        m_Weapon = newWeapon;
    }

    public Weapon GetWeapon()
    {
        return m_Weapon;
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
