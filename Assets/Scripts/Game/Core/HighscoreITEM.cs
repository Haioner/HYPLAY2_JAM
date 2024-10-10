using UnityEngine;
using TMPro;

public class HighscoreITEM : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Color playerTextColor;

    public void InitItem(string rank, string username, string score)
    {
        gameObject.SetActive(true);
        rankText.text = rank;
        usernameText.text = username;
        scoreText.text = score;
    }

    public void IsPlayerItem()
    {
        rankText.color = playerTextColor;
        usernameText.color = playerTextColor;
        scoreText.color = playerTextColor;
    }
}
