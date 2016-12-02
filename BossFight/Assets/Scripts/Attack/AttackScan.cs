using UnityEngine;
using System.Collections;

public class AttackScan : MonoBehaviour
{
    //Component vars
    public EntityStats m_Stats;

    //Rotation vars
    Transform m_Parent;
    float m_Speed;

    void Awake()
    {
        m_Parent = transform.parent;
    }

    void OnEnable()
    {
        if (m_Stats)
        {
            //Set different localrotation and speed depending on parent
            if (m_Stats.gameObject.tag == "Player")
            {
                m_Parent.localRotation = Quaternion.Euler(90, 0, 60);
                m_Speed = -((360 - m_Parent.localRotation.eulerAngles.y) * 2);
            }
            else if (m_Stats.gameObject.tag == "Enemy")
            {
                m_Parent.localRotation = Quaternion.Euler(0, 0, 30);
                float f = 90 - m_Parent.localRotation.eulerAngles.z;
                float ff = 90 + f;
                float fff = ff - m_Parent.localRotation.eulerAngles.z;
                m_Speed = fff;
            }
        }
        //else
        //    Debug.Log("Attackscan has no stats!");
    }

    void Update()
    {
        //Rotate from start to finish with speed depending on attacktime
        m_Parent.Rotate(0, 0, (m_Speed / m_Stats.GetAttackTime()) * Time.deltaTime);
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
                    col.gameObject.GetComponent<EntityStats>().ChangeHealth(-m_Stats.GetDamage());

                    //Apply knockback
                    if (col.gameObject.GetComponent<EntityStats>().GetCanBeKnockedback())
                    {
                        Vector3 force = (col.gameObject.transform.position - m_Stats.gameObject.transform.position).normalized * m_Stats.GetKnockbackForce();
                        col.gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
