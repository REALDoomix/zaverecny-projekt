using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame10(){
        PlayerPrefs.SetInt("size", 10);
        SceneManager.LoadScene("Game");
    }
    public void StartGame20(){
        PlayerPrefs.SetInt("size", 20);
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
