using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private TMP_Text ScoreHole1;
    [SerializeField]
    private TMP_Text ScoreHole2;
    [SerializeField]
    private TMP_Text ScoreHole3;
    [SerializeField]
    private TMP_Text ScoreHole4;


    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void Start()
    {
        if (ScoreHole1)
        {
            int score = PlayerPrefs.GetInt("Hole1BestScore");
            if (score > 0) ScoreHole1.text = score.ToString();
        }
        if (ScoreHole2)
        {
            int score = PlayerPrefs.GetInt("Hole2BestScore");
            if (score > 0) ScoreHole2.text = score.ToString();
        }
        if (ScoreHole3)
        {
            int score = PlayerPrefs.GetInt("Hole3BestScore");
            if (score > 0) ScoreHole3.text = score.ToString();
        }
        if (ScoreHole4)
        {
            int score = PlayerPrefs.GetInt("Hole4BestScore");
            if (score > 0) ScoreHole4.text = score.ToString();
        }
    }
}
