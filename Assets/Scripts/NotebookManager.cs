using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NotebookManager : MonoBehaviour
{
    [Header("Drag Ins")]
    [SerializeField] private GameObject openUI;
    [SerializeField] private GameObject notebookObject;
    [SerializeField] private GameObject seed;
    [SerializeField] private GameObject pageTurner;
    [SerializeField] private List<TextMeshProUGUI> textElements;
    [SerializeField] private Animator notebookAnimator;

    [Header("Public fields")]
    public List<PlantData> ListOfPlants;
    public void OpenNotebook()
    {
        notebookAnimator.Play("anim_NotebookOpen");
        openUI.SetActive(false);
    }

    public void CloseNotebook()
    {
        notebookAnimator.Play("anim_NotebookClose");
        openUI.SetActive(true);
    }

    public void FadeInUI()
    {
        foreach(TextMeshProUGUI t in textElements)
        {
            t.DOFade(1, .1f);
        }
    }

    public void FadeOutUI()
    {
        foreach (TextMeshProUGUI t in textElements)
        {
            t.DOFade(0, .1f);
        }
    }

    public void NextPage()
    {
        pageTurner.SetActive(true);
        pageTurner.GetComponent<Animator>().Play("anim_Pageturn");
    }

    public void DispenseSeed(string seedName)
    {
        CloseNotebook();
        seed.SetActive(true);
    }
}
