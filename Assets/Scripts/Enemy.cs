using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public GameObject target;
    public float moveSpeed;
    public float chaseDistance = 2f;
    private NavMeshAgent agent;

    private AttackController attackController;

    void Awake()
    {
        target = FindAnyObjectByType<PlayerController>().gameObject;
        attackController = GetComponentInChildren<AttackController>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void Update()
    {
        Vector3 dir = target.transform.position - transform.position;
        if (dir.magnitude > chaseDistance)
        {
            Vector3 awareDistance = -dir.normalized * chaseDistance;
            Vector3 targetPosition = target.transform.position + awareDistance;
            agent.SetDestination(targetPosition);
        }
        else
        {
            attackController.Poke();
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon") && other.GetComponent<PlayerWeapon>().IsSwing)
        {
            health -= other.GetComponent<PlayerWeapon>().damage;
            if (health <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
