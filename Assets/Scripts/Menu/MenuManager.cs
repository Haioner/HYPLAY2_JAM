using HYPLAY.Core.Runtime;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private MMF_Player loginFeedback;
    [SerializeField] private GameObject loginHolder;

    private void Awake()
    {
        Time.timeScale = 1;
        audioMixer.SetFloat("MasterPitch", 1f);

        if (HyplayBridge.IsLoggedIn)
        {
            StartLogged();
            loginHolder.SetActive(false);
        }
    }

    public async void StartLogged()
    {
        var res = await HyplayBridge.GetUserAsync();
        if (res.Success)
        {
            userNameText.text = res.Data.Username + " logged";
            loginFeedback.PlayFeedbacks();
        }
    }

    public void GameScene()
    {
        SceneManager.LoadScene("Game");
    }
}
