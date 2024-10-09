using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene("Game");
    }
}
