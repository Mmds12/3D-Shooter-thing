using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerLook PlayerLook;

    public GameObject exitButton;
    public GameObject restartButton;

    private bool gameStop = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameStop = !gameStop;

            if (gameStop)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                PlayerLook.enabled = false;
                Time.timeScale = 0f;

                exitButton.SetActive(true);
                restartButton.SetActive(true);
            }

            else if (!gameStop)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                PlayerLook.enabled = true;
                Time.timeScale = 1f;

                exitButton.SetActive(false);
                restartButton.SetActive(false);
            }
        }
    }

    public void startGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene("TheGame");
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        PlayerLook.enabled = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
