using HYPLAY.Leaderboards.Runtime;
using HYPLAY.Core.Runtime;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] private HyplayLeaderboard leaderboard;

    [Space]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI userBestScoreText;

    private int currentScore;
    private double userScore;

    private void Awake()
    {
        instance = this;

        HyplayBridge.LoggedIn += GetUserScore;
        if (HyplayBridge.IsLoggedIn)
        {
            GetUserScore();
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

        //SubmitScore();
    }

    private async void SubmitScore()
    {
        if (leaderboard == null || !CurrentScoreIsGreaterThanUser()) return;

        var res = await leaderboard.PostScore(Mathf.RoundToInt(currentScore));
        if (res.Success)
        {
            userBestScoreText.text =  "Best " + res.Data.score.ToString();
        }
    }

    private bool CurrentScoreIsGreaterThanUser()
    {
        return currentScore > userScore;
    }

    private async void GetUserScore()
    {
        var scores = await leaderboard.GetScores();
        if (scores.Success)
        {
            for (var i = 0; i < scores.Data.scores.Length; i++)
            {
                var score = scores.Data.scores[i];
                var res = await HyplayBridge.GetUserAsync();
                if (score.username == res.Data.Username && res.Success)
                {
                    userScore = score.score;
                    userBestScoreText.text = "Best " + score.score.ToString();
                }
            }
        }
    }
}
