using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public AudioSource clickSound;
    public GameObject[] panels; // All panels in one list
    public int panelIndex;      // Which panel THIS button controls

    private Button button;
    private static int currentPanelIndex = -1; // Shared across all buttons

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        // Turn all panels off at start (only once)
        if (currentPanelIndex == -1)
        {
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Escape closes any open panel
        if (Input.GetKeyDown(KeyCode.Escape) && currentPanelIndex != -1)
        {
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex = -1;
        }
    }

    public void OnClick()
    {
        clickSound.Play();
        Debug.Log("Opening panel: " + panels[panelIndex].name);
        // If clicking the same panel → close it
        if (currentPanelIndex == panelIndex)
        {
            panels[panelIndex].SetActive(false);
            currentPanelIndex = -1;
        }
        else
        {
            // Close any open panel first
            if (currentPanelIndex != -1)
                panels[currentPanelIndex].SetActive(false);

            // Open this button's panel
            panels[panelIndex].SetActive(true);
            currentPanelIndex = panelIndex;
        }
    }
}