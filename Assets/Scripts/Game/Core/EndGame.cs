using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EndGame : MonoBehaviour
{
    [Header("Leaderboard")]
    [SerializeField] private HighscoreITEM scoreItemPrefab;
    [SerializeField] private Transform scoresHolder;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    
    private static bool canAutoRestart;
    private CanvasGroup cg;

    private void OnEnable()
    {
        GameController.OnScoresAdded += SpawnScores;
    }

    private void OnDisable()
    {
        GameController.OnScoresAdded -= SpawnScores;
    }

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            canAutoRestart = false;
    }

    public void ActiveEndGame()
    {
        Time.timeScale = 1f;
        LeaderBoard();

        if (canAutoRestart)
        {
            RestartScene();
            return;
        }

        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    private void LeaderBoard()
    {
        GameController.instance.SubmitScore();
        playerScoreText.text = GameController.instance.GetUsername() + " : " + GameController.instance.GetUserScore().ToString();
    }

    private void SpawnScores(object sender, System.EventArgs e)
    {
        int iterator = 0;
        foreach (KeyValuePair<string, double> scoreEntry in GameController.instance.scoresList)
        {
            string playerName = scoreEntry.Key;
            double playerScore = scoreEntry.Value;
            string rank = "th";
            if (iterator == 0) rank = "st";
            else if (iterator == 1) rank = "nd";
            else if (iterator == 2) rank = "rd";

            HighscoreITEM item = Instantiate(scoreItemPrefab, scoresHolder);
            item.InitItem
                ((iterator + 1).ToString() + rank,
                playerName,
                playerScore.ToString());

            if (iterator == GameController.instance.GetUserIndex())
                item.IsPlayerItem();

            iterator++;
        }
    }

    public void SetAutoRestart(bool toggleValue)
    {
        canAutoRestart = toggleValue;
    }

    public void RestartScene()
    {
        TransitionController.instance.TransitionToSceneName("Game");
    }

    public void BackToMenu()
    {
        TransitionController.instance.TransitionToSceneName("Menu");
    }
}
