using UnityEngine;

public class SpawnEnemyRandomlyController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private int maxChilders;

    [Range(1f, 5f)]
    [SerializeField] private float spawnTime = 2.5f;
    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnTime;
        maxChilders = gameObject.transform.childCount;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnTime;
            int randomChild = Random.Range(0, maxChilders);

            if (gameObject.transform.childCount != 0)
                Instantiate(enemyPrefab, gameObject.transform.GetChild(randomChild).position, gameObject.transform.GetChild(randomChild).rotation);
            else
            {
                Instantiate(enemyPrefab, gameObject.transform.position, gameObject.transform.rotation);
                GetComponent<SpawnEnemyRandomlyController>().enabled = false;
            }
        }
    }
}
