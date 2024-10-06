
using UnityEngine;
using UnityEngine.UIElements;

public class HowToPlayScene : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        Button button = _root.Q<Button>("BackButton");
        button.RegisterCallback<ClickEvent>(e => GameManager.Instance.ShowStartScreen());
    }
}
