using UnityEngine;

public class ItemSpawnManagers : MonoBehaviour
{
    private static ItemSpawnManagers instance;
    public static ItemSpawnManagers Instance { get { return instance; } }

    [Header("just show item list, do nothing in script")]
    public EnumItems item;

    public GameObject[] prefabs;

    private void Awake()
    {
        instance = this;
    }
}
