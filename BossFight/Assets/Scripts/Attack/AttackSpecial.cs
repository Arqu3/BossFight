using UnityEngine;
using System.Collections;

public class AttackSpecial : MonoBehaviour
{
    //Component vars
    EntityStats m_Stats;

    void Awake()
    {
        m_Stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<EntityStats>())
        {
            if (col.gameObject.GetComponent<EntityStats>() != m_Stats)
            {
                //Can only deal damage to non-friendly entities
                if (m_Stats.gameObject.tag != col.gameObject.tag)
                {
                    //Deal damage
                    col.gameObject.GetComponent<EntityStats>().ChangeHealth((int)(-m_Stats.GetDamage() * (2.5f * m_Stats.GetCurrentCharge())));
                    //Debug.Log((int)(-m_Stats.GetDamage() * (2.5f * m_Stats.GetCurrentCharge())));

                    //Apply knockback
                    if (col.gameObject.GetComponent<EntityStats>().GetCanBeKnockedback())
                    {
                        Vector3 force = (col.gameObject.transform.position - m_Stats.gameObject.transform.position).normalized * m_Stats.GetKnockbackForce() * 5.0f * m_Stats.GetCurrentCharge();
                        col.gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
