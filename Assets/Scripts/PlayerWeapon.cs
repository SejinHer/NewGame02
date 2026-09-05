using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{
    public float damage = 0f;
    public float rotateSpeed = 0.2f;

    private float swingSpeed = 0.3f;

    public PlayerHp playerHp;
    public GameObject sword;
    public GameObject swordHandle;
    private Vector3 swordScaleOffset = new Vector3(0.3f, 0.3f, 0.3f);

    void Start()
    {
        playerHp.OnPlayerSwing += SwingSword;
        playerHp.OnPlayerCharging += ChargeSword;
    }

    void SwingSword(float damage)
    {
        this.damage = damage;
        StartCoroutine(SwingCoroutine(rotateSpeed));
    }

    void ChargeSword(float chargingTime)
    {
        Vector3 swordScale = new Vector3(chargingTime, swordScaleOffset.y, swordScaleOffset.z);
        sword.transform.localScale = swordScale;
        Vector3 swordPos = sword.transform.localPosition;
        Vector3 movePos = new Vector3(chargingTime / 2, swordPos.y, swordPos.z);
        sword.transform.localPosition = movePos;
    }

    IEnumerator SwingCoroutine(float duration)
    {
        float startAngle = swordHandle.transform.localEulerAngles.y;
        float endAngle = startAngle - 180f;
        float time = 0f;
        while (time < swingSpeed)
        {
            time += Time.deltaTime;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, time / duration);
            swordHandle.transform.localRotation = Quaternion.Euler(new Vector3(0f, currentAngle, 0f));
            yield return null;
        }
        sword.transform.localScale = swordScaleOffset;
        sword.transform.localPosition = new Vector3(swordScaleOffset.x / 2, 0f, 0f);
    }
}
