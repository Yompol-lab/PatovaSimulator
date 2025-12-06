using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    public static SettingsPanelController Instance;

    [Header("Panel de Ajustes")]
    public GameObject settingsPanel;

    [Header("Sliders")]
    public Slider conflictSlider;
    public Slider toleranceSlider;

    [Header("Variables del sistema")]
    public float conflictProbability = 0.2f;
    public float toleranceMultiplier = 1f;

    private bool panelOpen = false;

    private void Awake()
    {
        Instance = this;
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public bool IsOpen
    {
        get { return panelOpen; }
    }

    
    public void TogglePanel()
    {
        panelOpen = !panelOpen;

        if (settingsPanel != null)
            settingsPanel.SetActive(panelOpen);

        UpdateValues();
    }

    private void UpdateValues()
    {
        if (conflictSlider != null)
            conflictProbability = conflictSlider.value;

        if (toleranceSlider != null)
            toleranceMultiplier = toleranceSlider.value;
    }
}
