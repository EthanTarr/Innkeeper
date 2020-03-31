using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Transform> Tables;

    public float MinSpawnTime = 5f;
    public float MaxSpawnTime = 25f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCustomer()
    {
        while (true)
        {
            List<Transform> emptyTables = findEmptyTable();
            if(emptyTables.Count > 0)
            {
                emptyTables[Random.Range(0, emptyTables.Count - 1)].GetComponent<TableBehavior>().SpawnCustomer();
            }
            float SpawnTime = Random.Range(MinSpawnTime, MaxSpawnTime);
            yield return new WaitForSeconds(SpawnTime); //wait for spawntime
        }
    }

    private List<Transform> findEmptyTable()
    {
        List<Transform> emptyTables = new List<Transform>();
        foreach(Transform table in Tables)
        {
            if(table.GetComponent<TableBehavior>().CurrentCustomer == null)
            {
                emptyTables.Add(table);
            }
        }
        return emptyTables;
    }
}
