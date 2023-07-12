using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupGetter : MonoBehaviour
{
    [SerializeField]
    protected float clampX;

    [SerializeField]
    protected float clampY;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected Color highlightColor;

    protected Vector3 posMover = Vector3.zero;

    public virtual void OnPopup() => spriteRenderer.color = highlightColor;
    public virtual void OffPopup() => spriteRenderer.color = Color.white;

    protected virtual void SetPosition(Transform popupTransform)
    {
        var newPos = Camera.main.WorldToScreenPoint(transform.position + posMover);

        newPos.x = Mathf.Clamp(newPos.x, clampX, Screen.width - clampX);
        newPos.y = Mathf.Clamp(newPos.y, clampY, Screen.height - clampY);

        popupTransform.position = newPos;
    }
}
