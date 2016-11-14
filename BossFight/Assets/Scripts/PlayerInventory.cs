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
    public GameObject m_CursorItem;

    public int m_RedEssence = 0;
    public int m_BlueEssence = 0;
    public int m_GreenEssence = 0;

    public List<Grid> m_Grid = new List<Grid>();

    //Component vars
    PlayerController m_Player;
    EntityStats m_Stats;
    UIController m_UIController;

    //Inventorry grid vars
    Vector2 m_InventoryGrid = new Vector2(3, 5);
    int m_GridLength = 0;
    int m_Rows = 0;
    Grid m_LastGrid;

    Color m_ActiveColor = Color.green;
    int m_ActiveItems = 8;

	void Start ()
    {
        m_GridLength = (int)(m_InventoryGrid.x * m_InventoryGrid.y);

        m_Stats = GetComponent<EntityStats>();
        m_Player = GetComponent<PlayerController>();
        m_UIController = GameObject.Find("UI").GetComponent<UIController>();

        CalculateInvGrid();
        SpawnInventorySlots();
	}

    void Update()
    {
        //DisplayInventory();
        InventoryUpdate();
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

        float m_AvgX = (64 * m_InventoryGrid.y) / 2;
        float m_AvgY = (128 * m_InventoryGrid.x) / 2;

        for (int i = 0; i < m_GridLength; i++)
        {
            if (x % m_InventoryGrid.y == 0 && i > 0)
            {
                x = 0;
                y--;
            }

            GameObject clone = (GameObject)Instantiate(m_GridSlot, new Vector3((Screen.width / 2) + (128 * x * UIController.m_Scalefactor), 
                (Screen.height / 2) + ((128 * y) + m_AvgY) * UIController.m_Scalefactor, 0), Quaternion.identity, m_UIController.GetCharacterPanel());

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
                        GameObject temp = m_CursorItem;

                        m_LastGrid.SetItem(m_Grid[i].GetItem());
                        m_Grid[i].SetItem(m_CursorItem);
                        m_CursorItem = null;

                        //m_CursorItem = m_Grid[i].GetItem();
                        //m_Grid[i].SetItem(temp);
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
        }

        if (m_CursorItem)
            m_CursorItem.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            bool noneHover = true;
            for (int i = 0; i < m_Grid.Count; i++)
            {
                if (m_Grid[i].GetIsHover())
                    noneHover = false;
            }
            Debug.Log(noneHover);
            if (noneHover && m_CursorItem && m_LastGrid)
            {
                m_LastGrid.SetItem(m_CursorItem);
                m_CursorItem = null;
                m_LastGrid = null;
                Debug.Log("Set item to last known grid");
            }
        }

    }

    void DisplayInventory()
    {
        int x = 0;
        int y = 0;

        for (int i = 0; i < m_Grid.Count; i++)
        {
            if (x % m_InventoryGrid.y == 0)
            {
                x = 0;
                y--;
            }
            if (i < m_ActiveItems)
                m_ActiveColor = Color.green;
            else
                m_ActiveColor = Color.red;
            Debug.DrawRay(Camera.main.WorldToScreenPoint(new Vector3(1 * x, 0, 1 * y)), new Vector3(0, 10.0f, 0), m_ActiveColor);
            x++;
        }
    }

    void SwapItems(Grid g, GameObject gO)
    {
        if (g.GetItem() != gO)
        {
            GameObject temp = gO;
            gO = g.GetItem();
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
