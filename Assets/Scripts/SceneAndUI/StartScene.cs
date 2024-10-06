using UnityEngine;
using UnityEngine.UIElements;

public class StartScene : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;

    private Slider _musicSlider;
    private Slider _soundSlider;


    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        Button button = _root.Q<Button>("StartButton");
        button.RegisterCallback<ClickEvent>(StartGame);

        button = _root.Q<Button>("LevelSelectButton");
        button.RegisterCallback<ClickEvent>(e => GameManager.Instance.ShowLevelSelectScene());

        button = _root.Q<Button>("HowToPlayButton");
        button.RegisterCallback<ClickEvent>(e => GameManager.Instance.ShowHowToPlayScene());

        //button = _root.Q<Button>("CreditsButton");
        //button.RegisterCallback<ClickEvent>(e => GameManager.Instance.ShowCreditsScene());

        _musicSlider = _root.Q<Slider>("MusicSlider");
        float volume = GameManager.Instance.GetMusicVolumeSliderValue();
        _musicSlider.SetValueWithoutNotify(volume);
        _musicSlider.RegisterValueChangedCallback<float>(MusicVolumeChanged);

        _soundSlider = _root.Q<Slider>("SFXSlider");
        volume = GameManager.Instance.GetSoundVolumeSliderValue();
        _soundSlider.SetValueWithoutNotify(volume);
        _soundSlider.RegisterValueChangedCallback<float>(SoundVolumeChanged);
    }

    private void StartGame(ClickEvent evt)
    {
        GameManager.Instance.StartGame();
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
