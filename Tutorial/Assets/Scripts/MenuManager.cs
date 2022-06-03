using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManager;
    public bool GameState;
    public GameObject menuElement;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public static int score;
    public GameObject player;
    private void Start()
    {
        GameState = false;
        menuManager = this;
        highScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("score", score);
    }
    private void Update()
    {
        scoreText.text = "" + score;
        if (player == null)
        {
            StartCoroutine(ReLVL());
        }
    }
    public void StartTheGame()
    {
        GameState = true;
        menuElement.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        GameObject.FindWithTag("Particale").GetComponent<ParticleSystem>().Play();
    }
    IEnumerator ReLVL()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
}
