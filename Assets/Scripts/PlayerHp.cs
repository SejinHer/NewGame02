using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerHp : MonoBehaviour
{
    public float maxHp = 100;
    public float hp = 100;
    private InputActions inputActions;

    public float consumePerSecond = 10f;
    private bool isCharging = false;
    private float chargingTime;
    private float swingSpeed = 0.3f;

    public GameObject sword;
    private Vector3 swordScaleOffset = new Vector3(0.3f, 0.3f, 0.3f);

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Awake()
    {
        inputActions = new InputActions();

    }
    private void Start()
    {
        inputActions.Player.Attack.started += StartBloodSword;
        inputActions.Player.Attack.canceled += SwingBloodSword;
    }

    private void StartBloodSword(InputAction.CallbackContext ctx)
    {
        sword.transform.localScale = swordScaleOffset;
        isCharging = true;
    }

    private void SwingBloodSword(InputAction.CallbackContext ctx)
    {
        isCharging = false;
        Debug.Log($"Charging Time : {chargingTime}");
        StartCoroutine(SwingCoroutine(0.2f));
    }

    IEnumerator SwingCoroutine(float rotateSpeed)
    {
        float startAngle = transform.localEulerAngles.y;
        float endAngle = startAngle - 180f;
        float time = 0f;
        while (time < swingSpeed)
        {
            time += Time.deltaTime;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, time / rotateSpeed);
            transform.localRotation = Quaternion.Euler(new Vector3(0f, currentAngle, 0f));
            yield return null;
        }
        sword.transform.localScale = swordScaleOffset;
        sword.transform.localPosition = new Vector3(swordScaleOffset.x / 2, 0f, 0f);
        chargingTime = 0;
    }

    private void Update()
    {
        if (isCharging)
        {
            chargingTime += consumePerSecond * Time.deltaTime;
            hp -= consumePerSecond * Time.deltaTime;
            Vector3 swordScale = new Vector3(chargingTime, swordScaleOffset.y, swordScaleOffset.z);
            sword.transform.localScale = swordScale;
            Vector3 swordPos = sword.transform.localPosition;
            Vector3 movePos = new Vector3(chargingTime / 2, swordPos.y, swordPos.z);
            sword.transform.localPosition = movePos;
        }

        if(hp <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}
