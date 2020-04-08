using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Transform> Tables;

    public float MinSpawnTime = 5f;
    public float MaxSpawnTime = 25f;

    public List<Transform> StorageTables;
    public Transform Customer;

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

    public void ResetInn()
    {
        Customer.GetComponent<CustomerBehavior>().DrakeChance = Customer.GetComponent<CustomerBehavior>().OriginalDrakeChance;
        Customer.GetComponent<CustomerBehavior>().GoblinChance = Customer.GetComponent<CustomerBehavior>().OriginalGoblinChance;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance = Customer.GetComponent<CustomerBehavior>().OriginalAntiniumChance;
        foreach(Transform table in Tables)
        {
            Destroy(table.GetComponent<TableBehavior>().CurrentCustomer.gameObject);
            Destroy(table.GetComponent<TableBehavior>().CurrentCustomer.GetComponent<PopUpObjectBehavior>().Popup.gameObject);
        }
        foreach (Transform storage in StorageTables)
        {
            if (storage.GetComponent<StorageBehaviour>().LeftObject != null)
            {
                Destroy(storage.GetComponent<StorageBehaviour>().LeftObject.gameObject);
            }
            else if (storage.GetComponent<StorageBehaviour>().CenterObject != null)
            {
                Destroy(storage.GetComponent<StorageBehaviour>().CenterObject.gameObject);
            }
            else if (storage.GetComponent<StorageBehaviour>().RightObject != null)
            {
                Destroy(storage.GetComponent<StorageBehaviour>().RightObject.gameObject);
            }
        }
    }
}
