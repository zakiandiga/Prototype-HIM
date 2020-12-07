using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Caster : MonoBehaviour
{
    [SerializeField] private Animator animCurtain;

    public GameObject contentWarning, gameEnd;

    public static event Action<UI_Caster> OnContentWarningClose;

    public Scene uIScene;

    void Start()
    {
        ResponseManager.OnEnding += GameEnd;

        if (SceneManager.GetSceneByName("UI_Scene").isLoaded == false)
        {
            SceneManager.LoadSceneAsync("UI_Scene", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.UnloadSceneAsync("UI_Scene");
        }
    }

    private void OnDestroy()
    {
        ResponseManager.OnEnding -= GameEnd;
    }

    public void ContentWarning()
    {
        contentWarning.GetComponent<Animator>().SetTrigger("close");
        OnContentWarningClose?.Invoke(this);
    }

    private void GameEnd(ResponseManager response)
    {
        gameEnd.GetComponent<Animator>().SetTrigger("open");
        gameEnd.SetActive(true);
    }
}

