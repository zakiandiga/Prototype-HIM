using UnityEngine;
using UnityEngine.UI;

public class SystemTime : MonoBehaviour
{
    public GameObject timePanel;
    public GameObject datePanel;
    
    Text timeText;
    Text dateText;
     

    void Start()
    {
        dateText = datePanel.GetComponent<Text>();
        timeText = timePanel.GetComponent<Text>();
    }

    private void Update()
    {
        timeText.text = System.DateTime.Now.ToString("hh:mm");
        dateText.text = System.DateTime.Now.ToString("D");
    }
}
