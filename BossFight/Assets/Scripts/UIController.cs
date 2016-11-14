using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Component vars
    PlayerController m_Player;
    Text m_HookText;
    Text m_RemainingText;
    Text m_EssenceRText;
    Text m_EssenceBText;
    Text m_EssenceGText;
    Healthbar m_PlayerHealthbar;
    SceneController m_SceneController;
    Canvas m_Canvas;

    //Text vars
    Color m_HookColor = Color.green;

    //Character panel vars
    GameObject m_CharacterPanel;
    bool m_IsCharacterPanel = false;
    public static float m_Scalefactor = 0;

    void Awake()
    {
        m_Canvas = GetComponentInChildren<Canvas>();
        if (m_Canvas)
            m_Scalefactor = m_Canvas.scaleFactor;

        m_CharacterPanel = transform.FindChild("Canvas").FindChild("CharacterPanel").gameObject;

        if (m_CharacterPanel)
        {
            m_EssenceRText = m_CharacterPanel.transform.FindChild("EssenceText1").GetComponent<Text>();
            m_EssenceBText = m_CharacterPanel.transform.FindChild("EssenceText2").GetComponent<Text>();
            m_EssenceGText = m_CharacterPanel.transform.FindChild("EssenceText3").GetComponent<Text>();

            m_CharacterPanel.SetActive(false);
        }
    }

	void Start ()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_PlayerHealthbar = m_Player.transform.FindChild("Healthbar").GetComponent<Healthbar>();
        m_SceneController = GameObject.Find("SceneHandler").GetComponent<SceneController>();

        float offsetX = 0.5f;
        float offsetY = 0.5f;

        m_PlayerHealthbar.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 9));
        m_PlayerHealthbar.transform.position += new Vector3(m_PlayerHealthbar.transform.FindChild("Background").GetComponent<SpriteRenderer>().bounds.size.x / 2 + offsetX, 
            0, -(m_PlayerHealthbar.transform.FindChild("Background").GetComponent<SpriteRenderer>().bounds.size.y / 2 + offsetY));

        m_HookText = transform.FindChild("Canvas").FindChild("HookCDText").GetComponent<Text>();
        m_RemainingText = transform.FindChild("Canvas").FindChild("RemainingText").GetComponent<Text>();
    }
	
	void Update ()
    {
        TextUpdate();

        if (Input.GetKeyDown(KeyCode.I))
            ToggleCharacterPanel();
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

        if (m_RemainingText)
        {
            if (m_Player.IsInCombatArea())
            {
                m_RemainingText.text = "Enemies remaining: " + m_SceneController.GetRemainingEnemies();
            }
            else
                m_RemainingText.text = "";
        }

        if (m_IsCharacterPanel)
        {
            //if (m_EssenceRText)

        }
    }

    void ToggleCharacterPanel()
    {
        m_IsCharacterPanel = !m_IsCharacterPanel;
        m_CharacterPanel.SetActive(m_IsCharacterPanel);
    }

    public Transform GetCharacterPanel()
    {
        return m_CharacterPanel.transform;
    }
}
