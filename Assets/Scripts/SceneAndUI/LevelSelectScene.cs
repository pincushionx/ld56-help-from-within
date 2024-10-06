
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelectScene : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        Button button;

        foreach (GameManager.LevelData level in GameManager.Instance.Levels)
        {
            button = _root.Q<Button>(level.SceneId + "Button");
            button.RegisterCallback<ClickEvent>(e => LoadLevel(e, level.SceneId));
        }

        button = _root.Q<Button>("BackButton");
        button.RegisterCallback<ClickEvent>(e => GameManager.Instance.ShowStartScreen());
    }

    private void LoadLevel(ClickEvent evt, string name)
    {
        GameManager.Instance.ShowLevel(name);
    }
}
