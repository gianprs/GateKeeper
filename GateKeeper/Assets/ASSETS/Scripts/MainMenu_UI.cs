using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    public GameObject playButton, scoreButton, scoreTable, backButton;


    // Start is called before the first frame update
    void Start()
    {
        playButton.SetActive(true);
        scoreButton.SetActive(true);

        //scoreTable.SetActive(false);
        backButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowHighScore()
    {
        playButton.SetActive(false);
        scoreButton.SetActive(false);

        backButton.SetActive(true);
        //scoreTable.SetActive(true);
    }

    public void BackToMain()
    {
        playButton.SetActive(true);
        scoreButton.SetActive(true);

        backButton.SetActive(false);
        //scoreTable.SetActive(false);
    }
}
