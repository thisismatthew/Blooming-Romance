using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using FMODUnity;

public class NotebookManager : MonoBehaviour
{
    [Header("Drag Ins")]
    [SerializeField] private GameObject openUI;
    [SerializeField] private GameObject notebookObject;
    [SerializeField] private GameObject seed;
    [SerializeField] private GameObject pageTurner;
    [SerializeField] private GameObject newspaper;
    [SerializeField] private Image descriptiveImage;
    [SerializeField] private List<Sprite> plantSprites;
    [SerializeField] private List<TextMeshProUGUI> textElements;
    [SerializeField] private Animator notebookAnimator;
    [SerializeField] private Animator outroAnimator;

    private int pageIndex = 0;

    [Header("Money Stuff")]
    [SerializeField] private TextMeshProUGUI moneyDisplay;
    public int Money = 1;
    [SerializeField] private List<int> seedCosts;

    [Header("Public fields")]
    public List<PlantData> ListOfPlants;
    public bool IsOpen;

    public void OpenNotebook()
    {
        //FindAnyObjectByType<AudioManager>().Play("notebook open");
        RuntimeManager.PlayOneShot("event:/FX/Notebook Open");
        notebookAnimator.Play("anim_NotebookOpen");
        openUI.GetComponent<Image>().DOFade(0,.1f);
        pageIndex = 0;
        UpdatePageData();
        IsOpen = true;
    }

    public void CloseNotebook()
    {
        IsOpen = false;

        if (pageIndex == 5)
        {
            newspaper.SetActive(false);
        }

        //FindAnyObjectByType<AudioManager>().Play("notebook close");
        RuntimeManager.PlayOneShot("event:/FX/Notebook Close");
        notebookAnimator.Play("anim_NotebookClose");
        Invoke("FadeInUIButton", .4f);
    }

    public void FadeInUI()
    { 
        if (pageIndex == 5) newspaper.GetComponent<Image>().DOFade(1, .1f);
        foreach (TextMeshProUGUI t in textElements)
        {
            t.DOFade(1, .1f);
        }
        descriptiveImage.DOFade(1, .1f);
    }

    public void FadeOutUI()
    {
        if (pageIndex == 5) newspaper.GetComponent<Image>().DOFade(0, .1f);
        foreach (TextMeshProUGUI t in textElements)
        {
            t.DOFade(0, .1f);
        }
        descriptiveImage.DOFade(0, .1f);
    } 

    public void NextPage()
    {
        //FindAnyObjectByType<AudioManager>().Play("page turn");
        RuntimeManager.PlayOneShot("event:/FX/Page Turn");
        pageTurner.SetActive(true);
        pageTurner.GetComponent<Animator>().Play("anim_Pageturn");
        FadeOutUI();
        Invoke("FadeInUI", .2f);
        if(pageIndex < ListOfPlants.Count-1)
        {
            pageIndex++;
        }
        else { pageIndex = 0; }
        UpdatePageData();
    }

    public void DispenseSeed(string seedName)
    {
        //check if the player can afford it
        if (Money >= seedCosts[pageIndex])
        {
            if(ListOfPlants[pageIndex].Name=="Dinner With Hubby")
            {

                TriggerEndCutscene();
                return;
            }
            CloseNotebook();
            seed.SetActive(true);
            CharacterController player = FindObjectOfType<CharacterController>();
            player.HoldingSeed = true;
            player.CurrentSeedData = ListOfPlants[pageIndex];
            player.CurrentSeedData.Yield = seedCosts[pageIndex] * 2;
            Money -= seedCosts[pageIndex];
        }
        else
        {

        }
    }

    private void Update()
    {
        moneyDisplay.text = Money.ToString();
        if (FindObjectOfType<CharacterController>().HoldingSeed == false)
        {
            seed.SetActive(false);
        }
    }

    private void UpdatePageData()
    {
        textElements[5].text = seedCosts[pageIndex].ToString();
        if (pageIndex < 5)
        {
            textElements[2].text = "Buy Seed";
            newspaper.SetActive(false);
            textElements[0].gameObject.SetActive(true);
            textElements[1].gameObject.SetActive(true);
            textElements[4].gameObject.SetActive(true);
            textElements[5].gameObject.SetActive(true);
            textElements[6].gameObject.SetActive(true);
            textElements[7].gameObject.SetActive(true);
        }
        else
        {
            newspaper.SetActive(true);
            textElements[2].text = "Buy Dinner";
            textElements[0].gameObject.SetActive(false);
            textElements[1].gameObject.SetActive(false);
            textElements[4].gameObject.SetActive(false);
            textElements[5].gameObject.SetActive(false);
            textElements[6].gameObject.SetActive(false);
            textElements[7].gameObject.SetActive(false);

        }

        descriptiveImage.sprite = plantSprites[pageIndex];
        textElements[0].text = ListOfPlants[pageIndex].Name;
        textElements[1].text = ListOfPlants[pageIndex].Description;
        textElements[7].text = ListOfPlants[pageIndex].Wants;
    }

    public void FadeInUIButton()
    {
        openUI.GetComponent<Image>().DOFade(1, .2f);
    }

    private void TriggerEndCutscene()
    {
        outroAnimator.gameObject.SetActive(true);
        outroAnimator.Play("newoutro");
        Invoke("DelayedSceneLoad", 2f);
    }

    public void DelayedSceneLoad()
    {
        SceneManager.LoadScene("DateNight");
    }
}
