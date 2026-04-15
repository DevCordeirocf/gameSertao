using UnityEngine;
using UnityEngine.InputSystem;

public class UiController : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool paused = false;

    void Start()
    {
        HidePauseMenu();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (paused)
                HidePauseMenu();
            else
                ShowPauseMenu();
        }
    }

    private void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        paused = true;
        Time.timeScale = 0f;
    }

    private void HidePauseMenu()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BTN_Resume()
    {
        HidePauseMenu();
    }

    public void BTN_quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
