using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
	public int maxMessages = 25;

	public GameObject chatPanel, myTextObject, yourTextObject, errorChatBox;
	GameObject textObject;
	public Button replyButton;

	public string charName = "Player"; //assign the variable based on chosen friendlist
	public string speaker;
	
	public int toQuestion; //assign the variable based on current dialogue line, send to responseManager


	public List<Message> messageList = new List<Message>();



	public static SimpleConditionalConversation scc;

	// NOTE: When you do not use the google sheet option, it is expecting the file
	// to be named "data.csv" and for it to be in the Resources folder in Assets.
	public bool useGoogleSheet = false;
	public string googleSheetDocID = "";

	//ACTION ANNOUNCER
	public static event Action<DialogueManager> OnToAnswer;

	// Start is called before the first frame update
	void Start()
	{
		WindowManager.OnChatWindowEnabled += InnitiateTimer; //Subscribe to WindowManager event

		if (useGoogleSheet) {
			// This will start the asyncronous calls to Google Sheets, and eventually
			// it will give a value to scc, and also call LoadInitialHistory().
			GoogleSheetSimpleConditionalConversation gs_ssc = gameObject.AddComponent<GoogleSheetSimpleConditionalConversation>();
			gs_ssc.googleSheetDocID = googleSheetDocID;
		} else {
			scc = new SimpleConditionalConversation("data");
			LoadInitialSCCState();
		}

	}
	
	public static void LoadInitialSCCState()
	{
		// Example of setting the initial state:
		//scc.setGameStateValue("convoLine", "equals", 0);
		scc.setGameStateValue("temper", "equals", 0);

	}

	private void InnitiateTimer(WindowManager w)
    {
		StartCoroutine(StartChatTimer());
    }

	private IEnumerator StartChatTimer()
    {
		float innitiateDelay;
		innitiateDelay = UnityEngine.Random.Range(0.3f, 1.2f);

		yield return new WaitForSeconds(innitiateDelay);

		InnitiateChat();
    }
	
	public void InnitiateChat() //called from opening friend chat (friendlist window)
    {
		//set chat target
		//open chat window
		//load chat window with retrieved data 
		StartCoroutine(NPCTalks());
		Debug.Log("Chat innitiated!");
		Debug.Log("current questState is " + scc.questState);
	}

	IEnumerator NPCTalks()  //A coroutine that simulate the npc's typing
    {
		float typingDelay;
		typingDelay = UnityEngine.Random.Range(0.8f, 3f);

		yield return new WaitForSeconds(typingDelay);
		SendMessageToChat();	
    }

	private void SendMessageToChat() //A function for displaying the NPC messages
	{
		string[] lineData = DialogueManager.scc.getSCCLine(charName);
		string speaker = lineData[0];
		string text = lineData[1];

		Debug.Log(speaker + ": " + text);
		Debug.Log(DialogueManager.scc.questState);

		if (messageList.Count >= maxMessages)  //Removing the oldest message on memory
		{
			Destroy(messageList[0].textObject.gameObject);
			messageList.Remove(messageList[0]);
		}

		Message newMessage = new Message();
		newMessage.text = text;
		
		if(speaker == "Him") //remove the null later
        {
			textObject = yourTextObject;
        }
		else if(speaker == "Player")
        {
			textObject = myTextObject;
		}
		else if (speaker == "NONE")
        {
			textObject = errorChatBox;
        }

		GameObject newText = Instantiate(textObject, chatPanel.transform);

		newMessage.textObject = newText.GetComponentInChildren<Text>();
		newMessage.textObject.text = newMessage.text;

		messageList.Add(newMessage);
		
		CheckQuestion();	

	}

	private void CheckQuestion()    
	{
		if (DialogueManager.scc.getGameStateValue("toQuestion") == null || (int)DialogueManager.scc.getGameStateValue("toQuestion") <= 0)
        {
			StartCoroutine(NPCTalks()); //Restart the coroutine if the line doesn't require player's response
		}

		if (DialogueManager.scc.getGameStateValue("toQuestion") != null)
        {
			StopCoroutine(NPCTalks());			
			toQuestion = (int)DialogueManager.scc.getGameStateValue("toQuestion");			

			if (toQuestion > 0)
			{
				replyButton.interactable = true;
			}
		}	
		

	}

	public void AnswerChat()
    {
		OnToAnswer?.Invoke(this);
		replyButton.interactable = false;
	}

	// Update is called once per frame
	void Update()
	{
		
		/*
		 * Mike's notes
		// An example of getting a line of dialogue.
	    if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log(DialogueManager.scc.getSCCLine("Emma"));
		}
		
		// An example of modifying the state outside of the DialogueManager (e.g. you could put this in some
		// OnTriggerEnter or something)
	    if (Input.GetKeyDown(KeyCode.G)) {
			scc.setGameStateValue("playerWearing", "equals", "Green shirt");
		}
		*/
	}
}

[System.Serializable]
public class Message
{
	public string text;
	public Text textObject;
}
