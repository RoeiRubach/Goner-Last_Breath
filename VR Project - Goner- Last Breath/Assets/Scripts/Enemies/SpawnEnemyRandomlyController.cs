using UnityEngine;

public class SpawnEnemyRandomlyController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private int maxChilders;

    [Range(1f, 5f)]
    private float spawnTime = 2.5f;
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

            Instantiate(enemyPrefab, gameObject.transform.GetChild(randomChild));
        }
    }
}
