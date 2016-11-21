using UnityEngine;
using System.Collections;

public class Weapon : Item
{
	public override void Start()
    {
        base.Start();
        m_ItemType = ItemType.Equipable;
        m_EquipType = EquipType.Weapon;

        //Debug.Log(GetIType() + " " + GetEType() + " " + IsEquipAble());	
	}
	
	void Update()
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
}
