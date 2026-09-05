using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class PlayerHp : MonoBehaviour
{
    public float maxHp = 100;
    public float hp = 100;
    private InputActions inputActions;

    public float consumePerSecond = 10f;
    private bool isCharging = false;
    private float chargingTime;

    public float chargeDamage = 0f;

    public event Action<float> OnPlayerSwing;
    public event Action<float> OnPlayerCharging;

    private bool isUnbeatable = false;

    public float unbeatableTime = 1f;
    private float unbeatableTimer = 0f;

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
        isCharging = true;
    }

    private void SwingBloodSword(InputAction.CallbackContext ctx)
    {
        isCharging = false;
        Debug.Log($"Charging Time : {chargingTime}");
        chargeDamage = chargingTime * 2f;
        OnPlayerSwing?.Invoke(chargeDamage);
        chargingTime = 0;
        chargeDamage = 0f;
    }

    private void Update()
    {
        if (isCharging)
        {
            chargingTime += consumePerSecond * Time.deltaTime;
            hp -= consumePerSecond * Time.deltaTime;
            OnPlayerCharging?.Invoke(chargingTime);

        }

        if (hp <= 0)
        {
            Debug.Log("Game Over");
        }

        if (isUnbeatable)
        {
            unbeatableTimer += Time.deltaTime;
            if (unbeatableTimer >= unbeatableTime)
            {
                isUnbeatable = false;
                unbeatableTimer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            hp -= other.GetComponent<AttackController>().damage;
            isUnbeatable = true;
            if (hp <= 0f)
            {
                Debug.Log("Game Over");
            }
        }
    }
}
