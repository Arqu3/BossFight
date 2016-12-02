using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{
    //Position vars
    Transform m_FollowTransform;
    Vector3 m_Position;

	void Start()
    {
        m_FollowTransform = GameObject.Find("Player").transform;
	}
	
	void Update()
    {
        //Follow player position X and Z, Y is constant
        m_Position = m_FollowTransform.position;
        transform.position = new Vector3(m_Position.x, 10, m_Position.z);
	}
}
