using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    public GameObject inGame, leaderBoard;
    private Button nextLevel;
    
    public Text currentLevelText, nextLevelText;
    public Text countText;
    public Image fill;
    public Sprite orange, gray;
    void Awake()
    {
        Instance = this;
        StartCoroutine(StartGame());
    }

    
    void Update()
    {
        if (GameManager.Instance.failed)
        {
            if (leaderBoard.activeInHierarchy)
            {
                GameManager.Instance.failed = false;
                ReStart();
            }
        }
    }
    public void OpenLB()
    {
        inGame.SetActive(false);
        leaderBoard.SetActive(true);
        if (GameManager.Instance.failed)
        {
            currentLevelText.text = PlayerPrefs.GetInt("Level", 1).ToString();
            nextLevelText.text = PlayerPrefs.GetInt("Level", 1) + 1 + "";
            fill.sprite = gray;
        }
        else
        {
            currentLevelText.text = PlayerPrefs.GetInt("Level", 1) - 1 + "";
            nextLevelText.text = PlayerPrefs.GetInt("Level", 1).ToString();
            fill.sprite = orange;
        }
         
        
    }
    void ReStart()
    {
        nextLevel = GameObject.Find("/GameUI/LeaderboardPanel/NextLevel").GetComponent<Button>();
        nextLevel.onClick.RemoveAllListeners();
        nextLevel.onClick.AddListener(() => Reload());
        nextLevel.transform.GetChild(0).GetComponent<Text>().text="Again";
    }

   public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }


    IEnumerator StartGame()
    {
        countText.text = 3.ToString();
        yield return new WaitForSeconds(1);
        countText.color = Color.red;

        countText.text = 2.ToString();
        yield return new WaitForSeconds(1);
        countText.color = Color.yellow;

        countText.text = 1.ToString();
        yield return new WaitForSeconds(1);
        countText.color = Color.green;

        countText.text = "GO!";
        GameManager.Instance.start = true;

        yield return new WaitForSeconds(.5f);
        countText.gameObject.SetActive(false);
    }
}
