using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer rightSprite;
    public static Castle Instance { get; private set; }

    private bool isClear = false;

    private void Awake()
    {
        Instance = this;
    }

    public void StageClear()
    {
        isClear = true;
        rightSprite.sortingOrder = (int)EnumSpriteLayerOrder.Player + 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (!isClear)
            return;

        SoundManager.Instance.PlaySFX("Clear");
        collision.gameObject.SetActive(false);

        if(InGameManager.Instance != null)
            InGameManager.Instance?.PointCalculate();

        if (SceneLoader.Instance.State == SceneState.Tool)
            Invoke(nameof(ToolSceneClear), 6f);
    }   

    private void ToolSceneClear()
    {
        var toolMgr = ToolManager.Instance;

        if (toolMgr.ToolMode == ToolManager.ToolModeType.Save)
        {
            toolMgr.CJsonTest.GameComplete();
        }
        toolMgr.GoTool();
    }
}
