using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public List<GameObject> m_ItemPrefabs = new List<GameObject>();

    //Component vars
    UIController m_UIController;
    Text m_ItemDescText;
    Text m_ItemDescText2;
    Image m_CursorImage;

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

        m_UIController = GameObject.Find("UI").GetComponent<UIController>();

        m_ItemDescText = m_UIController.GetCharacterPanel().FindChild("ItemDescText").GetComponent<Text>();
        m_ItemDescText2 = m_ItemDescText.transform.FindChild("Panel").GetComponentInChildren<Text>();
        m_CursorImage = m_UIController.GetCharacterPanel().FindChild("CursorItem").GetComponent<Image>();

        if (m_CursorImage)
            m_CursorImage.enabled = false;

        CalculateInvGrid();
        SpawnInventorySlots();

        for (int i = 0; i < m_ItemPrefabs.Count; i++)
        {
            if (m_ItemPrefabs[i])
            {
                GameObject clone = (GameObject)Instantiate(m_ItemPrefabs[i]);
                clone.transform.SetParent(m_UIController.GetCharacterPanel());
                clone.transform.localScale = new Vector3(1, 1, 1);

                if (clone.GetComponent<Item>())
                    AddItem(clone.GetComponent<Item>());
            }
        }

        //for (int i = 0; i < 5; i++)
        //{
        //    GameObject clone = (GameObject)Instantiate(m_ItemPrefabs);
        //    clone.transform.SetParent(m_UIController.GetCharacterPanel());
        //    clone.transform.localScale = new Vector3(1, 1, 1);
        //    if (clone.GetComponent<Item>())
        //        AddItem(clone.GetComponent<Item>());
        //}
    }

    void Update()
    {
        if (m_UIController.GetCharacterPanel().gameObject.activeSelf)
        {
            InventoryUpdate();

            if (m_ItemDescText.gameObject.activeSelf)
            {
                Vector3[] corners = new Vector3[4];
                m_ItemDescText.rectTransform.GetWorldCorners(corners);

                float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
                float minX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);

                if (maxX > Screen.width)
                {
                    m_ItemDescText.transform.position = new Vector3(Screen.width - (m_ItemDescText.rectTransform.sizeDelta.x / 2f) * UIController.m_Scalefactor, 
                        m_ItemDescText.transform.position.y, m_ItemDescText.transform.position.z);
                }
                else if (minX < 0)
                {
                    m_ItemDescText.transform.position = new Vector3(Screen.width + (m_ItemDescText.rectTransform.sizeDelta.x / 2f) * UIController.m_Scalefactor,
                        m_ItemDescText.transform.position.y, m_ItemDescText.transform.position.z);
                }

                m_ItemDescText.transform.SetAsLastSibling();
            }
        }
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

            for (int j = 0; j < m_EquipSlots.Count; j++)
            {
                if (m_Grid[i].GetIsHover())
                {
                    if (m_Grid[i].GetItem())
                    {
                        m_ItemDescText.gameObject.SetActive(true);
                        m_ItemDescText.text = m_Grid[i].GetItem().GetInformation(m_ItemDescText2);
                        m_ItemDescText2.text = m_ItemDescText.text;
                        m_ItemDescText.transform.position = m_Grid[i].transform.position - 
                            new Vector3(0.0f, (m_ItemDescText.rectTransform.sizeDelta.y + m_Grid[i].GetComponent<RectTransform>().sizeDelta.y) / 2f, 0.0f) * UIController.m_Scalefactor;
                        break;
                    }
                }
                else if (m_EquipSlots[j].GetIsHover())
                {
                    if (m_EquipSlots[j].GetItem())
                    {
                        m_ItemDescText.gameObject.SetActive(true);
                        m_ItemDescText.text = m_EquipSlots[j].GetItem().GetInformation(m_ItemDescText2);
                        m_ItemDescText2.text = m_ItemDescText.text;
                        m_ItemDescText.transform.position = m_EquipSlots[j].transform.position -
                            new Vector3(0.0f, (m_ItemDescText.rectTransform.sizeDelta.y + m_EquipSlots[j].GetComponent<RectTransform>().sizeDelta.y) / 2f, 0.0f) * UIController.m_Scalefactor;
                        break;
                    }
                }
                else if (!IsHover())
                    m_ItemDescText.gameObject.SetActive(false);
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
        {
            if (!m_CursorImage.isActiveAndEnabled)
                m_CursorImage.enabled = true;

            m_CursorImage.sprite = m_CursorItem.GetImage().sprite;
            m_CursorImage.color = m_CursorItem.GetImage().color;
            m_CursorImage.transform.position = Input.mousePosition;
            m_CursorImage.transform.SetAsLastSibling();
        }
        else
        {
            if (m_CursorImage.isActiveAndEnabled)
                m_CursorImage.enabled = false;
        }

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

    bool IsHover()
    {
        bool hover = false;

        for (int i = 0; i < m_Grid.Count; i++)
        {
            if (m_Grid[i].GetIsHover())
            {
                hover = true;
                break;
            }
        }

        for (int i = 0; i < m_EquipSlots.Count; i++)
        {
            if (m_EquipSlots[i].GetIsHover())
            {
                hover = true;
                break;
            }
        }

        return hover;
    }
}
