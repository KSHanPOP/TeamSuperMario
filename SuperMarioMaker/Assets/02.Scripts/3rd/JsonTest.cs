using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class MapData
{
    [JsonProperty("mapname")]
    public string MapName { get; set; }

    [JsonProperty("time")]
    public float Time { get; set; }

    [JsonProperty("life")]
    public int Life { get; set; }


    [JsonProperty("BackGroundName")]
    public string BackGroundName { get; set; }

    [JsonProperty("MapRowLength")]
    public int MapRowLength { get; set; }

    [JsonProperty("MapColLength")]
    public int MapColLength { get; set; }

    [JsonProperty("Tiles")]
    public List<TileData> Tiles { get; set; }


}
public class GameObjectTileData
{
    public GameObject gameObject;
    public TileData tileData = new TileData();
}

public class TileData
{
    [JsonProperty("x")]
    public int X { get; set; }

    [JsonProperty("y")]
    public int Y { get; set; }

    [JsonProperty("tileType")]
    [JsonConverter(typeof(StringEnumConverter))]
    public ETileType TileType { get; set; }

    [JsonProperty("tileName")]
    public string TileName { get; set; }


    public override bool Equals(object obj)
    {
        if (obj is TileData other)
        {
            return X == other.X && Y == other.Y && TileType == other.TileType && TileName == other.TileName;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            hash = hash * 23 + TileType.GetHashCode();
            hash = hash * 23 + (TileName?.GetHashCode() ?? 0);
            return hash;
        }
    }
}


public class JsonTest : MonoBehaviour
{
    private int tilemapRow = 24;
    private int tilemapCol = 14;

    private int tilemapStartline = 7;
    private int tilemapEndline = 10;

    public ClickChangeTile CClickChangeTile;

    [SerializeField]
    private Tilemap tilemap;
    public void GameComplete()
    {
        SaveMapData(ToolManager.Instance.SetMapData());
    }

    public void SaveMapData(MapData mapData)
    {
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "SaveData");
        string fileName = mapData.MapName.Replace(":", "_") + ".json";
        string filePath = Path.Combine(folderPath, fileName);

        Directory.CreateDirectory(folderPath);

        string json = JsonConvert.SerializeObject(mapData, Formatting.Indented);
        System.IO.File.WriteAllText(filePath, json);

        string screenshotFileName = Path.ChangeExtension(filePath, ".png");
        ToolManager.Instance.gridMaker.CaptureMiniMap(screenshotFileName);
    }
    public MapData LoadMapData(string fileName, bool isDefalt = false)
    {
        string filePath;

        if (isDefalt)
        {
            filePath = Path.Combine(Path.GetFullPath(fileName));
        }
        else
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        if (File.Exists(filePath))
        {

            string dataAsJson = File.ReadAllText(filePath);
            MapData loadedData = JsonUtility.FromJson<MapData>(dataAsJson);
            return loadedData;
        }
        else
        {
            Logger.Debug("Cannot load game data!");
        }
        return null;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GameComplete();
        }
    }
}