using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _creditsPanel;
    
    private AudioSource _audioSource;
    
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        _creditsPanel.SetActive(true);
    }

    public void QuitCredits()
    {
        _creditsPanel.SetActive(false);
    }
    
    public void Quit()
    {
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
