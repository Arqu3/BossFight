using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EssenceType
{
    Red,
    Blue,
    Green
}

[RequireComponent(typeof(EntityStats), typeof(PlayerController))]
public class PlayerInventory : MonoBehaviour
{
    //Public vars
    public GameObject m_GridSlot;
    public Item m_CursorItem;

    public int m_RedEssence = 0;
    public int m_BlueEssence = 0;
    public int m_GreenEssence = 0;

    List<Grid> m_Grid = new List<Grid>();
    List<EquipmentSlot> m_EquipSlots = new List<EquipmentSlot>();

    public GameObject itemPrefab;

    //Component vars
    PlayerController m_Player;
    EntityStats m_Stats;
    UIController m_UIController;

    //Inventorry grid vars
    Vector2 m_InventoryGrid = new Vector2(3, 5);
    int m_GridLength = 0;
    int m_Rows = 0;
    Grid m_LastGrid;

    void Awake()
    {
        var eq = GameObject.FindGameObjectsWithTag("EquipmentSlot");
        if (eq.Length > 0)
        {
            for (int i = 0; i < eq.Length; i++)
            {
                m_EquipSlots.Add(eq[i].GetComponent<EquipmentSlot>());
            }
        }
    }

	void Start ()
    {
        m_GridLength = (int)(m_InventoryGrid.x * m_InventoryGrid.y);

        m_Stats = GetComponent<EntityStats>();
        m_Player = GetComponent<PlayerController>();
        m_UIController = GameObject.Find("UI").GetComponent<UIController>();

        CalculateInvGrid();
        SpawnInventorySlots();

        for (int i = 0; i < 5; i++)
        {
            GameObject clone = (GameObject)Instantiate(itemPrefab, m_UIController.GetCharacterPanel());
            if (clone.GetComponent<Item>())
                AddItem(clone.GetComponent<Item>());
        }
    }

    void Update()
    {
        if (m_UIController.GetCharacterPanel().gameObject.activeSelf)
            InventoryUpdate();
    }

    public int GetEssence(EssenceType type)
    {
        int num = 0;
        switch(type)
        {
            case EssenceType.Blue:
                num = m_BlueEssence;
                break;

            case EssenceType.Green:
                num = m_GreenEssence;
                break;

            case EssenceType.Red:
                num = m_RedEssence;
                break;
        }
        return num;
    }

