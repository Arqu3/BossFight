using UnityEngine;
using System.Collections;

public class TeleporterController : MonoBehaviour
{
    //Public vars
    public bool m_CanTeleport = true;

    //Teleport vars
    Transform m_ToNode;

	void Start ()
    {
        m_ToNode = transform.FindChild("Node");
	}

    public bool GetCanTeleport()
    {
        return m_CanTeleport;
    }
    public void SetCanTeleport(bool state)
    {
        m_CanTeleport = state;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player" && m_CanTeleport)
        {
            col.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            col.gameObject.transform.position = new Vector3(m_ToNode.position.x, col.gameObject.transform.position.y, m_ToNode.position.z);
            col.gameObject.GetComponent<NavMeshAgent>().enabled = true;

            Vector3 temp = transform.position;
            transform.position = m_ToNode.position;
            m_ToNode.position = temp;
            m_CanTeleport = false;
        }
    }
}
