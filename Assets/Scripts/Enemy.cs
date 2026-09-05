using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed;
    public float chaseDistance = 2f;

    void Awake()
    {
        target = FindAnyObjectByType<PlayerController>().gameObject;
    }

    void Update()
    {
        Vector3 dir = target.transform.position - transform.position;
        if(dir.magnitude > chaseDistance)
        {
            transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
        }
    }
}
