using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class ResponseManager : MonoBehaviour
{
    public GameObject chatPanel, textObject;
    public GameObject dialogueManager;
    public GameObject questionWindow;

    DialogueManager dialogue;
    ResponseHolder responseHolder;
    GlobalLightControl lightControl;

    private int answerCode;
    public Text question;
    public Text respondOne;
    public Text respondTwo;
    public Text respondThree;
    public GameObject responseBoxOne, responseBoxTwo, responseBoxThree;

    //temper limit numbers: Q1 = 15, Q2 = 30, Q3 = 45, Q4 = N/A
    //Q1T19 Q2T21 Q3T21

    public Answer currentAnswer; //current assigned answer

    [SerializeField] private List<string> nextQuestState;
    public int currentTemper = 0;
    public int temperLimit = 0;
    public int currentLevel = 0;

    private float endingDelay = 3f;

    public static Action<ResponseManager> OnEndingPhase;  //announce when player click on answer
    public static Action<ResponseManager> OnClosingPhone;
    public static Action<ResponseManager> OnEnding;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = dialogueManager.GetComponent<DialogueManager>();
        responseHolder = GetComponent<ResponseHolder>();
        lightControl = GameObject.Find("LightingManager").GetComponent<GlobalLightControl>();

        DialogueManager.OnToAnswer += SetAnswer;              
    }

    private void OnDestroy()
    {
        DialogueManager.OnToAnswer -= SetAnswer;
    }

    void EndingPopup()
    {
        Debug.Log("ending popup");
        OnEnding?.Invoke(this);
    }

    void ResponseCheck() //Check wheter closing phone or continue chat
    {
        responseBoxOne.SetActive(false);
        responseBoxTwo.SetActive(false);
        responseBoxThree.SetActive(false);

        if (currentAnswer.isEnding == true) 
        {
            Debug.Log("THIS IS THE END!");
            Invoke("EndingPopup", endingDelay);
        }

        //call close phone function if currentAnswer.isClosing == true
        if (currentAnswer.isClosing == false && currentAnswer.isEnding == false)
        {
            currentTemper = (int)DialogueManager.scc.getGameStateValue("temper");
            lightControl.TemperChange(currentTemper);
            //Debug.Log("Temper currently " + temperLimit);

            if (currentLevel == 0 && currentTemper >= 15)
            {
                //go to the last questState of the phase
                DialogueManager.scc.questState = "Q1T19";
            }
            else if (currentLevel == 1 && currentTemper >= 30)
            {
                //go to the last questState of the phase
                DialogueManager.scc.questState = "Q2T21";
            }
            else if (currentLevel == 2 && currentTemper >= 45)
            {
                //go to the last questState of the phase
                DialogueManager.scc.questState = "Q3T21";
            }

            //Debug.Log("Current Level is " + currentLevel);
            //Debug.Log("CurrentTemper is " + currentTemper);
            //Debug.Log("Current questState is " + DialogueManager.scc.questState);
            dialogue.InnitiateChat();
        }
            

        if (currentAnswer.isClosing == true)
        {
            //Debug.Log("isClosing is TRUE, Phase ended");
            OnClosingPhone?.Invoke(this);            
            currentLevel += 1;
            
            DialogueManager.scc.questState = nextQuestState[currentLevel];

            //Debug.Log("Current Level is " + currentLevel);
            //Debug.Log("CurrentTemper is " + currentTemper);

            if (currentLevel == 1)
            {
                currentTemper = 15;
                lightControl.TemperChange(currentTemper);                
            }                
            else if (currentLevel == 2)
            {
                currentTemper = 30;
                lightControl.TemperChange(currentTemper);
            }
            else if (currentLevel == 3)
            {
                currentTemper = 45;
                lightControl.TemperChange(currentTemper);
            }

            EndingPhase();

            //create a coroutine that run the event announcer once it return
        }
    }

    void EndingPhase()
    {
        OnEndingPhase?.Invoke(this);
    }

    private void SetAnswer(DialogueManager d)
    {
        answerCode = dialogue.toQuestion;
        currentAnswer = responseHolder.responses[answerCode];
        //Debug.Log("Assign Answer to responses index " + answerCode);

        //Set RESPONSE button appear to run this function!!!!
        SetCurrentResponse();
    }

    public void SetCurrentResponse() //for now it triggers from chat button
    {
        if(currentAnswer.noOption == true)
        {
            DialogueManager.scc.questState = currentAnswer.firstDestination;
            dialogue.InnitiateChat();
        }
        else
        {
            questionWindow.SetActive(true); //ANIMATE THIS BEFORE SetActive

            question.text = currentAnswer.question; //show the question to the UI
            
            if(currentAnswer.firstResponse != "")
            {
                responseBoxOne.SetActive(true);
                respondOne.text = currentAnswer.firstResponse;
            }

            if (currentAnswer.secondResponse != "")
            {
                responseBoxTwo.SetActive(true);
                respondTwo.text = currentAnswer.secondResponse;
            }
            
            if(currentAnswer.thirdResponse != "")
            {
                responseBoxThree.SetActive(true);
                respondThree.text = currentAnswer.thirdResponse;
            }
            

            DialogueManager.scc.setGameStateValue("toQuestion", "equals", 0); //Set toQuestion back to 0 here
            DialogueManager.scc.setGameStateValue("convoLine", "equals", 0);
        }        
    }

    //ASSIGN RESPONSE FROM THE BUTTON
    public void FirstResponse()
    {
        DialogueManager.scc.questState = currentAnswer.firstDestination;
        //Debug.Log("First response selected, questState now " + DialogueManager.scc.questState);

        questionWindow.SetActive(false); //ANIMATE THIS BEFORE SetActive

        //Start the coroutine to answer the player response
        //Or, depend on how we want the NPC to response
        string myText;
        myText = currentAnswer.firstResponse;

        if(dialogue.messageList.Count >= dialogue.maxMessages) //Destroy the oldest msg history
        {
            Destroy(dialogue.messageList[0].textObject.gameObject);
            dialogue.messageList.Remove(dialogue.messageList[0]);
        }

        Message newResponse = new Message();
        newResponse.text = myText;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newResponse.textObject = newText.GetComponentInChildren<Text>();
        newResponse.textObject.text = newResponse.text;

        dialogue.messageList.Add(newResponse);

        //call close phone function
        ResponseCheck();
    }

    public void SecondResponse()
    {
        DialogueManager.scc.questState = currentAnswer.secondDestination;
        //Debug.Log("First response selected, questState now " + DialogueManager.scc.questState);

        questionWindow.SetActive(false); //ANIMATE THIS BEFORE SetActive

        string myText;
        myText = currentAnswer.secondResponse;

        if (dialogue.messageList.Count >= dialogue.maxMessages) //Destroy the oldest msg history
        {
            Destroy(dialogue.messageList[0].textObject.gameObject);
            dialogue.messageList.Remove(dialogue.messageList[0]);
        }

        Message newResponse = new Message();
        newResponse.text = myText;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newResponse.textObject = newText.GetComponentInChildren<Text>();
        newResponse.textObject.text = newResponse.text;

        dialogue.messageList.Add(newResponse);

        //call close phone function
        ResponseCheck();
    }

    public void ThirdResponse()
    {
        DialogueManager.scc.questState = currentAnswer.thirdDestination;
        //Debug.Log("First response selected, questState now " + DialogueManager.scc.questState);

        questionWindow.SetActive(false); //ANIMATE THIS BEFORE SetActive

        //Start the coroutine to answer the player response
        //Or, depend on how we want the NPC to response
        string myText;
        myText = currentAnswer.thirdResponse;

        if (dialogue.messageList.Count >= dialogue.maxMessages) //Destroy the oldest msg history
        {
            Destroy(dialogue.messageList[0].textObject.gameObject);
            dialogue.messageList.Remove(dialogue.messageList[0]);
        }

        Message newResponse = new Message();
        newResponse.text = myText;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newResponse.textObject = newText.GetComponentInChildren<Text>();
        newResponse.textObject.text = newResponse.text;

        dialogue.messageList.Add(newResponse);

        Debug.Log(myText);

        //call close phone function
        ResponseCheck();
    }

}