using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    public GameObject waterChunk;

    private void Start()
    {
        Invoke(nameof(AddMeshCollider), 1.1f);

        GameObject instance = Instantiate(waterChunk, new Vector3(transform.position.x, waterChunk.transform.position.y, transform.position.z), waterChunk.transform.rotation);

        instance.transform.parent = transform;

        //instance.name += instance.transform.position.x.ToString();

        //SetVector3PlayerPref(transform.GetChild(0).name, instance.transform.position);
    }

    void SetVector3PlayerPref(string key, Vector3 objPos)
    {
        PlayerPrefs.SetFloat(key + "x", objPos.x);
        PlayerPrefs.SetFloat(key + "y", objPos.y);
        PlayerPrefs.SetFloat(key + "z", objPos.z);
    }

    public Vector3 GetVector3PlayerPref(string key)
    {
        return new Vector3(PlayerPrefs.GetFloat(key + "x"), PlayerPrefs.GetFloat(key + "y"), PlayerPrefs.GetFloat(key + "z"));
    }

    void AddMeshCollider()
    {
        MeshFilter mf = GetComponent<MeshFilter>();

        MeshCollider mc = GetComponent<MeshCollider>();

        if (mc.sharedMesh != mf.sharedMesh)
        {
            mc.sharedMesh = mf.sharedMesh;
        }
    }
}
