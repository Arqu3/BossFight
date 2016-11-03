using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{

    Transform m_FollowTransform;
    Vector3 m_Position;

	// Use this for initialization
	void Start()
    {
        m_FollowTransform = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update()
    {
        m_Position = m_FollowTransform.position;
        transform.position = new Vector3(m_Position.x, 10, m_Position.z);
	}
}
