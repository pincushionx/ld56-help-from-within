
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public LevelData Level;
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
        Level = Levels[0];
        UnityEngine.SceneManagement.SceneManager.LoadScene(Level.SceneId);
    }

    public void ShowLevel(string sceneId)
    {
        IsSingleLevelGame = true;

        Level = GetLevelBySceneId(sceneId);

        if (Level.SceneId == "" || Level.SceneId == null)
        {
            StartGame();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Level.SceneId);
        }
    }

    public bool HasNextLevel()
    {
        return Level.Index+1 < Levels.Length;
    }
    public LevelData GetNextLevel()
    {
        if (HasNextLevel())
        {
            return Levels[Level.Index + 1];
        }
        return default(LevelData);
    }

    public LevelData GetLevelBySceneId(string sceneId)
    {
        foreach (var level in Levels)
        {
            if(level.SceneId == sceneId) return level;
        }
        return default;
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



    public struct LevelData
    {
        public int Index;
        public string Label;
        public string SceneId;

        public LevelData(int index, string label, string id)
        {
            Index = index;
            Label = label;
            SceneId = id;
        }
    }

    public LevelData[] Levels = new LevelData[] {
        new LevelData(0, "Entry Level", "EntryLevelScene"),
        new LevelData(1, "Getting Over It", "LowClimbScene"),
        new LevelData(2, "Two Tumors", "TwoTumorsScene"),
        new LevelData(3, "We're a Team!", "MergeSplitScene"),
        new LevelData(4, "A Bridge Apart", "BridgeScene"),
        new LevelData(5, "Need a Boost", "MergeSplit2Scene"),
        new LevelData(6, "Stuck Together", "StaticShapeScene"),
        new LevelData(7, "Steps", "SteppedScene"),
        new LevelData(8, "Got a fork?", "PitchforkScene"),
        //new LevelData(2, "Sample Scene", "SampleScene")
    };
}
