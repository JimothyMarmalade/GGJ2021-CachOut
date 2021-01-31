 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Controller/Object References")]
    public LevelBuilder levelController;
    public PlayerMovement player;
	public Camera maincam;

    
    //Scoring References
    public float timeRemaining = 20;
    private bool timerRunning = false;
    public int score = 0;
    public int areasCleared = 0;
    public int areasVisited = -1;

    public int levelSize = 3;
    public int obstacleCount = 0;
    public int treasuresInAreaStart = 3;
    public int treasuresInAreaActual = 0;




    //UI References
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text areasClearedText;
    public TMP_Text potentialTreasuresText;

    public TMP_Text GOScore;
    public TMP_Text GOAreas;
    public TMP_Text GOFinal;

    public GameObject GameOverPanel;

    void Start()
    {
        GameOverPanel.SetActive(false);
        BuildNewLevel();
        timerRunning = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Quitting!");
            Application.Quit();

        }

        //When the player presses Enter or all treasures are collected, a new board is generated
        if ((Input.GetKeyDown(KeyCode.Return) || treasuresInAreaActual <= 0) && timerRunning)
        {
            timerRunning = false;
            BuildNewLevel();
            timerRunning = true;
        }

        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                GameOver();
            }

        }

        UpdateUI();
        
    }

    private void BuildNewLevel()
    {
        if (score >= (levelSize*levelSize))
            levelSize+=2;
        if (levelSize > 40)
            levelSize = 40;
		if ((levelSize+3)/2 < 5)
			maincam.orthographicSize = 5;
		else
			maincam.orthographicSize = (levelSize+3)/2;

        //Calculate new variables needed to generate board
        obstacleCount = levelSize + (areasVisited-3);
		if (obstacleCount > levelSize*levelSize/2)
			obstacleCount = levelSize*levelSize/2;

        treasuresInAreaStart = levelSize;
        if (levelSize > 3)
            treasuresInAreaStart += Random.Range(0, levelSize);
        
        if (levelSize > 5)
            treasuresInAreaStart -= Random.Range(0, levelSize+1);

        levelController.DrawNewBoard(levelSize, obstacleCount, treasuresInAreaStart);

        //increment necessary values
        if (treasuresInAreaActual == 0 && areasVisited >= 0)
            areasCleared++;
        areasVisited++;

        //Reset necessary values
        treasuresInAreaActual = treasuresInAreaStart;
    }
    private void UpdateUI()
    {
        //Debug.Log("UpdateUI");
        scoreText.text = "Treasures Found: " + score;
        areasClearedText.text = "Areas Cached Out: " + areasCleared;
        potentialTreasuresText.text = treasuresInAreaActual.ToString();
        timeText.text = "Time Remaining: " + (int)Mathf.Ceil(timeRemaining);
    }

    public void IncrementScore()
    {
        score++;
        treasuresInAreaActual--;
    }

    public void IncrementTime(int time)
    {
        timeRemaining += time;
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);

        GOScore.text = "Treasures Found: " + score;
        GOAreas.text = "Areas Cached Out: " + areasCleared;
        GOFinal.text = "Final Score: " + (score*areasCleared);

        player.gameObject.SetActive(false);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
    }


}
