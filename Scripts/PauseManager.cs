using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    public TMP_Text pause;

    void Start()
    {
        pause.enabled = false;
    }

    void Update()
    {
        // Check for pause input (e.g., "P" key)
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pause.enabled = true;
        Time.timeScale = 0f; // Pause the game by setting timescale to 0
        isPaused = true;

        // Optionally, disable player controls, pause animations, etc.
        // Example: GameManager.Instance.DisablePlayerControls();

        // Display a pause menu (optional)
        // Example: UIManager.Instance.ShowPauseMenu();
    }

    void ResumeGame()
    {
        pause.enabled = false;
        Time.timeScale = 1f; // Resume the game by restoring timescale to 1
        isPaused = false;

        // Optionally, enable player controls, resume animations, etc.
        // Example: GameManager.Instance.EnablePlayerControls();

        // Hide the pause menu (optional)
        // Example: UIManager.Instance.HidePauseMenu();
    }
}
