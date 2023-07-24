using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }

    private Dictionary<string, SaveFile> saveFiles = new Dictionary<string, SaveFile>();
   public Dictionary<string, SaveFile> SaveFiles
    {
        get { return saveFiles; }
    }
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

        LoadAllMapDataAndSprites();
    }

    private string GetPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "SaveData");
    }

    public void LoadAllMapDataAndSprites()
    {
        string folderPath = GetPath();

        // Check if the directory exists
        if (Directory.Exists(folderPath))
        {
            // Get all .json files in the directory
            string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

            // Loop through each .json file
            foreach (string jsonFile in jsonFiles)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jsonFile);
                string pngFilePath = Path.Combine(folderPath, fileNameWithoutExtension + ".png");

                if (File.Exists(pngFilePath))
                {
                    // Deserialize the JSON string to a MapData object
                    string json = File.ReadAllText(jsonFile);
                    MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

                    // Load the PNG as a Sprite
                    byte[] fileData = File.ReadAllBytes(pngFilePath);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(fileData);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    // Add to the dictionary
                    SaveFile saveFile = new SaveFile
                    {
                        mapData = mapData,
                        screenshot = sprite
                    };
                    saveFiles[fileNameWithoutExtension] = saveFile;
                }
            }
        }
    }


    [SerializeField] private GameObject popUp;
    public GameObject PopUp
    {
        get { return popUp; }
    }
    [SerializeField] private Button popUpExitButton;

    [SerializeField] private GameObject buttonPrefab; // ¹Ì¸® ¸¸µé¾î µÐ Button ÇÁ¸®ÆÕ
    [SerializeField] private Transform scrollViewContent;

    public void SetLoadList()
    {
        for(int i = 0; i < scrollViewContent.childCount; i++)
        {
            Destroy(scrollViewContent.GetChild(0).gameObject);
        }

        foreach (var saveFile in saveFiles)
        {
            var loadIcon = Instantiate(buttonPrefab, scrollViewContent);
            loadIcon.name = saveFile.Key;

            // Find the child object named "Screenshot" in the loadIcon object
            Transform screenshotChild = loadIcon.transform.Find("Screenshot");

            if (screenshotChild != null)
            {
                Image screenshotImage = screenshotChild.GetComponent<Image>();
                if (screenshotImage != null)
                {
                    screenshotImage.sprite = saveFile.Value.screenshot;
                }
                else
                {
                    Logger.Debug("No Image component found on 'Screenshot' child object of " + loadIcon.name);
                }
            }
            else
            {
                Logger.Debug("'Screenshot' child object not found in " + loadIcon.name);
            }

            loadIcon.GetComponent<Image>().sprite = saveFile.Value.screenshot;
        }
    }
    public void LoadGame(string name)
    {
        SceneLoader.Instance.LoadMainGameScene(name);
        popUp.SetActive(false);
    }
    public void Delete(string name)
    {
        string folderPath = GetPath();

        string jsonFilePath = Path.Combine(folderPath, name + ".json");
        if (File.Exists(jsonFilePath))
        {
            File.Delete(jsonFilePath);
        }
        else
        {
            Debug.LogWarning($"File not found: {jsonFilePath}");
        }

        string pngFilePath = Path.Combine(folderPath, name + ".png");
        if (File.Exists(pngFilePath))
        {
            File.Delete(pngFilePath);
        }
        else
        {
            Debug.LogWarning($"File not found: {pngFilePath}");
        }

        // Remove the SaveFile from the saveFiles dictionary
        if (saveFiles.ContainsKey(name))
        {
            saveFiles.Remove(name);
        }

        // Find the button in the scroll view and destroy it
        foreach (Transform child in scrollViewContent)
        {
            if (child.name == name)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }
    public void PopupExit()
    {
        popUp.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        popUpExitButton.onClick.AddListener(PopupExit);
    }
    void OnEnable()
    {
        SetLoadList();
    }

    public void ReloadMapList()
    {
        LoadAllMapDataAndSprites();
        SetLoadList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class SaveFile
{
    public MapData mapData = new MapData();
    public Sprite screenshot = null;
}