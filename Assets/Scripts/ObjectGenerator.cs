using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] float spawnDelay = 0.8f;
    [SerializeField] float sizeOffset = 50;
    [SerializeField] float rayOriginY = 80;    

    [SerializeField] Settings[] settings;

    [HideInInspector] public List<Vector3> positions = new List<Vector3>();

    int objAmount = 0;

    private void Start()
    {
        Invoke(nameof(GenerateObjects), spawnDelay);
    }

    Vector2 RandomPos(Vector3 parentPos)
    {
        float x = Random.Range(parentPos.x - sizeOffset, parentPos.x + sizeOffset);
        float z = Random.Range(parentPos.z - sizeOffset, parentPos.z + sizeOffset);

        return new Vector2(x, z);
    }

    void GenerateObjects()
    {
        foreach(Settings s in settings)
        {
            for (int i = 0; i < s.numOfObject; i++)
            {
                positions.Add(new Vector3(RandomPos(transform.parent.position).x, rayOriginY, RandomPos(transform.parent.position).y));

                RaycastHit hit;

                if (Physics.Raycast(positions[i], Vector3.down, out hit, 100f, s.whatIsGround))
                {
                    if (hit.point.y > s.minObjectSpawnThreshold && hit.point.y < s.maxObjectSpawnThreshold)
                    {
                        GameObject instance = null;

                        if (s.randomSpawn.randomScale == 0)
                        {
                            InstantiateObject(s, instance, hit);
                        }
                        else if (s.randomSpawn.RandomizedSpawnRate())
                        {
                            InstantiateObject(s, instance, hit);

                            objAmount++;
                        }
                    }
                }
            }
        }

        print(objAmount);
    }

    void InstantiateObject(Settings s, GameObject instance, RaycastHit hit)
    {
        GameObject @object = s.objects[s.RandomIndex()];

        instance = Instantiate(@object, new Vector3(hit.point.x, hit.point.y + s.objectSpawnOffsetY, hit.point.z),
            Quaternion.Euler(@object.transform.eulerAngles.x, @object.transform.eulerAngles.y + s.RandomRotation(), @object.transform.eulerAngles.z));

        instance.transform.parent = hit.transform;

        float scale = s.RandomScale();

        instance.transform.localScale = new Vector3(scale, scale, scale);
    }
}
