using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class NotebookManager : MonoBehaviour
{
    [Header("Drag Ins")]
    [SerializeField] private GameObject openUI;
    [SerializeField] private GameObject notebookObject;
    [SerializeField] private GameObject seed;
    [SerializeField] private GameObject pageTurner;
    [SerializeField] private Image descriptiveImage;
    [SerializeField] private List<Sprite> plantSprites;
    [SerializeField] private List<TextMeshProUGUI> textElements;
    [SerializeField] private Animator notebookAnimator;
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
        FindAnyObjectByType<AudioManager>().Play("notebook open");
        notebookAnimator.Play("anim_NotebookOpen");
        openUI.GetComponent<Image>().DOFade(0,.1f);
        pageIndex = 0;
        UpdatePageData();
        IsOpen = true;
    }

    public void CloseNotebook()
    {
        IsOpen = false;
        FindAnyObjectByType<AudioManager>().Play("notebook close");
        notebookAnimator.Play("anim_NotebookClose");
        Invoke("FadeInUIButton", .4f);

    }

    public void FadeInUI()
    {
        foreach(TextMeshProUGUI t in textElements)
        {
            t.DOFade(1, .1f);
        }
        descriptiveImage.DOFade(1, .1f);
    }

    public void FadeOutUI()
    {
        foreach (TextMeshProUGUI t in textElements)
        {
            t.DOFade(0, .1f);
        }
        descriptiveImage.DOFade(0, .1f);
    } 

    public void NextPage()
    {
        
        FindAnyObjectByType<AudioManager>().Play("page turn");
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
        descriptiveImage.sprite = plantSprites[pageIndex];
        textElements[0].text = ListOfPlants[pageIndex].Name;
        textElements[1].text = ListOfPlants[pageIndex].Description;
    }

    public void FadeInUIButton()
    {
        openUI.GetComponent<Image>().DOFade(1, .2f);
    }
}
