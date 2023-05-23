using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] Vector2 minMaxValue;

    [SerializeField] Light flashlight;
    [SerializeField] float speed;
    float value = 50;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && PlayerController.canMove)
        {
            if (flashlight.enabled)
            {
                flashlight.enabled = false;
            }
            else
            {
                flashlight.enabled = true;
            }
        }


        if (flashlight.enabled)
        {
            float increment = Input.GetAxisRaw("Mouse ScrollWheel") * speed;

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && value < minMaxValue.y)
            {
                value += increment;
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f && value > minMaxValue.x)
            {
                value += increment;
            }

            flashlight.intensity = value;
        }
    }
}
