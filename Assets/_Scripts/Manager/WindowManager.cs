using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    //[SerializeField] GameObject activeWindow;
    public GameObject chatWindow;
    public GameObject friendlistWindow;
    public GameObject phoneButton;
    public GameObject responseWindow;
    public Button message;
    public Button calendar;
    public Button closeChat;
    public Button replyButton;
    public Animator anim;
    public DialogueManager dialogueManager;

    [SerializeField] private float closePhoneDelayMin = 1f;
    [SerializeField] private float closePhoneDelayMax = 1.5f;

    private bool responeActive = false;
    private bool isOnChat = false;

    public static Action <WindowManager> OnChatWindowEnabled;
    public static Action<WindowManager> OnPhoneRing;

    private void Start()
    {
        ResponseManager.OnEndingPhase += PlayerReplied;
        UI_Caster.OnContentWarningClose += SetActivePhone;
        phoneButton.SetActive(false);
        //Initiate starting active window here
        //Set activeWindow to MainMenu?
    }

    private void OnEnable()
    {
        
    }

    private void OnDestroy()
    {
        ResponseManager.OnEndingPhase -= PlayerReplied;
        UI_Caster.OnContentWarningClose -= SetActivePhone;
    }

    private void SetActivePhone(UI_Caster ui)
    {
        StartCoroutine(PhoneRing());
    }

    private void PlayerReplied(ResponseManager r) //listener to ResponseManager OnPlayerReply()
    {
        StartCoroutine(ClosingPhone());
    }

    private IEnumerator ClosingPhone() //Delay time before closing phone
    {
        float closePhoneDelay;
        closePhoneDelay = UnityEngine.Random.Range(closePhoneDelayMin, closePhoneDelayMax);

        yield return new WaitForSeconds(closePhoneDelay);
        StartCoroutine(PhoneRing());
        phoneButton.SetActive(false);
        ClosePhone();
    }

    private IEnumerator PhoneRing()
    {
        float phoneRingDelay = UnityEngine.Random.Range(2f, 4f);

        yield return new WaitForSeconds(phoneRingDelay);

        OnPhoneRing?.Invoke(this);
        phoneButton.SetActive(true);
    }

    public void OpenPhone()
    {
        phoneButton.SetActive(false);
        anim.SetBool("isOpen", true);
        if(isOnChat)
        {
            dialogueManager.InnitiateChat();
        }


    }

    public void ClosePhone()
    {
        //CloseChat();
        StopCoroutine(ClosingPhone());
        phoneButton.SetActive(true);
        anim.SetBool("isOpen", false);

    }

    public void ResponseBackButton()
    {
        responseWindow.SetActive(false);
        replyButton.interactable = true;

    }

    public void OpenChat(string charName)
    {
        //charName = (string)gameObject.name;
        //transition animation of the current active window
        //change active window to chatWindow        
        //add transition animation here

        //temporary
        dialogueManager.charName = charName;
        chatWindow.SetActive(true);
        isOnChat = true;

        OnChatWindowEnabled?.Invoke(this);
        //Debug.Log("Character clicked = " + dialogueManager.charName);
    }

    public void CloseChat()
    {
        //This happen when back button on chat screen pressed
        //transition animation of chat window
        chatWindow.SetActive(false);
        isOnChat = false;
    }

    public void OpenFriendlist()
    {
        friendlistWindow.SetActive(true);
    }

    public void CloseFriendlist()
    {
        friendlistWindow.SetActive(false);
    }
}
