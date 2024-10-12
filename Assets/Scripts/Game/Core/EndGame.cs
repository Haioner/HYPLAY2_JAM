using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour
{
    [Header("Leaderboard")]
    [SerializeField] private HighscoreITEM scoreItemPrefab;
    [SerializeField] private Transform scoresHolder;
    [SerializeField] private TextMeshProUGUI playerScoreText;

    [Header("UI")]
    [SerializeField] private MMF_Player endFeedback;
    [SerializeField] private GameObject autoRestartButton;
    
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
        autoRestartButton.SetActive(canAutoRestart);
    }

    public void DisableAutoRestart()
    {
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

        endFeedback.PlayFeedbacks();
    }

    private void LeaderBoard()
    {
        GameController.instance.SubmitScore();
        playerScoreText.text = "Best " + GameController.instance.GetUsername() + " " + GameController.instance.GetUserScore().ToString();
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
        autoRestartButton.SetActive(toggleValue);
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
