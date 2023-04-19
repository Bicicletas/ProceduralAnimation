using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogColor : MonoBehaviour
{
    [SerializeField] Material skybox;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.fogColor = skybox.GetColor("_HorizonColor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
