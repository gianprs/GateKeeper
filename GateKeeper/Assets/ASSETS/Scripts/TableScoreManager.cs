using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableScoreManager : MonoBehaviour
{
    public Text[] playerName;
    public Text[] playerScore;

    public HighScoreManager _highscoreManager;

    public string player;
    public int finalScore;
    public bool add;

    // Start is called before the first frame update
    void Start()
    {
        _highscoreManager.GetData();
        for (int i = 0; i < playerScore.Length; i++)
        {

            playerScore[i].text = _highscoreManager.finalScores[i].score.ToString();

            playerName[i].text = _highscoreManager.finalScores[i].name.ToString();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (add)
        {
            _highscoreManager.GetData();
            _highscoreManager.AddItem(player, finalScore);
            for (int i = 0; i < playerScore.Length; i++)
            {
                playerScore[i].text = _highscoreManager.finalScores[i].score.ToString();

                playerName[i].text = _highscoreManager.finalScores[i].name.ToString();
            }
            add = false;
        }
    }
}
