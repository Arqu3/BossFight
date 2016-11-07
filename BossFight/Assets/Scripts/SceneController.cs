using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{
    //Public vars
    public GameObject[] m_EnemyPrefabs;
    public float m_SpawnInterval = 10.0f;
    public int m_MaxEnemies = 10;

    //Enemy spawnpoint vars
    Transform[] m_SpawnPoints;

    //Enemy spawn vars
    public static int m_CurrentEnemyAmount = 0;
    int m_SpawnedEnemyAmount = 0;

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
        m_SpawnPoints = new Transform[spawnpoints.Length];

        if (spawnpoints.Length > 0)
        {
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

        SpawnEnemy(m_EnemyPrefabs[0]);
	}
	
	void Update()
    {

	}

    void SpawnEnemy(GameObject type)
    {
        GameObject clone = (GameObject)Instantiate(type, m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length - 1)].position, Quaternion.identity);
        m_SpawnedEnemyAmount++;
        m_CurrentEnemyAmount++;
    }
}
