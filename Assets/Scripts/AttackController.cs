using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour
{
    public float damage = 5f;
    public float attackForce = 5f;
    public float attackDuration = 0.3f;
    public float attackCooldown = 1f;
    private float attackTime = 0f;
    private bool canAttack = true;
    public GameObject weapon;
    public Vector3 posOffset = new Vector3(0, 0, 0.5f);

    private void Awake()
    {
        weapon.SetActive(false);
    }
    public void Poke()
    {
        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(PokeCoroutine());
        }
    }

    void Update()
    {
        attackTime += Time.deltaTime;
        if (attackTime > attackCooldown)
        {
            attackTime = 0f;
            canAttack = true;
        }

    }

    IEnumerator PokeCoroutine()
    {
        weapon.SetActive(true);
        float elapsedTime = 0f;
        Vector3 originalPosition = weapon.transform.localPosition;

        while (elapsedTime < attackDuration)
        {
            float t = elapsedTime / attackDuration;
            weapon.transform.localPosition = Vector3.Lerp(originalPosition, originalPosition + Vector3.forward * attackForce, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weapon.SetActive(false);
        weapon.transform.localPosition = posOffset;
    }


}
