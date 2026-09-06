using System.Collections;
using System.Reflection;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float damage = 0f;
    public float rotateSpeed = 0.2f;
    public float limitScale = 10f;
    //private float weaponCoolDown = 0.5f;

    private bool isRightSwing = true;
    private bool isSwing = false;
    public bool IsSwing
    {
        get { return isSwing; }
        set { }
    }

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
        isSwing = true;
        StartCoroutine(SwingCoroutine(rotateSpeed));
    }

    void ChargeSword(float chargingTime)
    {
        swordHandle.transform.localRotation = Quaternion.Euler(Vector3.zero);
        float swordScaleX = chargingTime;
        if (swordScaleX > limitScale)
        {
            swordScaleX = limitScale;
        }
        Vector3 swordScale = new Vector3(swordScaleX, swordScaleOffset.y, swordScaleOffset.z);
        sword.transform.localScale = swordScale;
        Vector3 swordPos = sword.transform.localPosition;
        Vector3 movePos = new Vector3(swordScaleX / 2, swordPos.y, swordPos.z);
        sword.transform.localPosition = movePos;
    }

    IEnumerator SwingCoroutine(float duration)
    {
        float startAngle = swordHandle.transform.localEulerAngles.y;
        float endAngle = startAngle - 180f;
        // if (isRightSwing)
        // {
        //     endAngle = startAngle - 180f;
        //     isRightSwing = false;
        // }
        // else
        // {
        //     isRightSwing = true;
        // }

        float time = 0f;
        while (time < swingSpeed)
        {
            time += Time.deltaTime;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, time / swingSpeed);
            swordHandle.transform.localRotation = Quaternion.Euler(new Vector3(0f, currentAngle, 0f));
            yield return null;
        }
        sword.transform.localScale = swordScaleOffset;
        sword.transform.localPosition = new Vector3(swordScaleOffset.x / 2, 0f, 0f);

        isSwing = false;
    }
}
