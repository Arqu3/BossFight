using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public struct Requirements
{
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

    public Require m_Require;
    public State m_State;
    public float m_Value;

    public PlayerController m_Player;
    public BossController m_Boss;

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

[System.Serializable]
public struct Action
{
    public string m_Name;
    public int m_Priority;
    public float m_Cooldown;

    public List<Requirements> m_Requirements;

    public Action(int prio, float cd)
    {
        m_Priority = prio;
        m_Cooldown = cd;
        m_Name = "";
        m_Requirements = new List<Requirements>();
    }

    public void Update()
    {
        for (int i = 0; i < m_Requirements.Count; i++)
        {
            m_Requirements[i].CheckRequirement();
        }
    }
}

public class BossController : MonoBehaviour
{
    public List<Action> m_Actions = new List<Action>();

	PlayerController m_Player;

	// Use this for initialization
	void Start()
	{
		m_Player = GameObject.Find("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update()
    {
         for (int i = 0; i < m_Actions.Count; i++)
        {
            m_Actions[i].Update();
        }
	}

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
