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
        if (Input.GetMouseButtonDown(0))
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

        print(increment);

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && value < 100)
        {
            value += increment;
        }
        
        if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f && value > 30)
        {
            value += increment;
        }

        flashlight.intensity = value;
    }
}
