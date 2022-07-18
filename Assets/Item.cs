using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemObject item;

    private void Start()
    {
        transform.DOLocalMoveY(0.1f, 0.5f,false).SetLink(this.gameObject)
            .SetLoops(-1, LoopType.Yoyo).Play();
        
    }
}
