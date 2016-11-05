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
    EnemyStats m_EnemyStats;
    float m_ScaleFactor;

	void Start ()
    {
        m_Slider = transform.FindChild("Slider").gameObject;
        m_Background = transform.FindChild("Background").gameObject;

        m_Slider.transform.localScale = m_Background.transform.localScale;

        m_EnemyStats = transform.parent.GetComponent<EnemyStats>();

        if (m_EnemyStats)
        {
            m_ScaleFactor = m_Background.transform.localScale.x / m_EnemyStats.GetHealth();
            m_Mode = Mode.Enemy;
        }
        else if (transform.parent.tag == "Player")
        {
            m_ScaleFactor = m_Background.transform.localScale.x / PlayerController.GetHealth();
            m_Mode = Mode.Player;
        }
        else
            Debug.Log("Healthbar could not find stats!");
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
