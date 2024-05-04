using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public PlayerLook PlayerLook;
    private playerManager playerManager;

    public GameObject exitButton;
    public GameObject restartButton;
    public GameObject katanaTutorialText;
    public GameObject sensitivity;
    public GameObject sensitivitySlider;
    public GameObject music;
    public AudioMixer audioMixer;

    private bool gameStop = false;
    private bool playerDead = false;


    private float timeToKatanaTutorialText = 25f;
    private bool played = false;

    private void Start()
    {
        if(GameObject.Find("Player") != null)
        {
            playerManager = GameObject.Find("Player").GetComponent<playerManager>();
        }
    }

    private void Update()
    {
        if (playerManager != null)
        {
            playerDead = playerManager.isDead;
            timeToKatanaTutorialText -= Time.deltaTime;
        }
        else
            playerDead = false;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameStop = !gameStop;

            if (gameStop)
                GameStop();

            else if (!gameStop)
                GameContinue();
        }

        if( timeToKatanaTutorialText <= 0 && !played)
        {
            played = true;

            katanaTutorialText.GetComponent<Animator>().SetTrigger("Start");
            katanaTutorialText.GetComponent<Animator>().SetTrigger("End");
        }
    }

    public void GameStop()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        exitButton.SetActive(true);
        restartButton.SetActive(true);
        sensitivity.SetActive(true);
        sensitivitySlider.SetActive(true);
        music.SetActive(true);

        PlayerLook.enabled = false;
        Time.timeScale = 0f;
    }

    void GameContinue()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        exitButton.SetActive(false);
        restartButton.SetActive(false);
        sensitivity.SetActive(false);
        sensitivitySlider.SetActive(false);
        music.SetActive(false);

        PlayerLook.enabled = true;
        Time.timeScale = 1f;
    }

    public void RestartGame(int s)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;

        if(s == 1)
            SceneManager.LoadScene("TheGame");

        if (s == 2)
            SceneManager.LoadScene("TheGame2");
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

    public void musicChanger(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
