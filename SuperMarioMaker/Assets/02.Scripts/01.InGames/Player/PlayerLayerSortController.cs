using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayerSortController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    public void SetLayerSort(EnumSpriteLayerOrder layerOrder)
    {
        spriteRenderer.sortingOrder = (int)layerOrder;
    }
}