    public void AddEssence(EssenceType type, int amount)
    {
        switch(type)
        {
            case EssenceType.Red:
                m_RedEssence += amount;
                break;

            case EssenceType.Blue:
                m_BlueEssence += amount;
                break;

            case EssenceType.Green:
                m_GreenEssence += amount;
                break;
        }
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < m_Grid.Count; i++)
        {
            if (!m_Grid[i].GetItem())
            {
                m_Grid[i].SetItem(item);
                break;
            }
            if (i >= m_Grid.Count-1 && m_Grid[i].GetItem())
            {
                Debug.Log("Inventory full!");
            }
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < m_Grid.Count; i++)
        {
            if (m_Grid[i].GetItem())
            {
                if (m_Grid[i].GetItem() == item)
                {
                    Destroy(m_Grid[i].GetItem().gameObject);
                    m_Grid[i].SetItem(null);
                    break;
                }
                if (i >= m_Grid.Count - 1)
                {
                    if (m_Grid[i].GetItem() != item || !m_Grid[i].GetItem())
                        Debug.Log("Item does not exist");
                }
            }
        }
    }
    public void RemoveItem(int index)
    {
        index = Mathf.Clamp(index, 0, m_Grid.Count - 1);
        if (m_Grid[index].GetItem())
        {
            Destroy(m_Grid[index].GetItem().gameObject);
            m_Grid[index].SetItem(null);
        }
    }

    void CalculateInvGrid()
    {
        for (int i = 0; i < m_GridLength; i++)
        {
            if (i % m_InventoryGrid.y == 0)
                m_Rows++;
        }
    }

    void SpawnInventorySlots()
    {
        int x = 0;
        int y = 0;

        float offsetX = (96 * m_InventoryGrid.y) / 2;
        float offsetY = (96 * m_InventoryGrid.x) / 2;

        for (int i = 0; i < m_GridLength; i++)
        {
            if (x % m_InventoryGrid.y == 0 && i > 0)
            {
                x = 0;
                y--;
            }

            GameObject clone = (GameObject)Instantiate(m_GridSlot, new Vector3((Screen.width / 2) + ((128 * x) + offsetX * 0.75f) * UIController.m_Scalefactor, 
                (Screen.height / 2) + ((128 * y) - offsetY * 0.65f) * UIController.m_Scalefactor, 0), Quaternion.identity, m_UIController.GetCharacterPanel());

            if (clone.GetComponent<Grid>())
                m_Grid.Add(clone.GetComponent<Grid>());
            x++;
        }
    }

    void InventoryUpdate()
    {
        for (int i = 0; i < m_Grid.Count; i++)
        {
            if (m_Grid[i].GetIsHover() && Input.GetMouseButtonDown(0) && m_CursorItem == null)
            {
                if (m_Grid[i].GetItem() != null)
                {
                    m_CursorItem = m_Grid[i].GetItem();
                    m_LastGrid = m_Grid[i];
                    m_Grid[i].SetItem(null);
                    Debug.Log("Set cursor item");
                }
            }
            if (Input.GetMouseButtonUp(0) && m_CursorItem != null)
            {
                if (m_Grid[i].GetIsHover())
                {
                    if (m_Grid[i].GetItem())
                    {
                        m_LastGrid.SetItem(m_Grid[i].GetItem());
                        m_Grid[i].SetItem(m_CursorItem);
                        m_CursorItem = null;
                        Debug.Log("Swapped items");
                    }
                    else
                    {
                        m_Grid[i].SetItem(m_CursorItem);
                        m_CursorItem = null;
                        Debug.Log("Set Item to grid");
                    }
                }
            }

            if (m_Grid[i].GetIsHover() && m_Grid[i].GetItem())
            {
                if (m_Grid[i].GetItem().GetIsEquipAble())
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        for (int j = 0; j < m_EquipSlots.Count; j++)
                        {
                            if (m_Grid[i].GetItem().GetEType().Equals(m_EquipSlots[j].GetEqType()))
                            {
                                if (!m_EquipSlots[j].GetItem())
                                {
                                    m_EquipSlots[j].EquipItem(m_Grid[i].GetItem());
                                    m_Grid[i].SetItem(null);

                                    Debug.Log("Equiped item");
                                }
                                else
                                {
                                    Item temp = m_Grid[i].GetItem();
                                    m_Grid[i].SetItem(m_EquipSlots[j].GetItem());
                                    m_EquipSlots[j].UnEquipItem();
                                    m_EquipSlots[j].EquipItem(temp);

                                    Debug.Log("Swapped equiped item");
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (m_CursorItem)
        {
            if (m_CursorItem.GetIsEquipAble())
            {
                if (Input.GetMouseButtonUp(0))
                {
                    for (int i = 0; i < m_EquipSlots.Count; i++)
                    {
                        if (m_EquipSlots[i].GetIsHover() && m_EquipSlots[i].GetEqType().Equals(m_CursorItem.GetEType()))
                        {
                            if (!m_EquipSlots[i].GetItem())
                            {
                                m_EquipSlots[i].EquipItem(m_CursorItem);
                                m_CursorItem = null;
                                Debug.Log("Equiped item");
                            }
                            else
                            {
                                Item temp = m_CursorItem;
                                m_CursorItem = m_EquipSlots[i].GetItem();
                                m_EquipSlots[i].UnEquipItem();
                                m_EquipSlots[i].EquipItem(temp);
                                Debug.Log("Swapped equiped item");
                            }
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < m_EquipSlots.Count; i++)
            {
                if (m_EquipSlots[i].GetIsHover() && m_EquipSlots[i].GetItem())
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        m_CursorItem = m_EquipSlots[i].GetItem();
                        m_EquipSlots[i].UnEquipItem();
                        break;
                    }
                }
            }
        }


        if (m_CursorItem)
            m_CursorItem.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            bool noneHover = true;
            for (int i = 0; i < m_Grid.Count; i++)
            {
                for (int j = 0; j < m_EquipSlots.Count; j++)
                {
                    if (m_Grid[i].GetIsHover() || m_EquipSlots[j].GetIsHover())
                    {
                        noneHover = false;
                        break;
                    }
                }
            }
            //Debug.Log(noneHover);
            if (noneHover && m_CursorItem && m_LastGrid)
            {
                m_LastGrid.SetItem(m_CursorItem);
                m_CursorItem = null;
                m_LastGrid = null;
                Debug.Log("Set item to last known grid");
            }
        }

    }
    void SwapItems(Grid g, Item i)
    {
        if (g.GetItem() != i)
        {
            Item temp = i;
            i = g.GetItem();
            g.SetItem(temp);
        }
    }

    void SwapItems(Grid g1, Grid g2)
    {
        if (g1 != g2 && g1.GetItem() != g2.GetItem())
        {
            Grid temp = g1;
            g1 = g2;
            g2 = temp;
        }
    }
}
