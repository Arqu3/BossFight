using UnityEngine;
using System.Collections;

public class DummyController : MonoBehaviour
{
    //Component vars
    EntityStats m_Stats;

	void Start ()
    {
        m_Stats = GetComponent<EntityStats>();
	}
	
	void Update ()
    {
        //Set current health to full if below 1
        if (m_Stats.GetHealth() < 1)
            m_Stats.SetHealth(m_Stats.GetMaxHealth());
	}
}
