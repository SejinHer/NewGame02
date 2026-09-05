using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed;

    void Awake()
    {
        target = FindAnyObjectByType<PlayerController>().gameObject;
    }

    void Update()
    {
        Vector3 dir = target.transform.position - transform.position;

        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
    }
}
