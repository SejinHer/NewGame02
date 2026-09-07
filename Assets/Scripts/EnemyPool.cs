using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    ObjectPool<GameObject> enemyPool;

    public int poolSize = 5;

    private void Awake()
    {
        enemyPool = CreatePool();
        PrewarmedObject(enemyPool, poolSize);
    }

    ObjectPool<GameObject> CreatePool()
    {
        return new ObjectPool<GameObject>(
            createFunc: () => CreateEnemy(enemyPrefab),
            actionOnGet: enemy => GetEnemy(enemy),
            actionOnRelease: enemy => enemy.gameObject.SetActive(false),
            actionOnDestroy: enemy => Destroy(enemy),
            maxSize: poolSize
        );
    }
    private GameObject CreateEnemy(GameObject enemyPf)
    {
        GameObject enemyObj = Instantiate(enemyPf);
        enemyObj.transform.SetParent(transform);
        enemyObj.gameObject.SetActive(false);
        return enemyObj;
    }

    public void SpawnEnemy(Vector3 pos)
    {
        GameObject spawnObj = enemyPool.Get();
        spawnObj.transform.position = pos;
        GetEnemy(spawnObj);
    }
    private void GetEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
        Enemy enemyCom = enemy.GetComponent<Enemy>();
        enemyCom.Initialize();
    }

    private void PrewarmedObject(ObjectPool<GameObject> pool, int count)
    {
        GameObject[] prewarmedEnemy = new GameObject[count];
        for (int i = 0; i < count; i++) { prewarmedEnemy[i] = pool.Get(); }
        for (int i = 0; i < count; i++) { pool.Release(prewarmedEnemy[i]); }
    }
}
