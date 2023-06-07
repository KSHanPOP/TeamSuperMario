using UnityEngine;

[CreateAssetMenu(menuName = "CustomTiles/DynamicTile", fileName = "DynamicTile")]
public class DynamicTile : CustomTile
{
    [SerializeField]
    private GameObject prefab;
    private void OnEnable()
    {
        Logger.Debug("dynamicTileAwake");
        IngameObjectManager.Instance.DynamicTiles.Add(this);
    }
    public virtual void InstancePrefab()
    {
        Instantiate(prefab, transform.GetPosition(), Quaternion.identity);
        gameObject.SetActive(false);        
    }
}
