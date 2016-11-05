using UnityEngine;
using System.Collections;

public class TeleporterController : MonoBehaviour
{
    Transform m_ToNode;

	void Start ()
    {
        m_ToNode = transform.FindChild("Node");
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.root.tag == "Player")
        {
            col.transform.root.position = new Vector3(m_ToNode.position.x, col.transform.root.position.y, m_ToNode.position.z);
        }
    }
}
