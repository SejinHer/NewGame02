using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public GameObject target;
    public float moveSpeed;
    public float chaseDistance = 2f;
    private NavMeshAgent agent;
    private bool isBeating = false;
    private AttackController attackController;
    private Coroutine knockbackCoroutine;

    void Awake()
    {
        target = FindAnyObjectByType<PlayerController>().gameObject;
        attackController = GetComponentInChildren<AttackController>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = chaseDistance;
    }

    public void Initialize()
    {
        health = maxHealth;
        agent.speed = moveSpeed;
        agent.stoppingDistance = chaseDistance;
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }

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

        if (dir.magnitude < chaseDistance)
        {
            attackController.Poke();
        }
    }

    IEnumerator KnockbackRoutine()
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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("PlayerWeapon") && collision.gameObject.GetComponent<PlayerWeapon>().IsSwing)
    //    {
    //        health -= collision.gameObject.GetComponent<PlayerWeapon>().damage;
    //        isBeating = true;
    //        agent.isStopped = true;
    //        if (knockbackCoroutine != null)
    //        {
    //            StopCoroutine(knockbackCoroutine);
    //        }
    //        knockbackCoroutine = StartCoroutine(KnockbackRoutine());
    //        agent.isStopped = false;
    //        if (health <= 0f)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon") && other.GetComponent<PlayerWeapon>().IsSwing)
        {
            health -= other.GetComponent<PlayerWeapon>().damage;
            isBeating = true;
            if (agent != null) agent.isStopped = true;
            if (knockbackCoroutine != null)
            {
                StopCoroutine(knockbackCoroutine);
            }
            knockbackCoroutine = StartCoroutine(KnockbackRoutine());
            if (agent != null) agent.isStopped = false;
            if (health <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
