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

    protected float posMoverMult = 1f;

    public virtual void OnPopup() => spriteRenderer.color = highlightColor;
    public virtual void OffPopup() => spriteRenderer.color = Color.white;

    protected virtual void Start()
    {
        Vector3 cameraPosition = Camera.main.transform.position;

        float horizontalValue = (transform.position.x - cameraPosition.x) < 0 ? 1f : -1f;
        float verticalValue = (transform.position.y - cameraPosition.y) < 0 ? 1f : -1f;

        posMover = new Vector3(horizontalValue * posMoverMult, verticalValue, 0);
    }

    protected virtual void SetPosition(Transform popupTransform)
    {
        var newPos = Camera.main.WorldToScreenPoint(transform.position + posMover);

        newPos.x = Mathf.Clamp(newPos.x, clampX, Screen.width - clampX);
        newPos.y = Mathf.Clamp(newPos.y, clampY, Screen.height - clampY);

        popupTransform.position = newPos;
    }
}
