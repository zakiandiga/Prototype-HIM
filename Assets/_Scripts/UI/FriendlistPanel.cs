using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendlistPanel : MonoBehaviour
{    
    [Serializable] public struct Friend 
    {
        public string name;
        public string status;
        //public Sprite profPic;
    }

    [SerializeField] Friend[] friends;

    
    // Start is called before the first frame update
    void Start()
    {
        //these will be called when friendlist window is opened (instead on Start())
        GameObject friendlistButton = transform.GetChild(0).gameObject;
        GameObject g;

        int N = friends.Length; //length of the friendlist
        for(int i = 0; i < N ; N++) //10 = max number of friend on the visible list, populate length based on i
        {
            g = Instantiate(friendlistButton, transform);
            g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = friends[i].name;
            g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = friends[i].status;
            //g.transform.GetChild(2).GetComponent<Image>().sprite = friends[i].profPic;

        }
        Destroy(friendlistButton); //destroy the template
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
