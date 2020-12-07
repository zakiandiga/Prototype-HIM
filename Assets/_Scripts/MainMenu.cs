using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator animMenu, animCurtain;

    private bool inCredits = false;

    private void Start()
    {

    }

    public void ToCredits()
    {        
        if(!inCredits)
        {
            animMenu.SetTrigger("credits");
            inCredits = true;
        }
        else if (inCredits)
        {
            animMenu.SetTrigger("main");
            inCredits = false;
        }        
    }

    public void ToGame()
    {
        animCurtain.SetTrigger("trans");        
        Invoke("PlayGame", 0.8f);
    }
    

    // Start is called before the first frame update
    private void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }
}
