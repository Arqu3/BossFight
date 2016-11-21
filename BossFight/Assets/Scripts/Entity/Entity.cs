using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
    //Public vars
    Rigidbody m_Rigidbody;

	public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update()
    {
	}

    public virtual void Move()
    {
    }

    public virtual void Attack()
    {
    }
}
