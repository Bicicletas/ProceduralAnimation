using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

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

        

        float increment = Input.GetAxisRaw("Mouse ScrollWheel") * speed;

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && value < 500)
        {
            value += increment;
        }
        
        if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f && value > 20)
        {
            value += increment;
        }

        flashlight.intensity = value;
    }
}
