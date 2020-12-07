using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Answer", menuName = "NewAnswer")]
public class Answer : ScriptableObject
{
    public string questionCode;
    public bool isClosing = false;
    public bool noOption = false;
    public bool isEnding = false;
    [TextArea(2, 5)] public string question;

    [TextArea(2, 5)] public string firstResponse;
    public string firstDestination;
    [TextArea(2, 5)] public string secondResponse;
    public string secondDestination;
    [TextArea(2, 5)] public string thirdResponse;
    public string thirdDestination;

}
