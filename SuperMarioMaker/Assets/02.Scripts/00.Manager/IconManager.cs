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

    //enum NowState
    //{
    //    None, tiles, activeTiles, monster, gimmik
    //}


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
        // Object[] sprites = Resources.LoadAll("Sprite/Icon/" + iconTag, typeof(Sprite));
        //Object[] sprites = Resources.LoadAll("Sprite/Icon/" + iconTag, typeof(Sprite));

       


        //var length = sprites.Length;

        //for (int i = 0; i < scrollViewContent.childCount; i++)
        //{
        //    GameObject buttonObj = scrollViewContent.GetChild(i).gameObject;

        //    if (i < length)
        //    {
        //        Sprite sprite = sprites[i] as Sprite;
        //        buttonObj.GetComponent<Image>().sprite = sprite;

        //        // Get Button component and add listener
        //        Button button = buttonObj.GetComponent<Button>();
        //        button.onClick.AddListener(() => OnPopupButtonClicked(button));

        //        buttonObj.SetActive(true);
        //    }
        //    else
        //    {
        //        buttonObj.SetActive(false);
        //    }
        //}

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
            }
            else
            {
                buttonObj.SetActive(false);
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

        iconPopup.SetActive(false);
    }

    public void ExitPopup()
    {
        iconPopup.SetActive(false);
    }
}
