using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Febucci.UI;


enum Response
{
    good,
    bad,
    nuetral,
}

public class DialogueController : MonoBehaviour
{ 
    private CharacterController player;
    public bool InDialogue = false;
    private bool playerTurn = true;
    private PlantData activePlant;
    private PlantingBed activeBed;
    private string plantResponse;
    private Response lastResponceType;
    private bool actionTaken = false;

    [SerializeField] private Animator plantAnim;
    [SerializeField] private GameObject InputField;
    [SerializeField] private GameObject ResponseText;
    [SerializeField] private GameObject PlayerIcon;


    private void Start()
    {
        player = FindObjectOfType<CharacterController>();
        ResponseText.SetActive(false);
        plantAnim.gameObject.SetActive(false);
        InputField.SetActive(false);
        PlayerIcon.SetActive(false);
    }

    public void LaunchDialogue(PlantData _plant, PlantingBed _bed)
    {
        lastResponceType = Response.nuetral;
        activeBed = _bed;
        activePlant = _plant;
        actionTaken = false;
        player.IsTalking(true);
        playerTurn = true;
        InDialogue = true;
        transform.DOMoveY(-24f, 1f);
    }

    public void EndDialogue()
    {
        activeBed = null;
        activePlant = null;
        player.IsTalking(false);
        InDialogue = false;
        transform.DOMoveY(-58f, 1f);
    }

    private void Update()
    {
        if (!InDialogue) return;
        //set correct talking animation for the plant

        if (playerTurn)
        {
            InputField.GetComponent<TMP_InputField>().ActivateInputField();
            ResponseText.SetActive(false);
            plantAnim.gameObject.SetActive(false);
            InputField.SetActive(true);
            PlayerIcon.SetActive(true);

            //on the player's turn we're going to wait for them to hit enter and then send the text to get processed.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ProcessInput(InputField.GetComponent<TMP_InputField>().textComponent.text);
                Debug.Log(plantResponse);
                playerTurn = false;
                //toggle on the plant response

            }
        }
        else
        {

            ResponseText.SetActive(true);
            plantAnim.gameObject.SetActive(true);
            InputField.SetActive(false);
            PlayerIcon.SetActive(false);

            if (!actionTaken)
            {
                ResponseText.GetComponent<TextAnimatorPlayer>().ShowText(plantResponse);
                switch (activePlant.Name)
                {
                    case "Daisy":
                        plantAnim.Play("anim_talkingDaisy");
                        break;
                    case "Rose":
                        plantAnim.Play("talking_rose");
                        break;
                    case "Melon":
                        plantAnim.Play("Watermellon");
                        break;
                    case "Basil":
                        plantAnim.Play("talking_basil");
                        break;
                    case "Tomato":
                        plantAnim.Play("talking_tomato");
                        break;
                }
                ProcessResponse();
                Invoke("EndDialogue", 20f);
                actionTaken = true;
            }

        }
    }
    private void ProcessInput(string _text)
    {
        //lets clean and split up all of our text input
        _text = _text.Replace('!', ' ');
        _text = _text.Replace('.', ' ');
        _text = _text.Replace(',', ' ');
        _text = _text.Replace('?', ' ');
        _text = _text.Replace('\n', ' ');
        string[] words = _text.Split(" ");
        
        foreach (string w in words)
        {
            Debug.Log(w);
            foreach (string s in activePlant.Likes)
            {
                if (w == s)
                {
                    plantResponse = activePlant.GoodResponses[Random.Range(0, activePlant.GoodResponses.Count - 1)];
                    plantResponse = SwapTagWithWord(plantResponse, s);
                    lastResponceType = Response.good;
                    return;
                }
            }

            /*foreach (string s in activePlant.Dislikes)
            {
                if (w == s)
                {
                    plantResponse = activePlant.BadResponses[Random.Range(0, activePlant.BadResponses.Count - 1)];
                    plantResponse = SwapTagWithWord(plantResponse, s);
                    lastResponceType = Response.bad;
                    return;
                }
            }*/
        }

        //if we're here we did not find good words.
        //lets give a hint?
        //We're no longer processing nuetral responses - instead any non good response will be treated as bad. 
        plantResponse = activePlant.BadResponses[Random.Range(0, activePlant.BadResponses.Count - 1)];
        lastResponceType = Response.bad;

    }

    public void ProcessResponse()
    {
        plantResponse = "";
        InputField.GetComponent<TMP_InputField>().SetTextWithoutNotify("");
        switch (lastResponceType)
        {
            case Response.good:
                activeBed.TriggerGrowth = true;
                break;
            case Response.bad:
                activeBed.ShrivelPlant();
                break;
/*            case Response.nuetral:
                playerTurn = true;
                actionTaken = false;
                break;*/
        }
    }

    private string SwapTagWithWord(string _response, string _word)
    {
        string[] split = _response.Split("[x]");
        if (split.Length < 2)
        {
            Debug.LogError("no [x] in statement: " + _response);
            return _response;
        }
        return split[0] + _word + split[1];
    }
}


