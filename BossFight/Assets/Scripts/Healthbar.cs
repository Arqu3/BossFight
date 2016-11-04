using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour
{
    //Bar vars
    GameObject m_Slider;
    GameObject m_Background;

    //Component vars
    EnemyStats m_Stats;
    float m_ScaleFactor;

	void Start ()
    {
        m_Slider = transform.FindChild("Slider").gameObject;
        m_Background = transform.FindChild("Background").gameObject;

        m_Slider.transform.localScale = m_Background.transform.localScale;

        m_Stats = transform.parent.GetComponent<EnemyStats>();

        if (!m_Stats)
            Debug.Log("Healthbar could not find stats!");
        else
            m_ScaleFactor = m_Background.transform.localScale.x / m_Stats.GetHealth();
    }
	
	void Update ()
    {
        //if (m_Slider.transform.localPosition.x > -m_Background.transform.localScale.x / 2)
        //{
        //    m_Slider.transform.localPosition -= new Vector3(1f / 2f, 0, 0) * Time.deltaTime;
        //    m_Slider.transform.localScale -= new Vector3(1f, 0, 0) * Time.deltaTime;
        //}
        //ChangeHealth(-1 * Time.deltaTime);
	}

    public void ChangeScale(float value)
    {
        float f = value * m_ScaleFactor;

        if (m_Slider.transform.localPosition.x > -m_Background.transform.localScale.x / 2)
        {
            m_Slider.transform.localPosition += new Vector3(f / 2f, 0, 0);
            m_Slider.transform.localScale += new Vector3(f, 0, 0);
            m_Slider.transform.localScale = new Vector3(Mathf.Clamp(m_Slider.transform.localScale.x, 0f, m_Background.transform.localScale.x), m_Slider.transform.localScale.y, m_Slider.transform.localScale.z);
        }
    }
}
