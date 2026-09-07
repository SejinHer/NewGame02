using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using TMPro;
using UnityEngine.Rendering;

public class PlayerHp : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public float maxHp = 100;
    public float hp = 100;
    private InputActions inputActions;
    private bool isCharging = false;
    private float chargingTime;

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

        UpdateHpUI();
    }

    private void StartBloodSword(InputAction.CallbackContext ctx)
    {
        isCharging = true;
    }

    private void SwingBloodSword(InputAction.CallbackContext ctx)
    {
        isCharging = false;
        Debug.Log($"Charging Time : {chargingTime}");
        OnPlayerSwing?.Invoke(chargingTime);
        chargingTime = 0;
    }

    private void Update()
    {
        if (isCharging)
        {
            chargingTime += Time.deltaTime;
            //hp -= consumePerSecond * Time.deltaTime;
            OnPlayerCharging?.Invoke(chargingTime);

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

    void UpdateHpUI()
    {
        hpText.text = $"{hp} / {maxHp}";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && !isUnbeatable)
        {
            hp -= other.GetComponentInParent<AttackController>().damage;
            UpdateHpUI();
            isUnbeatable = true;
            if (hp <= 0f)
            {
                Debug.Log("Game Over");
            }
        }
    }
}
