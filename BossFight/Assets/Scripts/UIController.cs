﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Component vars
    PlayerController m_Player;
    Text m_HookText;
    Color m_HookColor = Color.green;

	void Start ()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_HookText = GameObject.Find("HookCDText").GetComponent<Text>();
	}
	
	void Update ()
    {
        TextUpdate();
	}

    void TextUpdate()
    {
        if (m_HookText)
        {
            m_HookText.color = m_HookColor;
            if (m_Player.GetIsHookReady())
            {
                m_HookColor = Color.green;
                m_HookText.text = "Hook Ready";
            }
            else
            {
                m_HookColor = Color.red;
                m_HookText.text = m_Player.GetCooldowns()[0].ToString("F1");
            }
        }
    }
}