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
    private bool triggerGrowth = false;
    private Animator childAnim;
    private bool playerInZone = false;
    private float plantingTimer = 10f;

    private void Start()
    {
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
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
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
            triggerGrowth = true;
            buttonPrompt.transform.DOScale(.001f, .3f);
            buttonPrompt.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "e";
        }


        //stage 1 needs time
        if (hasPlant && plantGrowthIndex ==1)
        {
            if (plantingTimer > 0)
            {
                plantingTimer -= Time.deltaTime;
            }
            else
            {
                plantingTimer = 10;
                triggerGrowth = true;
            }
        }

        if (hasPlant && triggerGrowth)
        {
            triggerGrowth = false;
            if (plantGrowthIndex == 0) { childAnim.Play("anim_grow_daisy"); }

            if (plantGrowthIndex == 1)
            {
                switch (currentPlant.Name)
                {
                    case "Rose":
                        //childAnim.Play("anim_rose_full");
                        break;
                    case "Daisy":
                        childAnim.Play("anim_daisy_full");
                        break;
                    case "Melon":
                        //childAnim.Play("anim_melon_full");
                        break;
                }

            }
            plantGrowthIndex++;
        }

    }
}
