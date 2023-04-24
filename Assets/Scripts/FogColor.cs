using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogColor : MonoBehaviour
{
    [SerializeField] Material sun;
    // Start is called before the first frame update
    void Update()
    {
        RenderSettings.fogColor = sun.GetColor("_HorizonColor");
    }
}
