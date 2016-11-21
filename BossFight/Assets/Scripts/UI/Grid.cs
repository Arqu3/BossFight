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
    Image m_ItemImage;

	void Start ()
    {
        m_Renderer = GetComponent<CanvasRenderer>();

        if (m_Renderer)
            m_StartColor = m_Renderer.GetColor();

        m_ItemImage = transform.FindChild("ItemImage").GetComponent<Image>();

        if (m_ItemImage)
            m_ItemImage.enabled = false;
    }

    void Update()
    {
        if (GetItem())
        {
            if (!m_ItemImage.isActiveAndEnabled)
                m_ItemImage.enabled = true;
            m_ItemImage.sprite = m_Item.GetImage().sprite;
            m_ItemImage.color = m_Item.GetImage().color;
        }
        else
        {
            if (m_ItemImage.isActiveAndEnabled)
                m_ItemImage.enabled = false;
        }
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
