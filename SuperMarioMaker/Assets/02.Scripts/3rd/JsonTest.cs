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
    public string Time { get; set; }

    [JsonProperty("life")]
    public string Life { get; set; }


    [JsonProperty("BackGroundName")]
    public string BackGroundName { get; set; }

    [JsonProperty("MapRowLength")]
    public string MapRowLength { get; set; }

    [JsonProperty("MapColLength")]
    public string MapColLength { get; set; }

    [JsonProperty("tiles")]
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
    public void MakeADefaultMapData()
    {
        var mapData = new MapData
        {
            MapName = "Defalt",
            Tiles = new List<TileData>()
        };
        for (int x = 0; x < tilemapRow; x++)
        {
            for (int y = 0; y < tilemapCol; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                //TileBase tileBase = tilemap.GetTile(pos);
                //CustomTile customTile = tileBase as CustomTile;
                var name = CClickChangeTile.GetCustomTileName(pos);
                //mapData.Tiles.Add(new TileData { X = x, Y = y, TileType = name });
            }
        }
        SaveMapData(mapData);
    }

    public void SaveMapData(MapData mapData)
    {
        string json = JsonConvert.SerializeObject(mapData);
        System.IO.File.WriteAllText("DefaltMapData.jon", json);
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
            //LoadMapData("DefaltMapData.jon", true);
            //Logger.Debug("로드 완");
            // MakeADefaultMapData();
            //Logger.Debug("세이브 완");
        }
    }
}


//public int[] TestVariable;
//private void Start()
//{
//    var result = JsonConvert.SerializeObject(TestVariable);//Convert to json
//    Debug.Log(result);
//}

//public int i;
//public float f;
//public bool b;
//public string str;
//public int[] iArray;
//public List<int> iList = new List<int>();
//public Dictionary<string, float> fDictionary = new Dictionary<string, float>();
//public JsonTest() { }
//public JsonTest(bool isSet)
//{

//    if (isSet)

//    {
//        i = 10;
//        f = 99.9f;
//        b = true;
//        str = "JSON Test String";
//        iArray = new int[] { 1, 1, 3, 5, 8, 13, 21, 34, 55 };

//        for (int idx = 0; idx < 5; idx++)
//        {
//            iList.Add(2 * idx);
//        }

//        fDictionary.Add("PIE", Mathf.PI);
//        fDictionary.Add("Epsilon", Mathf.Epsilon);
//        fDictionary.Add("Sqrt(2)", Mathf.Sqrt(2));

//    }
//}
//void Start()
//{
//    JsonTest jtc = new JsonTest(true);
//    string jsonData = ObjectToJson(jtc);
//    CreateJsonFile(Application.dataPath, "JTestClass", jsonData);
//}

//string ObjectToJson(object obj)
//{
//    return JsonConvert.SerializeObject(obj);
//}

//T JsonToOject<T>(string jsonData)
//{
//    return JsonConvert.DeserializeObject<T>(jsonData);
//}

//void CreateJsonFile(string createPath, string fileName, string jsonData)
//{
//    FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
//    byte[] data = Encoding.UTF8.GetBytes(jsonData);
//    fileStream.Write(data, 0, data.Length);
//    fileStream.Close();
//}
