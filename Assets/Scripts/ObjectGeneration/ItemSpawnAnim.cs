using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnAnim : MonoBehaviour
{
    private Material mat;
    Color a;
    [SerializeField] float speed = 1;

    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        mat.opt
        a = mat.color;
        a.a = 0;
    }

    private void Update()
    {
        if(a.a < 1) 
        {
            a.a += Time.deltaTime * speed;
            mat.color = a;
        }
    }
}
