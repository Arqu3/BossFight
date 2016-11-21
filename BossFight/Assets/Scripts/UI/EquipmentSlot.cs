using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EquipType
{
    Armor,
    Weapon,
    Trinket
}

[RequireComponent(typeof(CanvasRenderer))]
public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Public vars
    public EquipType m_Type;
    public Item m_Item;

    //Component vars
    EntityStats m_Stats;
    CanvasRenderer m_Renderer;
    Image m_ItemImage;

    //Hover vars
    bool m_IsHover = false;
    Color m_StartColor;
    Color m_CurColor = Color.green;

	void Start()
    {
        m_Stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
        m_Renderer = GetComponent<CanvasRenderer>();
        m_ItemImage = transform.FindChild("ItemImage").GetComponentInChildren<Image>();

        if (m_Renderer)
            m_StartColor = m_Renderer.GetColor();

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

    public void EquipItem(Item item)
    {
        m_Item = item;
        m_Item.SetEquiped(true);

        ChangeStats(1);
    }
    public void UnEquipItem()
    {
        if (m_Item)
            ChangeStats(-1);

        m_Item.SetEquiped(false);
        m_Item = null;
    }

    public Item GetItem()
    {
        return m_Item;
    }

    public EquipType GetEqType()
    {
        return m_Type;
    }
    public void SetEqType(EquipType type)
    {
        m_Type = type;
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

    public bool GetIsHover()
    {
        return m_IsHover;
    }

    void ChangeStats(int multiplier)
    {
        multiplier = Mathf.Clamp(multiplier, -1, 1);

        m_Stats.AddMaxHealth(m_Item.m_MaxHealth * multiplier);
        m_Stats.AddHealthMulti(m_Item.m_HealthMulti * multiplier);
        m_Stats.AddDamage(m_Item.m_Damage * multiplier);
        m_Stats.AddDamageMulti(m_Item.m_DamageMulti * multiplier);
        m_Stats.AddAttackSpeed(m_Item.m_AttackSpeed * multiplier);
        m_Stats.AddAttackSpeedMulti(m_Item.m_AttackSpeedMulti * multiplier);
        m_Stats.AddAttackTime(m_Item.m_AttackTime * multiplier);
        m_Stats.AddMovementSpeed(m_Item.m_MovementSpeed * multiplier);
        m_Stats.AddMovementMulti(m_Item.m_MovementMulti * multiplier);
        m_Stats.AddKnockbackForce(m_Item.m_KnockbackForce * multiplier);
        m_Stats.AddKnockbackMulti(m_Stats.m_KnockbackMulti * multiplier);
    }
}
