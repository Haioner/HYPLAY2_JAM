using UnityEngine.SceneManagement;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene("Game");
    }
}
