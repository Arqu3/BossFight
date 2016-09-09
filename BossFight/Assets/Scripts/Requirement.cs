using UnityEngine;
using System.Collections;

public enum State
{
    None,
    GreaterThan,
    LessThan,
    EqualTo
}
public enum Require
{
    PlayerDistance,
    PlayerHealth,
    Health,
    Time
}

public class Requirement : MonoBehaviour
{
    //Public vars
    public Require m_Require;
    public State m_State;
    public float m_Value;

    //Component vars
    PlayerController m_Player;
    BossController m_Boss;

    // Use this for initialization
    void Start ()
    {
        m_Player = GameObject.Find("Player").GetComponent<PlayerController>();
        m_Boss = GameObject.Find("Boss").GetComponent<BossController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public bool CheckRequirement()
    {
        switch (m_Require)
        {
            case Require.Health:
                switch (m_State)
                {
                    case State.EqualTo:
                        break;

                    case State.GreaterThan:
                        break;

                    case State.LessThan:
                        break;

                    case State.None:
                        break;
                }
                break;

            case Require.PlayerDistance:
                switch (m_State)
                {
                    case State.EqualTo:
                        return Vector2.Distance(m_Player.GetPosition(), m_Boss.GetPosition()) == m_Value;
                        //break;

                    case State.GreaterThan:
                        return Vector2.Distance(m_Player.GetPosition(), m_Boss.GetPosition()) > m_Value;
                        //break;

                    case State.LessThan:
                        return Vector2.Distance(m_Player.GetPosition(), m_Boss.GetPosition()) < m_Value;
                        //break;

                    case State.None:
                        break;
                }
                break;

            case Require.PlayerHealth:
                switch (m_State)
                {
                    case State.EqualTo:
                        break;

                    case State.GreaterThan:
                        break;

                    case State.LessThan:
                        break;

                    case State.None:
                        break;
                }
                break;

            case Require.Time:
                switch (m_State)
                {
                    case State.EqualTo:
                        break;

                    case State.GreaterThan:
                        break;

                    case State.LessThan:
                        break;

                    case State.None:
                        break;
                }
                break;
        }
        return false;
    }
}
