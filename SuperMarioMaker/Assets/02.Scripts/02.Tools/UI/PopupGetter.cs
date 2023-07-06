using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
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
    public Color highlightColor;

    protected Vector3 posMover = Vector3.zero;
    public virtual void OnPopup() { }
    public virtual void OffPopup() { }

    protected virtual void SetPosition(Transform popupTransform)
    {
        var newPos = Camera.main.WorldToScreenPoint(transform.position + posMover);

        newPos.x = Mathf.Clamp(newPos.x, clampX, Screen.width - clampX);
        newPos.y = Mathf.Clamp(newPos.y, clampY, Screen.height - clampY);

        popupTransform.position = newPos;
    }
}
