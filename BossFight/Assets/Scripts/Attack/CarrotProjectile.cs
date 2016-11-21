using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class CarrotProjectile : Projectile
{
	public override void Start()
    {
        base.Start();

        m_TotalDamage = GameObject.FindGameObjectWithTag("PlayerItems").GetComponentInChildren<CarrotRifle>().GetBonusDamage();

        m_Direction = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
        m_Direction.y = 0.0f;

        transform.position += m_Direction.normalized * 1.5f;
    }
}
