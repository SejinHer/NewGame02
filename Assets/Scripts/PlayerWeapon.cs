using System.Collections;
using System.Reflection;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float damage = 0f;
    public float damageMul = 0f;
    public float rotateSpeed = 0.2f;
    public float limitScaleX = 10f;
    public float limitScaleZ = 2.5f;
    private float checkCoolTime = 0f;
    public float weaponCoolDown = 0.5f;

    private bool isRightSwing = true;
    private bool isSwing = false;
    private bool canSwing = true;
    public bool IsSwing
    {
        get { return isSwing; }
        set { }
    }

    private float swingSpeed = 0.3f;

    public PlayerHp playerHp;
    public GameObject sword;
    public GameObject swordHandle;
    public Vector3 swordScaleOffset = new Vector3(0.3f, 0.3f, 0.3f);

    void Start()
    {
        playerHp.OnPlayerSwing += SwingSword;
        playerHp.OnPlayerCharging += ChargeSword;
    }

    void Update()
    {
        if (!canSwing)
        {
            checkCoolTime += Time.deltaTime;
            if (checkCoolTime > weaponCoolDown)
            {
                checkCoolTime = 0f;
                canSwing = true;
            }
        }
    }

    void SwingSword(float chargingTime)
    {
        this.damage = chargingTime * damageMul;
        isSwing = true;
        canSwing = false;
        StartCoroutine(SwingCoroutine(rotateSpeed));
    }

    void ChargeSword(float chargingTime)
    {
        if (canSwing)
        {
            //swordHandle.transform.localRotation = Quaternion.Euler(Vector3.zero);
            float swordScaleX = chargingTime * limitScaleX / 2;
            float swordScaleZ = chargingTime * limitScaleZ / 2;
            if (swordScaleX > limitScaleX) { swordScaleX = limitScaleX; }
            if (swordScaleZ > limitScaleZ) { swordScaleZ = limitScaleZ; }
            Vector3 swordScale = new Vector3(swordScaleX, swordScaleOffset.y, swordScaleZ);
            sword.transform.localScale = swordScale;
            Vector3 swordPos = sword.transform.localPosition;
            Vector3 movePos = new Vector3(swordScaleX / 2 + 1.5f, swordPos.y, swordPos.z);
            sword.transform.localPosition = movePos;
        }
    }

    IEnumerator SwingCoroutine(float duration)
    {
        float startAngle = swordHandle.transform.localEulerAngles.y;
        float endAngle = startAngle - 180f;
        if (!isRightSwing)
        {
            endAngle = startAngle + 180f;
            isRightSwing = true;
        }
        else { isRightSwing = false; }

        float time = 0f;
        while (time < swingSpeed)
        {
            time += Time.deltaTime;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, time / swingSpeed);
            swordHandle.transform.localRotation = Quaternion.Euler(new Vector3(0f, currentAngle, 0f));
            yield return null;
        }
        var swordCol = sword.GetComponent<BoxCollider>();
        sword.transform.localScale = swordScaleOffset;
        sword.transform.localPosition = new Vector3(1.5f, 0f, 0f);

        isSwing = false;
    }
}
