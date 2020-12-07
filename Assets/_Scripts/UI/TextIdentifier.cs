using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextIdentifier : MonoBehaviour
{
    public string testString = "I hope this help you";
    public string otherCase = "I hope";

    public void SearchForText()
    {
        if (testString.Contains("you"))
        {
            Debug.Log("The variable testString contains the word you");
        }
        if (testString.Contains(otherCase))
        {
            Debug.Log("The variable testString contains the value of the variable otherCase");
      }
    }
}
