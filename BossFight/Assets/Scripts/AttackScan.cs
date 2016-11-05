using UnityEngine;
using System.Collections;

public class AttackScan : MonoBehaviour
{
    Transform m_Parent;
    float m_Speed;
    //float m_Timer;

    void Awake()
    {
        m_Parent = transform.parent;
    }

    void OnEnable()
    {
        //transform.localPosition = transform.parent.GetComponent<BoxCollider>().center - 
        //  new Vector3(0, transform.parent.GetComponent<BoxCollider>().size.y, transform.parent.GetComponent<BoxCollider>().size.z) / 2;
        //m_Timer = 0;
        m_Parent.localRotation = Quaternion.Euler(90, 0, 60);
        m_Speed = -((360 - m_Parent.localRotation.eulerAngles.y) * 2);
    }

    void Update()
    {
        //transform.localPosition += new Vector3(0, transform.parent.GetComponent<BoxCollider>().size.y / PlayerController.m_BoxTimer, 0) * Time.deltaTime;
        //m_Timer -= (60 / PlayerController.m_BoxTimer) * Time.deltaTime;
        m_Parent.transform.Rotate(0, 0, (m_Speed / PlayerController.m_AttackTime) * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (col.gameObject.GetComponent<EnemyStats>())
            {
                col.gameObject.GetComponent<EnemyStats>().ChangeHealth(-PlayerController.GetDamage());
            }
        }
    }
}
