using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    
    [Header("Audio References")]
    [SerializeField] private MenuMusic menuMusic;
    
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SOUND_VOLUME_KEY = "SoundVolume";

    private void Start()
    {
        LoadSettings();
        
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
        
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void SetMusicVolume(float volume)
    {
        if (menuMusic != null)
        {
            menuMusic.SetVolume(volume);
        }
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }

    public void SetSoundVolume(float volume)
    {
        PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, volume);
    }

    private void LoadSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.6f);
        float soundVolume = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, 1f);
        
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
        
        if (menuMusic != null)
        {
            menuMusic.SetVolume(musicVolume);
        }
    }
}
