using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] Material skybox;
    [SerializeField] Light sun;
    [SerializeField] Light sun2;
    [SerializeField] float speed = 1;

    [Range(0, 2)]
    [SerializeField] float fogMultiplier = 0.5f;
    float rotationSpeed;

    private void Update()
    {
        skybox.SetColor("_SkyColor", sun.color * 2f);
        skybox.SetColor("_HorizonColor", sun.color);
        skybox.SetColor("_GroundColor", skybox.GetColor("_HorizonColor"));
        skybox.SetFloat("_Smoothness", sun.intensity - 2);
        sun2.color = sun.color;
        //skybox.SetFloat("_Exposure", sun.intensity > 0.5f ? sun.intensity : 0.5f);
        //RenderSettings.fogColor = sun.color * fogMultiplier;
        RenderSettings.fogColor = skybox.GetColor("_HorizonColor") * fogMultiplier;
        RenderSettings.ambientLight = skybox.GetColor("_HorizonColor") * fogMultiplier;

        rotationSpeed += Time.deltaTime * speed;

        if(rotationSpeed >= 360)
        {
            rotationSpeed = 0;
        }
        skybox.SetFloat("_Rotation", Mathf.Clamp(rotationSpeed, 0, 360));
    }
}
