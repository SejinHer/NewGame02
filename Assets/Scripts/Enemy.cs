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
    private bool isBeating = false;

    private AttackController attackController;

    void Awake()
    {
        target = FindAnyObjectByType<PlayerController>().gameObject;
        attackController = GetComponentInChildren<AttackController>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = chaseDistance;
    }

    void Update()
    {
        Vector3 dir = target.transform.position - transform.position;
        if (isBeating)
        {
            transform.Translate(-Vector3.forward * 10 * Time.deltaTime);
        }
        else if (isBeating == false)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            attackController.Poke();
        }
    }

    IEnumerator StopBeating()
    {
        Vector3 dir = target.transform.position - transform.position;
        float t = 0f;
        while (t < 0.2f)
        {
            transform.Translate(-Vector3.forward * 6 * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
        isBeating = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon") && other.GetComponent<PlayerWeapon>().IsSwing)
        {
            health -= other.GetComponent<PlayerWeapon>().damage;
            isBeating = true;
            agent.isStopped = true;
            StartCoroutine(StopBeating());
            agent.isStopped = false;
            if (health <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
