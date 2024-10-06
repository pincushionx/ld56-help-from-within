using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;

    private Slider _musicSlider;
    private Slider _soundSlider;
    //private void Awake()
    //{
    //    _document = GetComponent<UIDocument>();
    //    _root = _document.rootVisualElement;

    //    Button button = _root.Q<Button>("ResumeButton");
    //    button.RegisterCallback<ClickEvent>(Resume);

    //    _musicSlider = _root.Q<Slider>("MusicSlider");
    //    _musicSlider.RegisterValueChangedCallback<float>(MusicVolumeChanged);

    //    _soundSlider = _root.Q<Slider>("SFXSlider");
    //    _soundSlider.RegisterValueChangedCallback<float>(SoundVolumeChanged);
    //}

    private void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        Button button = _root.Q<Button>("ResumeButton");
        button.RegisterCallback<ClickEvent>(Resume);

        button = _root.Q<Button>("RestartLevelButton");
        button.RegisterCallback<ClickEvent>(RestartLevel);

        button = _root.Q<Button>("BackToStartButton");
        button.RegisterCallback<ClickEvent>(BackToStart);

        _musicSlider = _root.Q<Slider>("MusicSlider");
        _musicSlider.RegisterValueChangedCallback<float>(MusicVolumeChanged);

        _soundSlider = _root.Q<Slider>("SFXSlider");
        _soundSlider.RegisterValueChangedCallback<float>(SoundVolumeChanged);


        float volume = GameManager.Instance.GetMusicVolumeSliderValue();
        _musicSlider.SetValueWithoutNotify(volume);

        volume = GameManager.Instance.GetSoundVolumeSliderValue();
        _soundSlider.SetValueWithoutNotify(volume);
    }

    private void Resume(ClickEvent e)
    {
        TimeKeeper.Instance.Unpause();
    }
    private void RestartLevel(ClickEvent e)
    {
        GameManager.LevelData level = GameManager.Instance.Level;
        GameManager.Instance.ShowLevel(level.SceneId);
    }
    private void BackToStart(ClickEvent e)
    {
        GameManager.Instance.ShowStartScreen();
    }
    private void MusicVolumeChanged(ChangeEvent<float> evt)
    {
        GameManager.Instance.SetMusicVolume(evt.newValue);
    }
    private void SoundVolumeChanged(ChangeEvent<float> evt)
    {
        GameManager.Instance.SetSoundVolume(evt.newValue);
    }
}
