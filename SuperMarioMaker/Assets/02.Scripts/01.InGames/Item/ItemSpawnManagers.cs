using UnityEngine;

public class ItemSpawnManagers : MonoBehaviour
{
    private static ItemSpawnManagers instance;
    public static ItemSpawnManagers Instance { get { return instance; } }
    
    public GameObject[] prefabs;

    private void Awake()
    {
        instance = this;
    }
}
