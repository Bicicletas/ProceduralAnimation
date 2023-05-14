using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectGenerator : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float repeatSpawnRate = 0.8f;
    [SerializeField] float sizeOffset = 50;
    [SerializeField] float rayOriginY = 80;
    [SerializeField] int maxObjectsPerChunk = 10;

    [SerializeField] Settings[] settings;

    bool canSpawn = true;

    private void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            StartCoroutine(InvokeRepeating());
        }
        //InvokeRepeating("PassSettings", spawnDelay, repeatSpawnRate);
    }

    IEnumerator InvokeRepeating()
    {
        PassSettings();
        yield return new WaitForSeconds(repeatSpawnRate);
        canSpawn = true;
    }

    Vector2 RandomPos(Vector3 parentPos)
    {
        //Vector2 randomPos = Random.insideUnitCircle;

        float x = Random.Range(parentPos.x - sizeOffset, parentPos.x + sizeOffset);
        float z = Random.Range(parentPos.z - sizeOffset, parentPos.z + sizeOffset);

        /*
        float ranX = randomPos.x;
        float ranY = randomPos.y;

        float x = (parentPos.x + sizeOffset) * (Mathf.Abs(ranX) > radius ? ranX : ranX > 0 ? radius : -radius);
        float z = (parentPos.z + sizeOffset) * (Mathf.Abs(ranY) > radius ? ranY : ranY > 0 ? radius : -radius);
        */

        return new Vector2(x, z);

        
    }

    void PassSettings()
    {
        foreach(Settings s in settings)
        {
            GenerateObjects(s);
        }
    }

    void GenerateObjects(Settings s)
    {

        Vector3 position = new Vector3(RandomPos(transform.position).x, rayOriginY, RandomPos(transform.position).y);

        RaycastHit hit;

        if (Mathf.Abs(position.x) > (Mathf.Abs(transform.position.x) + sizeOffset) / 2 && Mathf.Abs(position.z) > (Mathf.Abs(transform.position.z) + sizeOffset) / 2)
        {
            //print(position + "instanced" + transform.position);
            
        }
        else
        {
            //print(position);
        }

        if (Physics.Raycast(position, Vector3.down, out hit, 100f, s.whatIsGround))
        {
            if (hit.point.y > s.minObjectSpawnThreshold && hit.point.y < s.maxObjectSpawnThreshold)
            {
                if (hit.transform.childCount >= maxObjectsPerChunk)
                {
                    return;
                }

                GameObject instance = null;

                if (s.randomScale == 0)
                {
                    InstantiateObject(s, instance, hit);
                }
                else if (s.RandomizedSpawnRate())
                {
                    InstantiateObject(s, instance, hit);
                }
            }
        }
    }

    void InstantiateObject(Settings s, GameObject instance, RaycastHit hit)
    {
        GameObject @object = s.objects[s.RandomIndex()];

        instance = Instantiate(@object, new Vector3(hit.point.x, hit.point.y + s.objectSpawnOffsetY, hit.point.z),
            Quaternion.Euler(@object.transform.eulerAngles.x, @object.transform.eulerAngles.y + s.RandomRotation(), @object.transform.eulerAngles.z));

        if (!s.worldInstance)
        {
            instance.transform.parent = hit.transform;
        }

        float scale = s.RandomScale();

        if (s.childInstanceScale)
        {
            instance.transform.GetChild(0).localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            instance.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("SavedGame");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sizeOffset);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sizeOffset / 2);
    }
}
