using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ItemType
{
    Equipable,
    Consumable,
    Quest,
    Misc
}

public enum Rarity
{
    Trash,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class Item : MonoBehaviour
{
    //Public vars
    public ItemType m_ItemType;
    public EquipType m_EquipType;
    public Rarity m_Rarity;
    public string m_ItemName;
    public string m_Description;
    public bool m_IsStackable;

    public int m_MaxHealth = 0;
    public float m_HealthMulti = 0.0f;
    public int m_Damage = 0;
    public float m_DamageMulti = 0.0f;
    public float m_MovementSpeed = 0.0f;
    public float m_MovementMulti = 0.0f;
    public float m_AttackSpeed = 0.0f;
    public float m_AttackSpeedMulti = 0.0f;
    public float m_AttackTime = 0.0f;
    public float m_AggroRange = 0.0f;
    public float m_AttackRange = 0.0f;
    public float m_KnockbackForce = 0.0f;
    public float m_KnockbackMulti = 0.0f;
    public bool m_CanBeKnockedback = true;
    public float m_DamagedTimer = 0.0f;
    public bool m_CanDie = true;
    public bool m_CanTakeDamage = true;
    public bool m_CanBeStunned = true;
    public float m_StunAmount = 0.0f;
    public float m_StunReduction = 0.0f;

    public virtual ItemType GetIType()
    {
        return m_ItemType;
    }
    public virtual EquipType GetEType()
    {
        return m_EquipType;
    }
    public virtual bool GetIsEquipAble()
    {
        return m_ItemType.Equals(ItemType.Equipable);
    }
    public virtual bool GetIsStackable()
    {
        return m_IsStackable;
    }
    public virtual string GetDescription()
    {
        return m_Description;
    }
    public virtual string GetName()
    {
        return m_ItemName;
    }
    public virtual Rarity GetRarity()
    {
        return m_Rarity;
    }
}
