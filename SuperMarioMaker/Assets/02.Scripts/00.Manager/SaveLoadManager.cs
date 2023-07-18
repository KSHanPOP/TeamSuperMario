using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }

    List<MapData> mapDatas= new List<MapData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadAllMapData();
    }

    public List<MapData> LoadAllMapData()
    {
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "SaveData");
        var mapDataList = new List<MapData>();

        // Check if the directory exists
        if (Directory.Exists(folderPath))
        {
            // Get all .json files in the directory
            string[] files = Directory.GetFiles(folderPath, "*.json");

            // Loop through each file
            foreach (string file in files)
            {
                // Read the file content
                string json = File.ReadAllText(file);

                // Deserialize the JSON string to a MapData object
                MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

                // Add the MapData object to the list
                mapDataList.Add(mapData);
            }
        }

        return mapDataList;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
