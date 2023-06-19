using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    private bool isInstanced = false;

    private ObjectMove objectMove;

    private void Awake()
    {
        
    }


    public virtual void StartInstance()
    {
        StartCoroutine(InstanceCoroutine());
    }

    public virtual IEnumerator InstanceCoroutine()
    {
        yield break;
    }

}
