using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public string key = "Zombie";
    public GameObject Zombie;

    void Awake()
    {
        key = Zombie.name;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnZombie());
    }

    private IEnumerator SpawnZombie()
    {
        while (true)
        {
            GameObject instance = PoolManager.Instance.GetObject(key, Zombie, this.transform);

            instance.transform.position = this.transform.position;
            yield return new WaitForSeconds/*(5f)*/(Random.Range(1f, 3.5f));
        }
    }
}
