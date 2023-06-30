using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IconButtonSetting : MonoBehaviour
{
    [SerializeField]
    private Button chooseButton;
    [SerializeField]
    private Button selectedButton;
    [SerializeField]
    private string defaltIconFilePath;

    [SerializeField]
    private Outline outLine;

    public void SetOutline(bool onOff) => outLine.enabled = onOff;

    [SerializeField] private string tag;
    [SerializeField] private string nowName;
    public string NowName
    {
        get { return nowName; }
        set { nowName = value; }
    }
    public string Tag { get { return tag; } }
    private void Init()
    {
        var prefap = ResourceManager.instance.GetPrefabWithTag(tag).GetComponent<PrefapInfo>();
        name = prefap.name;
        Sprite tempSprite = Resources.Load<Sprite>(prefap.IconSpritePath);
        chooseButton.GetComponent<Image>().sprite = tempSprite;
        selectedButton.GetComponent<Image>().sprite = tempSprite;

        chooseButton.onClick.AddListener(OnChooseButtonClick);
        selectedButton.onClick.AddListener(OnSelectedButtonClick);
        outLine.enabled = false;
    }

    private void Start()
    {
        Init();
    }

    private void SetTag() //파일 "Sprite/Icon/StaticTileIcon/TileIcon_00";// 이것만 저장하는 함수
    {
        string[] parts = defaltIconFilePath.Split('/');
        tag = parts[2];
        //string targetPart = parts[2];
    }
    // Insert the operations that should be done when the choose button is clicked
    private void OnChooseButtonClick()
    {
        Logger.Debug("Choose button clicked!" + tag);
        ToolManager.Instance.iconManager.NowTag = tag;
        ToolManager.Instance.iconManager.SetIconPopup(tag);
    }

    // Insert the operations that should be done when the selected button is clicked
    private void OnSelectedButtonClick()
    {
        Logger.Debug("Selected button clicked!" + tag);

        ToolManager.Instance.iconManager.NowTag = tag;
        //ToolManager.Instance.iconManager.NowName = NowName;

    }

    public void ChangeImage(Sprite sprite, string name)
    {
        selectedButton.GetComponent<Image>().sprite = sprite;
        //NowName = name;

    }
}
