using UnityEngine.Rendering;
using UnityEngine;

public class PostProcessingLoader : MonoBehaviour
{
    private Volume m_Volume;
    private int data_PostProcessing = 1;

    private void Awake()
    {
        m_Volume = GetComponent<Volume>();

        if (PlayerPrefs.HasKey("Post"))
            data_PostProcessing = PlayerPrefs.GetInt("Post");

        bool isOn = data_PostProcessing == 1 ? true : false;
        SetPostState(isOn);
    }

    public void SetPostState(bool state)
    {
        m_Volume.enabled = state;
    }
}
