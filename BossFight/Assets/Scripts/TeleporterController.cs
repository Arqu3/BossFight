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
        if (col.transform.tag == "Player")
        {
            col.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            col.gameObject.transform.position = new Vector3(m_ToNode.position.x, col.gameObject.transform.position.y, m_ToNode.position.z);
            col.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
