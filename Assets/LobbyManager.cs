using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject start;
    public GameObject settings;
    public GameObject exit;
    public GameObject back;
    public GameObject gameName;
    public GameObject musicController;
    public GameObject level1;
    public GameObject level2;

    public GameObject quality;
    public GameObject qualityText;

    public GameObject imageIn;
    public GameObject imageInParent;
    private float startCounter = 1f;
    private bool gameStart = false;
    private int level;

    public AudioMixer audio_Mixer;


    private void Update()
    {
        if(gameStart)
        {
            startCounter -= Time.deltaTime;
            if(startCounter < 0)
            {
                gameStart = false;
                startCounter = 1f;

                startGame_Real();
            }
        }
    }

    public void levelChoose()
    {
        start.SetActive(false);
        settings.SetActive(false);
        exit.SetActive(false);

        level1.SetActive(true);
        level2.SetActive(true);
        back.SetActive(true);
    }

    public void startGame(int s)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;

        gameStart = true;
        GameObject t = Instantiate(imageIn, imageInParent.transform);
        //Destroy(t, .93f);

        level = s;
    }

    void startGame_Real()
    {
        if(level == 1)
        {
            SceneManager.LoadScene("TheGame");
        }
        else
        {
            SceneManager.LoadScene("TheGame2");
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        start.SetActive(false);
        settings.SetActive(false);
        exit.SetActive(false);
        gameName.SetActive(false);

        quality.SetActive(true);
        qualityText.SetActive(true);

        musicController.SetActive(true);
        back.SetActive(true);
    }

    public void Back()
    {
        start.SetActive(true);
        settings.SetActive(true);
        exit.SetActive(true);
        gameName.SetActive(true);

        quality.SetActive(false);
        qualityText.SetActive(false);

        musicController.SetActive(false);
        back.SetActive(false);

        level1.SetActive(false);
        level2.SetActive(false);
    }

    public void Quality(int index)
    {
        if (index == 3) index += 2;
        if (index == 2) index += 1;
        if (index == 1) index += 1;
        QualitySettings.SetQualityLevel(index);
    }

    public void musicChanger(float volume)
    {
        audio_Mixer.SetFloat("volume", volume);
    }

    public void fullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }
}
