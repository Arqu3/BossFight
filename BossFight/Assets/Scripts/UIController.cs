using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Component vars
    PlayerController m_Player;
    Text m_HookText;
    Healthbar m_PlayerHealthbar;

    //Text vars
    Color m_HookColor = Color.green;

	void Start ()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_PlayerHealthbar = m_Player.GetComponentInChildren<Healthbar>();

        float offsetX = 0.5f;
        float offsetY = 0.5f;

        m_PlayerHealthbar.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 9));
        m_PlayerHealthbar.transform.position += new Vector3(m_PlayerHealthbar.transform.FindChild("Background").GetComponent<SpriteRenderer>().bounds.size.x / 2 + offsetX, 
            0, -(m_PlayerHealthbar.transform.FindChild("Background").GetComponent<SpriteRenderer>().bounds.size.y / 2 + offsetY));

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
