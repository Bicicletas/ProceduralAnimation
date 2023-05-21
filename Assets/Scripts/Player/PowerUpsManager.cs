using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpsManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Space]
    [SerializeField] Image[] powerUpsImages;
    [SerializeField] Image[] bgImages;
    [SerializeField] TextMeshProUGUI[] amountTexts;

    [Space]

    [SerializeField] Color bgSelectedColor;
    Color bgStartColor;

    [Space(20)]

    [SerializeField] float speedBoostStartTime;
    [SerializeField] float jumpBoostStartTime;

    [Space(20)]

    public float speedBoostCurrentTime;
    public float jumpBoostCurrentTime;

    [Space(20)]

    public bool speedBoostTimerOn;
    public bool jumpBoostTimerOn;

    [Space(20)]

    public float speedStartValue = 0;
    public float jumpStartValue = 0;

    int powerUpSelected = 0;

    private void Start()
    {
        speedStartValue = PlayerController.instance.force;
        jumpStartValue = PlayerController.instance.jumpForce;

        UpdateUI(0, ShopMenu.instance.speedBoostAmount);
        UpdateUI(1, ShopMenu.instance.jumpBoostAmount);

        bgStartColor = bgImages[0].color;

        bgImages[powerUpSelected].color = bgSelectedColor;
    }

    void UpdateUI(int index, int amount)
    {
        amountTexts[index].text = "" + amount;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && powerUpSelected < powerUpsImages.Length - 1)
        {
            bgImages[powerUpSelected].color = bgStartColor;
            powerUpSelected++;
            bgImages[powerUpSelected].color = bgSelectedColor;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && powerUpSelected > 0)
        {
            bgImages[powerUpSelected].color = bgStartColor;
            powerUpSelected--;
            bgImages[powerUpSelected].color = bgSelectedColor;
        }

        if (ShopMenu.instance.speedBoostAmount > 0)
        {
            if(amountTexts[0].text != "" + ShopMenu.instance.speedBoostAmount)
            {
                UpdateUI(0, ShopMenu.instance.speedBoostAmount);
            }

            if (powerUpSelected == 0)
            {
                if (Input.GetKeyDown(KeyCode.Q) && !speedBoostTimerOn)
                {
                    PlayerController.instance.force += ShopMenu.instance.speedMult * ShopMenu.instance.speedBoostAmount;

                    speedBoostCurrentTime = speedBoostStartTime;

                    speedBoostTimerOn = true;
                }
            }

            if (speedBoostTimerOn)
            {
                speedBoostCurrentTime -= Time.deltaTime;

                float fillAmount = speedBoostCurrentTime / speedBoostStartTime;

                powerUpsImages[0].fillAmount = fillAmount;

                if (speedBoostCurrentTime <= speedBoostStartTime / 2)
                {
                    PlayerController.instance.force = speedStartValue;

                    powerUpsImages[0].color = Color.red;
                }

                if (speedBoostCurrentTime <= 0)
                {
                    speedBoostTimerOn = false;

                    powerUpsImages[0].color = Color.white;

                    powerUpsImages[0].fillAmount = 1;
                }
            }
        }

        if (ShopMenu.instance.jumpBoostAmount > 0)
        {
            if (amountTexts[1].text != "" + ShopMenu.instance.jumpBoostAmount)
            {
                UpdateUI(1, ShopMenu.instance.jumpBoostAmount);
            }

            if (powerUpSelected == 1)
            {
                if (Input.GetKeyDown(KeyCode.Q) && !jumpBoostTimerOn)
                {
                    PlayerController.instance.jumpForce += ShopMenu.instance.jumpMult * ShopMenu.instance.jumpBoostAmount;

                    PlayerController.instance.JumpMechanic();

                    PlayerController.instance._playerAnimator.Play("Jump");

                    PlayerController.instance._playerAnimator.SetBool("IsJumping", true);

                    PlayerController.instance.jumpForce = jumpStartValue;

                    powerUpsImages[1].color = Color.red;

                    jumpBoostCurrentTime = jumpBoostStartTime;

                    jumpBoostTimerOn = true;
                }
            }

            if (jumpBoostTimerOn)
            {
                jumpBoostCurrentTime -= Time.deltaTime;

                float fillAmount = jumpBoostCurrentTime / jumpBoostStartTime;

                powerUpsImages[1].fillAmount = fillAmount;

                if (jumpBoostCurrentTime <= 0)
                {
                    jumpBoostTimerOn = false;

                    powerUpsImages[1].color = Color.white;

                    powerUpsImages[1].fillAmount = 1;
                }
            }
        }
    }
}
