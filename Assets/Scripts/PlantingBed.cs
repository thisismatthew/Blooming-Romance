using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlantingBed : MonoBehaviour
{
    private bool playerInZoneWithSeed = false;
    private bool hasPlant = false;
    private CharacterController player;
    [SerializeField] private GameObject buttonPrompt;
    private PlantData currentPlant;
    private int plantGrowthIndex;
    public bool TriggerGrowth = false;
    private Animator childAnim;
    private bool playerInZone = false;
    [SerializeField]private float plantingTimer = 10f;
    [SerializeField] private GameObject timeUI;
    private DialogueController dc;

    private void Start()
    {
        dc = FindObjectOfType<DialogueController>();
        player = FindObjectOfType<CharacterController>();
        childAnim = GetComponentInChildren<Animator>();
    }
    //if the player has a seed and enters the planting zone they can plant a seed.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            playerInZone = true;
            //if they got a seed 
            if (player.HoldingSeed)
            {
                playerInZoneWithSeed = true;
                buttonPrompt.transform.DOScale(1f, .3f);
            }

            if (hasPlant && plantGrowthIndex == 0)
            {
                buttonPrompt.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "q";
                buttonPrompt.transform.DOScale(1f, .3f);
            }

            if (hasPlant && plantGrowthIndex == 2)
            {
                buttonPrompt.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "e";
                buttonPrompt.transform.DOScale(1f, .3f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        playerInZone = false;
        playerInZoneWithSeed = false;
        buttonPrompt.transform.DOScale(0.001f, .3f);
        buttonPrompt.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "e";
    }

    private void Update()
    {
        if (playerInZoneWithSeed)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                FindAnyObjectByType<AudioManager>().Play("planting");
                hasPlant = true;
                currentPlant = player.CurrentSeedData;
                player.HoldingSeed = false;
                playerInZoneWithSeed = false;
                buttonPrompt.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "q";
                childAnim.Play("seed");
            }
        }

        //stage 0 needs water
        if (hasPlant && playerInZone && plantGrowthIndex == 0)
        {
            if (player.CurrentState != CharacterState.pouring) return;
            Debug.Log("pouring!");
            TriggerGrowth = true;
            buttonPrompt.transform.DOScale(.001f, .3f);
            buttonPrompt.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "e";
        }


        //stage 1 needs time
        if (hasPlant && plantGrowthIndex ==1)
        {
            timeUI.transform.DOScale(1.5f, .3f);
            TextMeshProUGUI timerIndicator = timeUI.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            if (plantingTimer > 8) timerIndicator.text = ".";
            if (plantingTimer > 6 && plantingTimer < 8) timerIndicator.text = "..";
            if (plantingTimer > 4 && plantingTimer < 6) timerIndicator.text = "...";
            if (plantingTimer > 2 && plantingTimer < 4) timerIndicator.text = "!";


            if (plantingTimer > 0)
            {
                plantingTimer -= Time.deltaTime;
            }
            else
            {
                plantingTimer = 10;
                TriggerGrowth = true;
            }
        }

        //stage 2 needs conversation!
        if (hasPlant && plantGrowthIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.E)&& !dc.InDialogue && playerInZone)
            {
                buttonPrompt.transform.DOScale(.001f, .3f);
                //open the dialogue system
                dc.LaunchDialogue(currentPlant, this);
            }
        }

        if (hasPlant && plantGrowthIndex == 3)
        {
            //we are done we can rapture this flower and give the player some money
            //Todo give money and make some kind of animation that pops the flower or something.
            FindObjectOfType<NotebookManager>().Money += currentPlant.Yield;
            currentPlant = null;
            hasPlant = false;
            plantGrowthIndex = 0;
            childAnim.Play("coin_pop");
        }

        if (hasPlant && TriggerGrowth)
        {
            TriggerGrowth = false;
            if (plantGrowthIndex == 0) { childAnim.Play("anim_grow_daisy"); }

            if (plantGrowthIndex == 1)
            {
                timeUI.transform.DOScale(0f, .3f);
                switch (currentPlant.Name)
                {
                    case "Rose":
                        childAnim.Play("rose_grow_anim");
                        break;
                    case "Daisy":
                        childAnim.Play("anim_daisy_full");
                        break;
                    case "Melon":
                        childAnim.Play("anim_melon_full");
                        break;
                    case "Basil":
                        childAnim.Play("anim_grow_basil");
                        break;
                    case "Tomato":
                        childAnim.Play("anim_grow_tomato");
                        break;
                }

            }
            plantGrowthIndex++;
        }

    }

    public void ShrivelPlant()
    {
        Debug.Log("shrivelled");
        plantGrowthIndex = 0;
        childAnim.Play("anim_seedling_idle");

    }

}
