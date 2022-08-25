using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using System.IO;


public class GameManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    public static bool playerIsDead = false;
    public static float timer = 0f;
    public Text time_used_text;
    public Text Highest_score_text;
    private int menuCounter;
    public GameObject menu;
    [SerializeField]
    PlayerData player_data;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Time.timeScale = 1;
            playerIsDead = false;
            menuCounter = 0;
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Time.timeScale = 1;
            menuCounter = 0;
            player_data = new PlayerData();
            time_used_text = GameObject.Find("UI")?.transform.Find("time")?.GetComponent<Text>();
            playerIsDead = false;
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Time.timeScale = 1;
            menuCounter = 0;
            time_used_text = GameObject.Find("UI")?.transform.Find("time")?.GetComponent<Text>();
            Highest_score_text = GameObject.Find("UI")?.transform.Find("highest_score")?.GetComponent<Text>();
            playerIsDead = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 & !playerIsDead)
        {
            timer += 1f * Time.deltaTime;
            time_used_text.text = "Time " + Math.Round(timer, 2);
            
        }
        
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            Time.timeScale = 1;

            time_used_text.text = "Surviving for "+ Math.Round(timer, 2)+"s";
            player_data = new PlayerData();
           // Debug.Log(player_data.data);
            
            if (JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("car_chasing_data")) == null)
            {
                PlayerPrefs.SetString("car_chasing_data", JsonUtility.ToJson(player_data));
            } 
            player_data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("car_chasing_data"));
            player_data.data = timer;
            if (player_data.highest_data < player_data.data)
            {
                player_data.highest_data = player_data.data;
                //Debug.Log("done");
            }
            PlayerPrefs.SetString("car_chasing_data", JsonUtility.ToJson(player_data));
            Highest_score_text.text = "Highest: " + Math.Round(player_data.highest_data, 2) + "s";
            /*
            
            player_data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("jsondata"));
            highestScore = player_data.data;
            int score = (int)timer;
            if (highestScore < score)
            {
                player_data.data = score;
                highestScore = score;
                Debug.Log("done");
            }
            PlayerPrefs.SetString("jsondata", JsonUtility.ToJson(player_data));
            Highest_score_text.text = "Highest: "+ highestScore + "s";*/


        }
       
        if ((playerIsDead) & SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(NextStage(3f));
        }
    }

    public void PlayerIsDead()
    {
        playerIsDead = true;
    }


    



    #region menu
    public void ActivateMenu()
    {
        if (menuCounter == 0)
        {
            StartCoroutine(Menu());
            menuCounter += 1;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            StartCoroutine(DeMenu());
            menuCounter = 0;
        }

    }
    IEnumerator Menu()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        RectTransform menuPos = menu.GetComponent<RectTransform>();
        menuPos.anchoredPosition = new Vector3(0, 0, 0);

    }

    IEnumerator DeMenu()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        RectTransform menuPos = menu.GetComponent<RectTransform>();
        menuPos.anchoredPosition = new Vector3(0, 2300, 0);

    }

    public void BackToHomePage()
    {
        timer = 0;
        Time.timeScale = 1;
        menuCounter = 0;
        playerIsDead = false;
        levelLoader.LoadSpecificScene("Start");

    }


    public void ReTry()
    {
        timer = 0;
        Time.timeScale = 1;
        menuCounter = 0;
        playerIsDead = false;
        levelLoader.LoadSpecificScene("Main Game");
    }

    public void StartGame()
    {
        levelLoader.LoadSpecificScene("Main Game");
    }


    public void QuitGame()
    {
        StartCoroutine(QuitG());
    }

    public bool GetPlayerIsDead()
    {
        return playerIsDead;
    }

    IEnumerator QuitG()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Application.Quit();
    }

    IEnumerator NextStage(float interval)
    {
        yield return new WaitForSecondsRealtime(interval);
        levelLoader.LoadNextLevel();
    }


    #endregion


    [System.Serializable]
    public class PlayerData
    {
        public float data;
        public float highest_data;
    }
}
