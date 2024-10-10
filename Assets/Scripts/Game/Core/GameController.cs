using HYPLAY.Leaderboards.Runtime;
using System.Collections.Generic;
using HYPLAY.Core.Runtime;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public HyplayLeaderboard Leaderboard;
    [SerializeField] private HyplayLeaderboard.OrderBy orderBy;
    [SerializeField, Range(1, 15)] private int numScoresToShow = 15;
    public Dictionary<string, double> scoresList = new Dictionary<string, double>();
    public static event System.EventHandler OnScoresAdded;

    [Space]
    [SerializeField] private TextMeshProUGUI currentScoreText;

    private string username;
    private int userScoreIndex = -1;
    private int currentScore;
    private double userScore;

    private void Awake()
    {
        instance = this;

        HyplayBridge.LoggedIn += StartUserScore;
        if (HyplayBridge.IsLoggedIn)
        {
            StartUserScore();
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void AddScore()
    {
        currentScore++;
        currentScoreText.text = "Score " + currentScore.ToString();
    }

    private async void StartUserScore()
    {
        var scores = await Leaderboard.GetScores(orderBy, 0, numScoresToShow);
        var res = await HyplayBridge.GetUserAsync();
        if (scores.Success && res.Success)
        {
            username = res.Data.Username;

            for (var i = 0; i < scores.Data.scores.Length; i++)
            {
                var score = scores.Data.scores[i];
                if (score.username == res.Data.Username)
                    userScore = score.score;
            }
        }
    }

    public async void SubmitScore()
    {
        if (Leaderboard == null) return;

        if (CurrentScoreIsGreaterThanUser())
        {
            var res = await Leaderboard.PostScore(Mathf.RoundToInt(currentScore));
        }
        AddLeaderboardList();
    }

    private bool CurrentScoreIsGreaterThanUser()
    {
        return currentScore > userScore;
    }

    public async void AddLeaderboardList()
    {
        scoresList.Clear();
        var scores = await Leaderboard.GetScores(orderBy, 0, numScoresToShow);
        var res = await HyplayBridge.GetUserAsync();
        if (scores.Success)
        {
            for (int i = 0; i < scores.Data.scores.Length; i++)
            {
                var score = scores.Data.scores[i];
                scoresList.Add(score.username, score.score);

                if(score.username == res.Data.Username)
                    userScoreIndex = i;
            }
        }

        OnScoresAdded?.Invoke(this, System.EventArgs.Empty);
    }

    public string GetUsername()
    {
        return username;
    }

    public int GetUserIndex()
    {
        return userScoreIndex;
    }

    public double GetUserScore()
    {
        if (currentScore > userScore)
            return currentScore;
        else
            return userScore;
    }
}
