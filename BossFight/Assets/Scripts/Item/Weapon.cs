using UnityEngine;
using System.Collections;

public class Weapon : Item
{
    public bool m_IsAttack = false;
    public bool m_IsSpecialAttack = false;
    public bool m_CanCharge = true;

    public override void Start()
    {
        base.Start();
        m_ItemType = ItemType.Equipable;
        m_EquipType = EquipType.Weapon;
	}

    public virtual void Attack()
    {

    }
    public virtual void AttackUpdate(float attackSpeed, float attackTime)
    {

    }

    public virtual void SpecialAttack(float charge)
    {

    }
    public virtual void SpecialAttackUpdate(float attackSpeed, float attackTime)
    {

    }

    public override ItemType GetIType()
    {
        return ItemType.Equipable;
    }

    public override EquipType GetEType()
    {
        return EquipType.Weapon;
    }

    public override bool GetIsEquipAble()
    {
        return true;
    }

    public override bool GetIsStackable()
    {
        return false;
    }

    public virtual bool GetBasicButton()
    {
        return Input.GetMouseButton(0);
    }

    public virtual bool GetSpecialButton()
    {
        return Input.GetMouseButton(1);
    }

    public virtual bool AttackObjActive()
    {
        return false;
    }
    public virtual bool SpecialObjActive()
    {
        return false;
    }

    public virtual bool GetIsAttack()
    {
        return m_IsAttack;
    }
    public virtual bool GetIsSpecial()
    {
        return m_IsSpecialAttack;
    }
    public bool GetCanCharge()
    {
        return m_CanCharge;
    }
}
