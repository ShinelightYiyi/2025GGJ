using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject music;
    public void Play()
    {
        SceneManager.LoadScene("Prepare");
        DontDestroyOnLoad(music);
    }

    public void Quit()
    {
        Application.Quit();
        
    }
}
