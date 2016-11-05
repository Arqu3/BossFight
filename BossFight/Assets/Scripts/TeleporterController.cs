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
        if (col.transform.parent.tag == "Player")
        {
            col.transform.parent.GetComponent<NavMeshAgent>().enabled = false;
            col.transform.parent.position = new Vector3(m_ToNode.position.x, col.transform.parent.position.y, m_ToNode.position.z);
            col.transform.parent.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
