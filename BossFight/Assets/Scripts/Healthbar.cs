using UnityEngine;
using System.Collections;

public enum Mode
{
    Health,
    Charge
};

public class Healthbar : MonoBehaviour
{
    //Public vars
    public Mode m_Mode = Mode.Health;

    //Bar vars
    GameObject m_Slider;
    GameObject m_Background;

    //Component vars
    EntityStats m_EntityStats;
    float m_ScaleFactor;

	void Start()
    {
        m_Slider = transform.FindChild("Slider").gameObject;
        m_Background = transform.FindChild("Background").gameObject;

        m_Slider.transform.localScale = m_Background.transform.localScale;

        m_EntityStats = transform.parent.GetComponent<EntityStats>();

        if (m_EntityStats)
        {
            if (m_Mode.Equals(Mode.Health))
                m_ScaleFactor = m_Background.transform.localScale.x / m_EntityStats.GetMaxHealth();
            else
            {
                m_ScaleFactor = m_Background.transform.localScale.x;
                SetScale(0);
            }
        }
    }

    public void ChangeScale(float value)
    {
        float f = value * m_ScaleFactor;

        m_Slider.transform.localPosition += new Vector3(f / 2f, 0, 0);
        m_Slider.transform.localScale += new Vector3(f, 0, 0);
        m_Slider.transform.localScale = new Vector3(Mathf.Clamp(m_Slider.transform.localScale.x, 0f, m_Background.transform.localScale.x),
            m_Slider.transform.localScale.y, m_Slider.transform.localScale.z);
        m_Slider.transform.localPosition = new Vector3(Mathf.Clamp(m_Slider.transform.localPosition.x, -m_Background.transform.localScale.x / 2, 0), 0, 0);
    }

    public void SetScale(float value)
    {
        float f = value * m_ScaleFactor;
        if (f == 0)
            m_Slider.transform.localPosition = new Vector3(-m_Background.transform.localScale.x / 2, 0, 0);
        else
            m_Slider.transform.localPosition = new Vector3(f / 2f, 0, 0);
        m_Slider.transform.localScale = new Vector3(f, m_Background.transform.localScale.y, m_Background.transform.localScale.z);
        m_Slider.transform.localScale = new Vector3(Mathf.Clamp(m_Slider.transform.localScale.x, 0f, m_Background.transform.localScale.x),
            m_Slider.transform.localScale.y, m_Slider.transform.localScale.z);
        m_Slider.transform.localPosition = new Vector3(Mathf.Clamp(m_Slider.transform.localPosition.x, -m_Background.transform.localScale.x / 2, 0), 0, 0);
    }

    public void ChangeScaleFactor(float value)
    {
        if (value != 0.0f)
            m_ScaleFactor = m_Background.transform.localScale.x / value;
    }
}
