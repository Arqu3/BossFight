using UnityEngine;
using System.Collections;

public class AttackScan : MonoBehaviour
{
    //Component vars
    EntityStats m_Stats;

    //Rotation vars
    Transform m_Parent;
    float m_Speed;
    //float m_Timer;

    void Awake()
    {
        m_Parent = transform.parent;
        m_Stats = m_Parent.transform.parent.transform.parent.GetComponent<EntityStats>();
    }

    void OnEnable()
    {
        //transform.localPosition = transform.parent.GetComponent<BoxCollider>().center - 
        //  new Vector3(0, transform.parent.GetComponent<BoxCollider>().size.y, transform.parent.GetComponent<BoxCollider>().size.z) / 2;
        //m_Timer = 0;
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

    void Update()
    {
        //transform.localPosition += new Vector3(0, transform.parent.GetComponent<BoxCollider>().size.y / PlayerController.m_BoxTimer, 0) * Time.deltaTime;
        //m_Timer -= (60 / PlayerController.m_BoxTimer) * Time.deltaTime;
        m_Parent.Rotate(0, 0, (m_Speed / m_Stats.GetAttackTime()) * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<EntityStats>())
        {
            if (col.gameObject.GetComponent<EntityStats>() != m_Stats)
            {
                if (m_Stats.gameObject.tag != col.gameObject.tag)
                {
                    col.gameObject.GetComponent<EntityStats>().ChangeHealth(-m_Stats.GetDamage());

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
