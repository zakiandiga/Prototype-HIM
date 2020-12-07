using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLightControl : MonoBehaviour
{

    Light2D lt;

    [SerializeField] [Range(0f, 1f)] float transitionDuration;
    
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    private Color currentColor = Color.white;
    private float currentIntensity;
    
    public int currentTemper = 0;
    public TemperState temperState = TemperState.q1;
    
    void Start()
    {        
        lt = GetComponent<Light2D>();

        lt.color = currentColor;
        lt.intensity = currentIntensity;

        TemperChange(1);
    }

    public void TemperChange(int temper)
    {
        currentTemper = temper;
        
        switch (currentTemper)
        {
            case 0:
                temperState = TemperState.q1;
                break;
            case 15:
                temperState = TemperState.q2;
                break;
            case 30:
                temperState = TemperState.q3;
                break;
            case 45:
                temperState = TemperState.q4;
                break;
            default:
                temperState = TemperState.q1;
                break;
        }
        Debug.Log("LIGHTING CURRENT TEMPER IS " + currentTemper + ", and current temper state is " + temperState); ;

        //LightSwitch();
    }

    /*
    private void LightSwitch()
    {
        switch (temperState)
        {
            case TemperState.q1:
                currentColor = Color.Lerp(currentColor, color1, transitionDuration);
                currentIntensity = Mathf.Lerp(currentIntensity, (float)0.3, transitionDuration);
                break;
            case TemperState.q2:
                currentColor = Color.Lerp(currentColor, color2, transitionDuration);
                currentIntensity = Mathf.Lerp(currentIntensity, (float)0.2, transitionDuration);
                break;
            case TemperState.q3:
                currentColor = Color.Lerp(currentColor, color3, transitionDuration);
                currentIntensity = Mathf.Lerp(currentIntensity, (float)0.1, transitionDuration);
                break;
            case TemperState.q4:
                currentColor = Color.Lerp(currentColor, color4, transitionDuration);
                currentIntensity = Mathf.Lerp(currentIntensity, (float)0.05, transitionDuration);
                break;
        }
        lt.color = currentColor;
        lt.intensity = currentIntensity;
        Debug.Log("Light Color & intensity change to " + lt.color + " & " + lt.intensity);
    }
    */

    void Update()
    {
        lt.color = currentColor;
        lt.intensity = currentIntensity;

        switch (temperState)
        {
            case TemperState.q1:
                currentColor = Color.Lerp(currentColor, color1, transitionDuration);
                currentIntensity = Mathf.Lerp(currentIntensity, (float)0.3, transitionDuration);
                break;
            case TemperState.q2:
                currentColor = Color.Lerp(currentColor, color2, transitionDuration);
                currentIntensity = Mathf.Lerp(currentIntensity, (float)0.2, transitionDuration);
                break;
            case TemperState.q3:
                currentColor = Color.Lerp(currentColor, color3, transitionDuration);
                currentIntensity = Mathf.Lerp(currentIntensity, (float)0.1, transitionDuration);
                break;
            case TemperState.q4:
                currentColor = Color.Lerp(currentColor, color4, transitionDuration);
                currentIntensity = Mathf.Lerp(currentIntensity, (float)0.05, transitionDuration);
                break;
        }

        /*
        I tried:
        float t = Mathf.PingPong(Time.time, duration) / duration;
        AND
        float t = (Time.time - startTime) * duration;
        as options with t instead of duration in the currentColor line
        */
        /*
        if((int)DialogueManager.scc.getGameStateValue("temper") != null)
        {
            currentTemper = (int)DialogueManager.scc.getGameStateValue("temper");
        }
        if ((int)DialogueManager.scc.getGameStateValue("temper") == 0)
            temper = Temper.q1;

        if ((int)DialogueManager.scc.getGameStateValue("temper") == 15)
            temper = Temper.q2;

        if ((int)DialogueManager.scc.getGameStateValue("temper") == 30)
            temper = Temper.q3;

        if ((int)DialogueManager.scc.getGameStateValue("temper") == 45)
            temper = Temper.q4;
        */
        /*
        if (temperState == TemperState.q1)
        {
            currentColor = Color.Lerp(currentColor, color1, transitionDuration);
            currentIntensity = Mathf.Lerp(currentIntensity, (float)0.3, transitionDuration);
        }

        if (temperState == TemperState.q2)
        {
            currentColor = Color.Lerp(currentColor, color2, transitionDuration);
            currentIntensity = Mathf.Lerp(currentIntensity, (float)0.2, transitionDuration);
        }

        if (temperState == TemperState.q3)
        {
            currentColor = Color.Lerp(currentColor, color3, transitionDuration);
            currentIntensity = Mathf.Lerp(currentIntensity, (float)0.1, transitionDuration);
        }

        if (temperState == TemperState.q4)
        {
            currentColor = Color.Lerp(currentColor, color4, transitionDuration);
            currentIntensity = Mathf.Lerp(currentIntensity, (float)0.05, transitionDuration);
        }
        */
    }

    public enum TemperState
    {
        q1,
        q2,
        q3,
        q4
    }
}
