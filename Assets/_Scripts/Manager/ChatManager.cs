using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    /*
    public int maxMessages = 25;

    public GameObject chatPanel, textObject;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }    

    public void SendMessageToChat(string text)  //Call this function to send message data to chatbox
    {
        text = DialogueManager.scc.getSCCLine("Briar");


        if (messageList.Count >= maxMessages)  //Removing the oldest message on memory
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }    
            

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }
    */
}

/*
[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
*/
