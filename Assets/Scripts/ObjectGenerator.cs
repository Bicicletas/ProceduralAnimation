using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] float spawnDelay = 0.8f;
    [SerializeField] float sizeOffset = 50;
    [SerializeField] float rayOriginY = 80;    

    [SerializeField] Settings[] settings;

    [HideInInspector] public List<Vector3> positions = new List<Vector3>();

    private GameObject objectPosPanel;

    [SerializeField] GameObject posPanel;

    public static List<GameObject> panelList = new List<GameObject>();

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

    Vector3 pos = new Vector3(0, 0, 0);

    void GenerateObjects()
    {
        objectPosPanel = GameObject.Find("ObjectPositions");

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

                        if (s.randomScale == 0)
                        {
                            InstantiateObject(s, instance, hit);
                        }
                        else if (s.RandomizedSpawnRate())
                        {
                            InstantiateObject(s, instance, hit);

                            if(panelList.Count <= 5)
                            {
                                GameObject panelInstance = Instantiate(posPanel, objectPosPanel.transform);

                                if (panelInstance != null)
                                {
                                    panelInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Item at " + Mathf.RoundToInt(pos.x)+ ", " + Mathf.RoundToInt(pos.y) + ", " + Mathf.RoundToInt(pos.z);
                                    panelList.Add(panelInstance);
                                    print(panelList.Count);
                                }
                            }
                            else
                            {
                                Destroy(panelList[0]);
                                panelList.Remove(panelList[0]);
                            }
                        }
                    }
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

        pos = instance.transform.position;

        float scale = s.RandomScale();

        instance.transform.localScale = new Vector3(scale, scale, scale);
    }
}
