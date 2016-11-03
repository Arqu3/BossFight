using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//[System.Serializable]
//public struct Requirements
//{
//    public enum State
//    {
//        None,
//        GreaterThan,
//        LessThan,
//        EqualTo
//    }
//    public enum Require
//    {
//        PlayerDistance,
//        PlayerHealth,
//        Health,
//        Time
//    }

//    public Require m_Require;
//    public State m_State;
//    public float m_Value;

//    public PlayerController m_Player;
//    public BossController m_Boss;

//    public bool CheckRequirement()
//    {
//        switch (m_Require)
//        {
//            case Require.Health:
//                switch (m_State)
//                {
//                    case State.EqualTo:
//                        break;

//                    case State.GreaterThan:
//                        break;

//                    case State.LessThan:
//                        break;

//                    case State.None:
//                        break;
//                }
//                break;

//            case Require.PlayerDistance:
//                switch (m_State)
//                {
//                    case State.EqualTo:
//                        return Vector2.Distance(m_Player.GetPosition(), m_Boss.GetPosition()) == m_Value;
//                    //break;

//                    case State.GreaterThan:
//                        return Vector2.Distance(m_Player.GetPosition(), m_Boss.GetPosition()) > m_Value;
//                    //break;

//                    case State.LessThan:
//                        return Vector2.Distance(m_Player.GetPosition(), m_Boss.GetPosition()) < m_Value;
//                    //break;

//                    case State.None:
//                        break;
//                }
//                break;

//            case Require.PlayerHealth:
//                switch (m_State)
//                {
//                    case State.EqualTo:
//                        break;

//                    case State.GreaterThan:
//                        break;

//                    case State.LessThan:
//                        break;

//                    case State.None:
//                        break;
//                }
//                break;

//            case Require.Time:
//                switch (m_State)
//                {
//                    case State.EqualTo:
//                        break;

//                    case State.GreaterThan:
//                        break;

//                    case State.LessThan:
//                        break;

//                    case State.None:
//                        break;
//                }
//                break;
//        }
//        return false;
//    }
//}

//[System.Serializable]
//public struct Action
//{
//    public string m_Name;
//    public int m_Priority;
//    public float m_Cooldown;

//    public List<Requirements> m_Requirements;

//    public Action(int prio, float cd)
//    {
//        m_Priority = prio;
//        m_Cooldown = cd;
//        m_Name = "";
//        m_Requirements = new List<Requirements>();
//    }

//    public void Update()
//    {
//        for (int i = 0; i < m_Requirements.Count; i++)
//        {
//            m_Requirements[i].CheckRequirement();
//        }
//    }
//}

public enum ActionState
{
    None,
    Moving,
    Charging,
    Attacking,
}

public class BossController : MonoBehaviour
{
    //Public vars
    //public List<Action> m_Actions = new List<Action>();
    public ActionState m_State = ActionState.None;
    public int m_Health = 500;
    public int m_Damage = 20;
    public float m_MovementSpeed = 8.0f;
    public float m_TurnSpeed = 4.0f;
    public float m_AttackRange = 5.0f;
    public float m_ChargeRange = 20.0f;

    //Component vars
	PlayerController m_Player;



	// Use this for initialization
	void Start()
	{
		m_Player = GameObject.Find("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update()
    {
        // for (int i = 0; i < m_Actions.Count; i++)
        //{
        //    m_Actions[i].Update();
        //}
	}

    void CheckState()
    {

    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public float CheckDistance(float value)
    {
        return Vector2.Distance(m_Player.transform.position, transform.position);
    }
}
