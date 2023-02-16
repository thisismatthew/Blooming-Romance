using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreeAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveZ(transform.position.z+2, 20f).SetLoops(-1,LoopType.Yoyo);
        transform.DOMoveX(transform.position.x + .5f, 10f).SetLoops(-1, LoopType.Yoyo);

    }


}
