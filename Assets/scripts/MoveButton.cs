using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    public MoveManager moveManager;
    public int moveIndex;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        moveManager.PerformMove(moveIndex);
    }
}