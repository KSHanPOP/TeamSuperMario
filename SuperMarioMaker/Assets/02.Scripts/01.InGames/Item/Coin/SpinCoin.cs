

using System.Collections;
using UnityEngine;

public class SpinCoin : ItemBase
{
    [SerializeField]
    private float destHeightAdjust;

    public override IEnumerator InstanceCoroutine(Vector2 destPos)
    {
        destPos.y += destHeightAdjust;

        float timer = 0f;
        float inverseInstanceTime = 1 / instanceSequnceTime;
        Vector3 startPos = transform.position;

        while (timer < instanceSequnceTime)
        {
            timer += Time.deltaTime;

            float t = timer * inverseInstanceTime * 2;

            if (t > 1)
            {                             
                t = - t + 2;
            }

            transform.position = Vector3.Lerp(startPos, destPos, t);

            yield return null;
        }

        ScoreManager.Instance.GetCoin(1);   

        Destroy(gameObject);
        yield break;
    }
}
