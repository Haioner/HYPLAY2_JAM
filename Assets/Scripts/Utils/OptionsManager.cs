using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private KeyCode optionsInput;

    [Header("Animations")]
    [SerializeField] private DOTweenAnimation gamePausedDOT;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [Space]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicText;
    [Space]
    [SerializeField] private Slider soundSlider;
    [SerializeField] private TextMeshProUGUI soundText;

    [Header("PostProcessing")]
    [SerializeField] private Toggle postToggle;

    private CanvasGroup canvasGroup;
    private bool optionsIsOn;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        LoadMusic();
        LoadSound();
        LoadPostProcessing();
    }

    private void Update()
    {
        if (Input.GetKeyDown(optionsInput))
        {
            SwitchOptions();
        }
    }

    public bool GetOptionsState()
    {
        return optionsIsOn;
    }

    public void SwitchOptions()
    {
        optionsIsOn = !optionsIsOn;
        Time.timeScale = optionsIsOn ? 0 : 1;
        gamePausedDOT.DORestart();
        SwitchCanvasGroup();
    }

    public void SwitchOptionsInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SwitchOptions();
        }
    }

    private void SwitchCanvasGroup()
    {
        if (optionsIsOn)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void BackToMenu()
    {
        TransitionController.instance.TransitionToSceneName("Menu");
    }

    //Music
    private void LoadMusic()
    {
        if (PlayerPrefs.HasKey("MVolume"))
        {
            float keyValue = PlayerPrefs.GetFloat("MVolume");
            musicSlider.value = keyValue;

            float key_100 = keyValue * 100;
            musicText.text = key_100.ToString("F0");
        }
    }

    public void UpdateMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 40);
        PlayerPrefs.SetFloat("MVolume", value);

        float key_100 = value * 100;
        musicText.text = key_100.ToString("F0");
    }

    //Sound
    private void LoadSound()
    {
        if (PlayerPrefs.HasKey("SVolume"))
        {
            float keyValue = PlayerPrefs.GetFloat("SVolume");
            soundSlider.value = keyValue;

            float key_100 = keyValue * 100;
            soundText.text = key_100.ToString("F0"); 
        }
    }

    public void UpdateSoundVolume(float value)
    {
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(value) * 40);
        PlayerPrefs.SetFloat("SVolume", value);

        float key_100 = value * 100;
        soundText.text = key_100.ToString("F0");
    }

    #region PostProcessing
    public void SwitchPostProcessing(bool isOn)
    {
        int postState = isOn ? 1 : 0;
        PlayerPrefs.SetInt("Post", postState);
        UpdatePostProcessingLoaders(isOn);
    }

    private void UpdatePostProcessingLoaders(bool isOn)
    {
        PostProcessingLoader[] allPostLoaders = FindObjectsByType<PostProcessingLoader>(FindObjectsSortMode.InstanceID);
        foreach (PostProcessingLoader item in allPostLoaders)
            item.SetPostState(isOn);
    }

    private void LoadPostProcessing()
    {
        if (PlayerPrefs.HasKey("Post"))
        {
            bool isOn = PlayerPrefs.GetInt("Post") == 1 ? true : false;
            postToggle.SetIsOnWithoutNotify(isOn);
            UpdatePostProcessingLoaders(isOn);
        }
    }
    #endregion
}
