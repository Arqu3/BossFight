using UnityEngine;
using System.Collections;

public enum Mode
{
    Enemy,
    Player
};

public class Healthbar : MonoBehaviour
{
    //Public vars
    public Mode m_Mode = Mode.Enemy;

    //Bar vars
    GameObject m_Slider;
    GameObject m_Background;

    //Component vars
    EntityStats m_EntityStats;
    float m_ScaleFactor;

	void Start ()
    {
        m_Slider = transform.FindChild("Slider").gameObject;
        m_Background = transform.FindChild("Background").gameObject;

        m_Slider.transform.localScale = m_Background.transform.localScale;

        m_EntityStats = transform.parent.GetComponent<EntityStats>();

        if (m_EntityStats)
        {
            m_ScaleFactor = m_Background.transform.localScale.x / m_EntityStats.GetHealth();
            if (transform.parent.tag == "Enemy")
                m_Mode = Mode.Enemy;
            else if (transform.parent.tag == "Player")
                m_Mode = Mode.Player;
        }
    }

    void Update()
    {

    }

    public void ChangeScale(float value)
    {
        float f = value * m_ScaleFactor;

        if (m_Slider.transform.localPosition.x > -m_Background.transform.localScale.x / 2)
        {
            m_Slider.transform.localPosition += new Vector3(f / 2f, 0, 0);
            m_Slider.transform.localScale += new Vector3(f, 0, 0);
            m_Slider.transform.localScale = new Vector3(Mathf.Clamp(m_Slider.transform.localScale.x, 0f, m_Background.transform.localScale.x), 
                m_Slider.transform.localScale.y, m_Slider.transform.localScale.z);
        }
    }
}
