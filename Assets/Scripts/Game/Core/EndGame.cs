using UnityEngine.SceneManagement;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public void RestartScene()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene("Game");
        TransitionController.instance.TransitionToSceneName("Game");
    }
}
