using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody))]
public class Essence : MonoBehaviour
{
    //Public vars
    public EssenceType m_Type = EssenceType.Blue;
    public int m_Amount = 1;
    public Transform m_PlayerTransform;

    //Component vars
    SpriteRenderer m_Renderer;
    Rigidbody m_Rigidbody;

	void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Rigidbody = GetComponent<Rigidbody>();

        switch(m_Type)
        {
            case EssenceType.Blue:
                m_Renderer.color = Color.blue;
                break;

            case EssenceType.Green:
                m_Renderer.color = Color.green;
                break;

            case EssenceType.Red:
                m_Renderer.color = Color.red;
                break;

            default:
                break;
        }

        if (transform.rotation.eulerAngles.x != 90)
            transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

    void Update()
    {
        if (Vector3.Distance(transform.position, m_PlayerTransform.position) < 3)
        {
            Vector3 dir = m_PlayerTransform.position - transform.position;
            m_Rigidbody.velocity = Vector3.Lerp(Vector3.zero, dir.normalized * 100.0f, 10.0f * Time.deltaTime);
        }
        else
            m_Rigidbody.velocity = Vector3.zero;
    }

    public void SetAmount(int amount)
    {
        m_Amount = amount;
    }
    public int GetAmount()
    {
        return m_Amount;
    }

    public void SetEType(EssenceType type)
    {
        m_Type = type;
    }
    public EssenceType GetEType()
    {
        return m_Type;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<PlayerInventory>().AddEssence(GetEType(), GetAmount());
            Destroy(gameObject);
        }
    }
}
