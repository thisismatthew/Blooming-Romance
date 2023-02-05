using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Title : MonoBehaviour
{
    private void Start()
    {
        Invoke("DelayedDisapear", 3f);
        Invoke("DelayedFade", 4.5f);
    }

    public void DelayedDisapear()
    {
        transform.DOScale(0, 2f).SetEase(Ease.InElastic);
    }
    public void DelayedFade()
    {
        GetComponent<Image>().DOFade(0f, .5f);
    }
}
