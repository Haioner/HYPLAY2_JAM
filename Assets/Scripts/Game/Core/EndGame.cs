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
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI roomCountText;
    
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
        UpdateTexts();
    }

    private void LeaderBoard()
    {
        GameController.instance.SubmitScore();
        playerScoreText.text = GameController.instance.GetUsername() + " Best score " + GameController.instance.GetUserScore().ToString();
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

    private void UpdateTexts()
    {
        timeText.text = "<color=#92E8C0>Time Played</color> <bounce>" + GameController.instance.GetTime();
        roomCountText.text = "<color=#92E8C0>Rooms</color> <bounce>" + FindFirstObjectByType<RoomManager>().GetCurrentRoom().ToString();
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
