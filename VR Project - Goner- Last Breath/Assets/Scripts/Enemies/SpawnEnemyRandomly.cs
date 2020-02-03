using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyRandomly : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;


    int maxChilders;
    private float spawnTimer = 3f;

    private void Start()
    {
        maxChilders = gameObject.transform.childCount;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = 3f;
            int randomChild = Random.Range(0, maxChilders);

            Instantiate(enemyPrefab, gameObject.transform.GetChild(randomChild));
        }

    }
}
