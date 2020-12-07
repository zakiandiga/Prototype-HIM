using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionTest : MonoBehaviour
{

    public bool turnOnLight = false;
    public Material phoneScreen;

    /*private void Awake()
    {
        phoneScreen.DisableKeyword("_EMISSION");
    }*/

    void Update()
    {
        if (turnOnLight == true)
            phoneScreen.EnableKeyword("_EMISSION");
        else
            phoneScreen.DisableKeyword("_EMISSION");
    }
}
