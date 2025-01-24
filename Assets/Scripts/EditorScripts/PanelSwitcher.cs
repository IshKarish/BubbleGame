using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject firstPanel;
    private GameObject _currentPanel;

    private void Start()
    {
        _currentPanel = firstPanel;
    }

    public void SwitchPanel(GameObject panel)
    {
        _currentPanel.SetActive(false);
        panel.SetActive(true);
        _currentPanel = panel;
    }
}
