using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "NewCharacter")]
public class Character : ScriptableObject
{
    public string characterName;
    public Sprite ProfPic;
    public string innitialStatus;

}
