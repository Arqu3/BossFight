using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasRenderer))]
public class Grid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Public vars
    public Item m_Item;

    //Hover vars
    public bool m_IsHover = false;

    //Color vars
    CanvasRenderer m_Renderer;
    Color m_StartColor;
    Color m_CurColor = Color.green;

	void Start ()
    {
        m_Renderer = GetComponent<CanvasRenderer>();

        if (m_Renderer)
            m_StartColor = m_Renderer.GetColor();
	}

    void Update()
    {
        if (m_Item)
            m_Item.transform.position = transform.position;
    }

    public void OnPointerEnter(PointerEventData pData)
    {
        m_IsHover = true;
        m_Renderer.SetColor(m_CurColor);
    }

    public void OnPointerExit(PointerEventData pData)
    {
        m_IsHover = false;
        m_Renderer.SetColor(m_StartColor);
    }

    public void SetItem(Item item)
    {
        m_Item = item;
    }
    public Item GetItem()
    {
        return m_Item;
    }

    public bool GetIsHover()
    {
        return m_IsHover;
    }
}
