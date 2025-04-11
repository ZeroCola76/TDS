using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public string key = "Zombie";
    public GameObject Zombie;

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
            yield return new WaitForSeconds(1f);
        }
    }
}
