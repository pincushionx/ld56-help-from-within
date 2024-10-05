
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public string Day = null;
    public bool IsSingleLevelGame = false;

    public AudioMixer AudioMixer;

    const float minVolume = -80f;
    const float maxVolume = 20f;

    private void Awake()
    {
        AudioMixer = Resources.Load<AudioMixer>("Master");
    }

    public void ShowStartScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }

    public void ShowLevelSelectScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectScene");
    }

    public void ShowHowToPlayScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HowToPlayScene");
    }

    public void ShowCreditsScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CreditsScene");
    }

    public void StartGame()
    {
        IsSingleLevelGame = false;
        Day = null;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void ShowLevel(string dayName)
    {
        IsSingleLevelGame = true;
        Day = dayName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }


    public void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSoundVolume(float volume)
    {
        AudioMixer.SetFloat("SoundVolume", volume);
    }

    public float GetMusicVolumeSliderValue()
    {
        float volume;
        AudioMixer.GetFloat("MusicVolume", out volume);
        return volume;
    }
    public float GetSoundVolumeSliderValue()
    {
        float volume;
        AudioMixer.GetFloat("SoundVolume", out volume);
        return volume;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
