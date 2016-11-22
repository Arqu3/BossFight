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
    }
    public Image GetImage()
    {
        return m_Image;
    }

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
    public virtual string GetDescription()
    {
        return m_Description;
    }
    public virtual string GetInstruction()
    {
        return m_Instruction;
    }
    public virtual string GetName()
    {
        return m_ItemName;
    }
    public virtual Rarity GetRarity()
    {
        return m_Rarity;
    }
    public virtual bool GetIsEquiped()
    {
        return m_IsEquiped;
    }
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
    public virtual string GetInformation(Text text)
    {
        string s = "";

        s += "<color=" + GetNameColor() + ">" + GetName() + "\n</color>";

        if (m_MaxHealth != 0)
            s += m_MaxHealth.ToString() + " to max health\n";

        if (m_HealthMulti != 0)
            s += SetMultiText(m_HealthMulti) + "health\n";

        if (m_Damage != 0)
            s += m_Damage.ToString() + " to damage\n";

        if (m_DamageMulti != 0)
            s += SetMultiText(m_DamageMulti) + "damage\n";

        if (m_MovementSpeed != 0)
            s += m_MovementSpeed.ToString() + " to movement speed\n";

        if (m_MovementMulti != 0)
            s += SetMultiText(m_MovementMulti) + "movement speed\n";

        if (m_AttackSpeed != 0)
            s += m_AttackSpeed.ToString() + " to attack speed\n";

        if (m_AttackSpeedMulti != 0)
            s += SetMultiText(m_AttackSpeedMulti) + "attack speed\n";

        if (GetInstruction() != "")
            s += "<color=#ff0000ff>" + GetInstruction() + "\n</color>";

        if (GetDescription() != "")
            s += "<color=#d1c458ff>" + GetDescription() + "</color>";

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
