using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour
{
    public float damage = 5f;
    public float attackForce = 5f;
    public float attackDuration = 0.3f;
    public float attackCooldown = 1f;
    private bool isAttacking = false;
    public GameObject weapon;

    public void Poke()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(PokeCoroutine());
        }
    }

    IEnumerator PokeCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = weapon.transform.localPosition;
        while (elapsedTime < attackDuration)
        {
            float t = elapsedTime / attackDuration;
            weapon.transform.localPosition = Vector3.Lerp(originalPosition, originalPosition + Vector3.forward * attackForce, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(attackCooldown);
        weapon.transform.localPosition = originalPosition;

        isAttacking = false;
    }


}
