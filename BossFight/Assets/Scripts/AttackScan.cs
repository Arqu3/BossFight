using UnityEngine;
using System.Collections;

public class AttackScan : MonoBehaviour
{
    bool m_IsActive = false;
    float m_Distance;

    void OnEnable()
    {
        m_IsActive = true;
        transform.localPosition = transform.parent.GetComponent<BoxCollider>().center - 
            new Vector3(0, transform.parent.GetComponent<BoxCollider>().size.y, transform.parent.GetComponent<BoxCollider>().size.z) / 2;

        //Vector3 v = transform.parent.GetComponent<BoxCollider>().size.y * transform.parent.lossyScale;
        //m_Distance = v.y - transform.position.y;
        //Debug.Log(m_Distance);
    }

    void Update()
    {
        if (m_IsActive)
            transform.localPosition += new Vector3(0, transform.parent.GetComponent<BoxCollider>().size.y / PlayerController.m_BoxTimer, 0) * Time.deltaTime;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Dealt: " + PlayerController.GetDamage() + " damage");
        }
    }
}
