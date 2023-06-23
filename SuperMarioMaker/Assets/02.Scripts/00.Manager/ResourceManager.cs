using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        LoadAllPrefabsInFolder("Prefabs");
    }

    private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

    [SerializeField] private List<string> targetTags;

    private void LoadAllPrefabsInFolder(string folderPath)
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>(folderPath);

        foreach (GameObject prefab in prefabs)
        {
            // �������� �±װ� ���ϴ� �±� �� �ϳ��� ��ġ�ϴ��� Ȯ���մϴ�.
            if (targetTags.Contains(prefab.tag))
            {
                prefabDict[prefab.name] = prefab;
                Logger.Debug("Load Prefeb : " + prefab.name);
            }
            else
            {
                Logger.Debug("Unload Prefeb : " + prefab.name);
            }
        }
    }

    public void SpawnPrefabByName(string name, Vector3 position)
    {
        if (prefabDict.ContainsKey(name))
        {
            Instantiate(prefabDict[name], position, prefabDict[name].transform.rotation);
        }
        else
        {
            Debug.LogError("No prefab of name: " + name);
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }
}
