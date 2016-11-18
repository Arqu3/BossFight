using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    //Public vars
    public GameObject[] m_EnemyPrefabs;
    public GameObject[] m_DropPrefabs;
    public float m_SpawnInterval = 10.0f;
    public int m_MaxEnemies = 10;

    //Component vars
    TeleporterController m_Teleporter;

    //Player vars
    PlayerController m_Player;
    PlayerInventory m_Inventory;

    //Enemy spawnpoint vars
    Transform[] m_SpawnPoints;

    //Enemy spawn vars
    List<EntityStats> m_Enemies = new List<EntityStats>();
    int m_SpawnedEnemyAmount = 0;
    int m_EnemiesKilled = 0;
    float m_SpawnTimer = 0.0f;

    //Arena vars
    GameObject m_Arena1;
    public static float m_MaxX;
    public static float m_MinX;
    public static float m_MaxZ;
    public static float m_MinZ;

    void Awake()
    {
        m_Arena1 = GameObject.Find("Arena1");
        var v = m_Arena1.transform.FindChild("Plane").GetComponent<MeshRenderer>();
        m_MaxX = v.bounds.center.x + v.bounds.extents.x;
        m_MinX = v.bounds.center.x - v.bounds.extents.x;

        m_MaxZ = v.bounds.center.z + v.bounds.extents.z;
        m_MinZ = v.bounds.center.z - v.bounds.extents.z;
        //Debug.Log(m_MinX + " " + m_MaxX + " " + m_MinZ + " " + m_MaxZ);
    }

	void Start()
    {
        //Find and sort spawnpoints
        var spawnpoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if (spawnpoints.Length > 0)
        {
            m_SpawnPoints = new Transform[spawnpoints.Length];

            for (int i = 0; i < spawnpoints.Length; i++)
            {
                m_SpawnPoints[i] = spawnpoints[i].transform;
            }

            for (int write = 0; write < m_SpawnPoints.Length; write++)
            {
                for (int sort = 0; sort < m_SpawnPoints.Length - 1; sort++)
                {
                    if (m_SpawnPoints[sort].GetComponent<SpawnPoint>().m_ID > m_SpawnPoints[sort + 1].GetComponent<SpawnPoint>().m_ID)
                    {
                        var temp = m_SpawnPoints[sort + 1];
                        m_SpawnPoints[sort + 1] = m_SpawnPoints[sort];
                        m_SpawnPoints[sort] = temp;
                    }
                }
            }
        }
        else
            Debug.Log("Could not find any spawnpoints!");

        m_Teleporter = GameObject.Find("Teleport").GetComponent<TeleporterController>();
        m_Player = GameObject.Find("Player").GetComponent<PlayerController>();
        m_Inventory = m_Player.gameObject.GetComponent<PlayerInventory>();
	}
	
	void Update()
    {
        EnemyUpdate();
	}

    void EnemyUpdate()
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            if (m_Enemies[i].GetHealth() < 1 && m_Enemies[i].GetCanDie())
            {
                if (Random.Range(0, 3) == 1)
                    DropItem(m_DropPrefabs[0], m_Enemies[i].transform.position);

                Destroy(m_Enemies[i].gameObject);
                m_Enemies.Remove(m_Enemies[i]);
                m_EnemiesKilled++;
                break;
            }
        }

        if (m_Player.IsInCombatArea())
        {
            if (m_SpawnedEnemyAmount < m_MaxEnemies)
            {
                m_SpawnTimer += Time.deltaTime;
                if (m_SpawnTimer >= m_SpawnInterval)
                {
                    SpawnEnemy(m_EnemyPrefabs[0]);
                    m_SpawnTimer = 0.0f;
                }
            }
            else if (m_SpawnedEnemyAmount >= m_MaxEnemies && m_Enemies.Count == 0)
            {
                m_SpawnTimer = 0.0f;
                m_Teleporter.SetCanTeleport(true);
            }
        }
        else
        {
            m_SpawnTimer = 0.0f;
            m_Teleporter.SetCanTeleport(true);

            if (m_SpawnedEnemyAmount >= m_MaxEnemies && m_Enemies.Count == 0)
            {
                m_SpawnedEnemyAmount = 0;
                m_EnemiesKilled = 0;
            }
        }
    }

    public int GetRemainingEnemies()
    {
        return m_MaxEnemies - m_EnemiesKilled;
    }

    void SpawnEnemy(GameObject type)
    {
        GameObject clone = (GameObject)Instantiate(type, m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length - 1)].position, Quaternion.identity);
        if (clone.GetComponent<EntityStats>())
            m_Enemies.Add(clone.GetComponent<EntityStats>());
        else
            Debug.Log("Spawned enemy is missing EntityStats!");
        m_SpawnedEnemyAmount++;
    }

    void DropItem(GameObject item, Vector3 position)
    {
        GameObject clone = (GameObject)Instantiate(item, new Vector3(position.x, 0.0f, position.z), Quaternion.identity);
        if (clone.GetComponent<Essence>())
        {
            //var v = System.Enum.GetValues(typeof(EssenceType));
            //EssenceType randomE = (EssenceType)v.GetValue(Random.Range(0, v.Length));

            int random = Random.Range(1, 101);
            EssenceType randomType;

            if (random > 0 && random < 46)
                randomType = EssenceType.Blue;
            else if (random > 45 && random < 91)
                randomType = EssenceType.Green;
            else
                randomType = EssenceType.Red;

            clone.GetComponent<Essence>().m_Type = randomType;
        }
    }
}
