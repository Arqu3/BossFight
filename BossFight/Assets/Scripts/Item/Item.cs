using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public string m_Instruction;
    public string m_Description;
    public bool m_IsStackable;

    //Public stat vars
    public int m_MaxHealth = 0;
    public float m_HealthMulti = 0.0f;
    public int m_MinDamage = 0;
    public int m_MaxDamage = 0;
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
    //public bool m_CanBeKnockedback = true;
    //public float m_DamagedTimer = 0.0f;
    //public bool m_CanDie = true;
    //public bool m_CanTakeDamage = true;
    //public bool m_CanBeStunned = true;
    //public float m_StunAmount = 0.0f;
    //public float m_StunReduction = 0.0f;

    //Component vars
    Image m_Image;

    public virtual void Start()
    {
        m_Image = GetComponent<Image>();
        m_MinDamage = Mathf.Clamp(m_MinDamage, 0, m_MaxDamage);
    }

    public Image GetImage()
    {
        return m_Image;
    }

    //Get and set states/values etc
    bool m_IsEquiped = false;

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
    //Set text color in string
    public virtual string GetDescription()
    {
        return "<color=#d1c458ff>" + m_Description + "</color>";
    }
    //Set text color in string
    public virtual string GetInstruction()
    {
        return "<color=#ff0000ff>" + m_Instruction + "</color>";
    }
    //Set text color in string
    public virtual string GetName()
    {
        return "<color=" + GetNameColor() + ">" + m_ItemName + "</color>";
    }
    public virtual Rarity GetRarity()
    {
        return m_Rarity;
    }
    public virtual bool GetIsEquiped()
    {
        return m_IsEquiped;
    }

    //Set parent to player if currently equiped, otherwise set parent to UI
    public virtual void SetEquiped(bool state)
    {
        m_IsEquiped = state;

        if (m_IsEquiped)
        {
            transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform.FindChild("Items"));
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.SetParent(GameObject.Find("CharacterPanel").transform);
            transform.localPosition = new Vector3(Screen.width * 2.0f, Screen.height, 0.0f);
        }
    }

    //Get all non-0 stat & text values
    public virtual string GetInformation(Text text)
    {
        string s = "";

        s += GetName();

        if (m_MaxHealth != 0)
            s += "\n" + m_MaxHealth.ToString() + " to max health";

        if (m_HealthMulti != 0)
            s += "\n" + SetMultiText(m_HealthMulti) + "health";

        if (m_MaxDamage != 0)
            s += "\n" + m_MinDamage.ToString() + "-" + m_MaxDamage.ToString() + " damage";

        if (m_DamageMulti != 0)
            s += "\n" + SetMultiText(m_DamageMulti) + "damage";

        if (m_MovementSpeed != 0)
            s += "\n" + m_MovementSpeed.ToString() + " to movement speed";

        if (m_MovementMulti != 0)
            s += "\n" + SetMultiText(m_MovementMulti) + "movement speed";

        if (m_AttackSpeed != 0)
            s += "\n" + m_AttackSpeed.ToString() + " attack speed";

        if (m_AttackSpeedMulti != 0)
            s += "\n" + SetMultiTextInverted(m_AttackSpeedMulti) + "attack speed";

        if (m_Instruction != "")
            s += "\n" + GetInstruction();

        if (m_Description != "")
            s += "\n" + GetDescription();

        s = s.Replace("NL", "\n");

        return s;
    }

    public string SetMultiText(float value)
    {
        string s = "";

        if (value != 0)
        {
            s = (value * 100).ToString() + "% ";

            if (value > 0)
                s += "increased ";
            else
                s += "decreased ";
        }

        return s;
    }

    public string SetMultiTextInverted(float value)
    {
        string s = "";

        if (value != 0)
        {
            s = (value * 100 * -1).ToString() + "% ";

            if (value > 0)
                s += "decreased ";
            else
                s += "increased ";
        }

        return s;
    }

    //Get different color depending on rarity
    string GetNameColor()
    {
        string color = "";

        switch (m_Rarity)
        {
            case Rarity.Trash:
                //Gray
                color = "#808080ff";
                break;
            case Rarity.Common:
                //White
                color = "#ffffffff";
                break;
            case Rarity.Uncommon:
                //Green/lime
                color = "#00ff00ff";
                break;
            case Rarity.Rare:
                //Blue
                color = "#0000ffff";
                break;
            case Rarity.Epic:
                //Purple
                color = "#800080ff";
                break;
            case Rarity.Legendary:
                //Orange
                color = "#ffa500ff";
                break;
            default:
                break;
        }

        return color;
    }
}
