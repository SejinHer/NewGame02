using System.Collections;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    private EnemyPool enemyPool;
    private GameObject player;
    [SerializeField] private float worldDia = 30f;
    [SerializeField] private float spawnDistLimit = 5f;

    private void Awake()
    {
        enemyPool = GetComponent<EnemyPool>();
        player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy(0.5f, 0, 2f));
        StartCoroutine(SpawnEnemy(0.5f, 4, 2f));
        StartCoroutine(SpawnEnemy(1f, 4, 4f));
    }

    IEnumerator SpawnEnemy(float spawnRate, float delay, float continueTime)
    {
        float dia = worldDia / 2 - 1;
        float time = continueTime;

        while (time > 0)
        {
            int rand = Random.Range(0, 360);
            enemyPool.SpawnEnemy(AngleToVector(rand, dia));
            yield return new WaitForSeconds(spawnRate);
            time -= spawnRate;
        }
    }

    Vector3 AngleToVector(float deg, float dis)
    {
        var rad = deg * Mathf.Deg2Rad;
        Vector3 spawnPos = new Vector3(Mathf.Cos(rad) * dis, Mathf.Sin(rad) * dis);
        if (Mathf.Abs(player.transform.position.x - spawnPos.x) < 5
            && Mathf.Abs(player.transform.position.z - spawnPos.z) < 5)
        {
            float value = (Random.Range(0, 2) == 0) ? -50f : 50f;
            rad = (deg + value) * Mathf.Deg2Rad;
            spawnPos = new Vector3(Mathf.Cos(rad) * dis, 0, Mathf.Sin(rad) * dis);

        }
        return new Vector3(Mathf.Cos(rad) * dis, 0, Mathf.Sin(rad) * dis);
    }
}
