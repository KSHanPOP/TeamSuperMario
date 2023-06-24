using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    [SerializeField]
    private IconButtonSetting tiles;
    [SerializeField]
    private IconButtonSetting activeTiles;
    [SerializeField]
    private IconButtonSetting item;
    [SerializeField]
    private IconButtonSetting Monster;
    [SerializeField]
    private IconButtonSetting Gimmik;

    [SerializeField]
    private GameObject iconPopup;
    [SerializeField]
    private Button popupExitButton;

    private string nowTag;

    public string NowTag
    {
        get { return nowTag; }
        set
        {
            nowTag = value;

            tiles.SetOutline(false);
            activeTiles.SetOutline(false);
            item.SetOutline(false);
            Monster.SetOutline(false);
            Gimmik.SetOutline(false);

            nowName = nowNames[nowTag];

            if (nowTag == "None")
            {
                return;
            }
            else if (nowTag == "Platform")
            {
                tiles.SetOutline(true);
            }
            else if (nowTag == "DynamicTile")
            {
                activeTiles.SetOutline(true);
            }
            else if (nowTag == "Item")
            {
                item.SetOutline(true);
            }
            else if (nowTag == "Monster")
            {
                Monster.SetOutline(true);
            }
            else if (nowTag == "Gimmik")
            {
                Gimmik.SetOutline(true);
            }
        }
    }

    private void Start()
    {
        IconInit();
    }

    [SerializeField]
    private GameObject buttonPrefab; // 미리 만들어 둔 Button 프리팹
    [SerializeField]
    private Transform scrollViewContent; // Scroll View의 Content 객체
    private int buttonCount; // 생성할 버튼의 개수
    public void IconInit()
    {
        buttonCount = MaxIconCount();

        for (int i = 0; i < buttonCount; i++)
        {
            var newButton = Instantiate(buttonPrefab, scrollViewContent);
            newButton.name = "Button" + (i + 1); // 각 버튼의 이름을 설정합니다.
        }
        popupExitButton.onClick.AddListener(() => ExitPopup());
        iconPopup.SetActive(false);
    }

    private List<string> platformName = new List<string>();
    private List<string> dynamicTileName = new List<string>();
    private List<string> itemName = new List<string>();
    private List<string> monsterName = new List<string>();
    private List<string> gimmikName = new List<string>();

    private Dictionary<string, string> nowNames = new Dictionary<string, string>();
    private string nowName;
    public string NowName
    {
        get { return nowName; }
        set { nowName = value; }
    }
    public void SetIconName()
    {
        var Platform = ResourceManager.instance.GetAllPrefabsWithTag("Platform");
        foreach (var name in Platform)
        {
            platformName.Add(name.name);
        }
        nowNames.Add("Platform", platformName[0]);

        var DynamicTile = ResourceManager.instance.GetAllPrefabsWithTag("DynamicTile");
        foreach (var name in DynamicTile)
        {
            dynamicTileName.Add(name.name);
        }
        nowNames.Add("DynamicTile", dynamicTileName[0]);

        var Item = ResourceManager.instance.GetAllPrefabsWithTag("Item");
        foreach (var name in Item)
        {
            itemName.Add(name.name);
        }
        nowNames.Add("Item", itemName[0]);

        var Monster = ResourceManager.instance.GetAllPrefabsWithTag("Monster");
        foreach (var name in Monster)
        {
            monsterName.Add(name.name);
        }
        nowNames.Add("Monster", monsterName[0]);

        var Gimmik = ResourceManager.instance.GetAllPrefabsWithTag("Gimmik");
        foreach (var name in Gimmik)
        {
            gimmikName.Add(name.name);
        }
        nowNames.Add("Gimmik", gimmikName[0]);

    }
    public List<string> GetIconName(string tag)
    {
        switch (tag)
        {
            case "Platform":
                return platformName;
            case "DynamicTile":
                return dynamicTileName;
            case "Item":
                return itemName;
            case "Monster":
                return monsterName;
            case "Gimmik":
                return gimmikName;
            default:
                return null;
        }
    }

    string[] folderPaths = new string[]
{
    "Sprite/Icon/StaticTileIcon",
    "Sprite/Icon/DynamicTileIcon",
    "Sprite/Icon/ItemIcon",
    "Sprite/Icon/MonsterIcon",
    "Sprite/Icon/GimmikIcon",
};
    public int MaxIconCount()
    {
        int maxSpriteCount = 0;

        foreach (string folderPath in folderPaths)
        {
            Object[] sprites = Resources.LoadAll(folderPath, typeof(Sprite));
            if (sprites.Length > maxSpriteCount)
            {
                maxSpriteCount = sprites.Length;
            }
        }
        return maxSpriteCount;
    }

    public void SetIconPopup(string iconTag)
    {
        iconPopup.SetActive(true);

        SetActivePopupButtons(iconTag);

        if (nowTag == "Platform")
        {
            tiles.SetOutline(true);
        }
        else if (nowTag == "DynamicTile")
        {
            activeTiles.SetOutline(true);
        }
        else if (nowTag == "Item")
        {
            item.SetOutline(true);
        }
        else if (nowTag == "Monster")
        {
            Monster.SetOutline(true);
        }
        else if (nowTag == "Gimmik")
        {
            Gimmik.SetOutline(true);
        }
    }

    public void SetActivePopupButtons(string iconTag)
    {
        var prefaps = ResourceManager.instance.GetAllPrefabsWithTag(nowTag);

        List<Sprite> sprites = new List<Sprite>();

        foreach (var prefap in prefaps)
        {
            sprites.Add(Resources.Load<Sprite>(prefap.GetComponent<PrefapInfo>().IconSpritePath));
        }

        var length = sprites.Count;

        for (int i = 0; i < scrollViewContent.childCount; i++)
        {
            GameObject buttonObj = scrollViewContent.GetChild(i).gameObject;

            if (i < length)
            {
                Sprite sprite = sprites[i] as Sprite;
                buttonObj.GetComponent<Image>().sprite = sprite;

                // Get Button component and add listener
                Button button = buttonObj.GetComponent<Button>();
                button.onClick.AddListener(() => OnPopupButtonClicked(button));

                buttonObj.SetActive(true);
                buttonObj.name = prefaps[i].name;
            }
            else
            {
                buttonObj.SetActive(false);
                buttonObj.name = " ";
            }
        }
    }

    public void OnPopupButtonClicked(Button button)
    {
        // Do something when a Popupbutton is clicked
        Debug.Log(button.name + " was clicked!");

        var buttonImage = button.GetComponent<Image>().sprite;

        if (nowTag == "Platform")
        {
            tiles.ChangeImage(buttonImage);
        }
        else if (nowTag == "DynamicTileIcon")
        {
            activeTiles.ChangeImage(buttonImage);
        }
        else if (nowTag == "Item")
        {
            item.ChangeImage(buttonImage);
        }
        else if (nowTag == "Monster")
        {
            Monster.ChangeImage(buttonImage);
        }
        else if (nowTag == "Gimmik")
        {
            Gimmik.ChangeImage(buttonImage);
        }

        nowNames[nowTag] = button.name;

        iconPopup.SetActive(false);
    }

    public void ExitPopup()
    {
        iconPopup.SetActive(false);
    }
}
