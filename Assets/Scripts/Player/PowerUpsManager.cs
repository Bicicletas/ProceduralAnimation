using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpsManager : MonoBehaviour
{
    [SerializeField] Image[] powerUpsImages;

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

    // Update is called once per frame
    void Update()
    {
        if (ShopMenu.instance.speedBoostAmount > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && !speedBoostTimerOn)
            {
                speedStartValue = PlayerController.instance.force;

                PlayerController.instance.force += ShopMenu.instance.speedMult * ShopMenu.instance.speedBoostAmount;

                speedBoostCurrentTime = speedBoostStartTime;

                speedBoostTimerOn = true;
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
            if (Input.GetKeyDown(KeyCode.Alpha2) && !jumpBoostTimerOn)
            {
                speedStartValue = PlayerController.instance.jumpForce;

                PlayerController.instance.jumpForce += ShopMenu.instance.jumpMult * ShopMenu.instance.jumpBoostAmount;

                jumpBoostCurrentTime = jumpBoostStartTime;

                jumpBoostTimerOn = true;
            }

            if (jumpBoostTimerOn)
            {
                jumpBoostCurrentTime -= Time.deltaTime;

                float fillAmount = jumpBoostCurrentTime / jumpBoostStartTime;

                powerUpsImages[1].fillAmount = fillAmount;

                if (jumpBoostCurrentTime <= jumpBoostStartTime / 2)
                {
                    PlayerController.instance.jumpForce = jumpStartValue;

                    powerUpsImages[1].color = Color.red;
                }

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
