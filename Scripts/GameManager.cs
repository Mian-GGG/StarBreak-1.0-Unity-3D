using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static bool isPaused;
    public static int sceneNum = 1;
    public GameObject storyMode, part1, part2, part3, part4, next, acceptMission, startPanel;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        StartCoroutine(ResumeGame());
    }

    private IEnumerator ResumeGame()
    {
        Debug.Log("Trying to Resume"); Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    
    AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("Pause");

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

    }

    public void Pause()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        if (isPaused)
        {
            SceneManager.UnloadSceneAsync("Pause");
            isPaused = false;
        }

        SceneManager.LoadScene(sceneNum);
    }

    public void QuitToMainMenu()
    {
        isPaused = false;
        SceneManager.LoadScene("Start");
    }
    
    public void QuitApplication()
    {
        Application.Quit();
    }

    public void NextLevel()
    {
        sceneNum++;
        SceneManager.LoadScene(sceneNum);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void StartBreifing()
    {
        if (startPanel.activeSelf)
        {
            startPanel.SetActive(false);
            storyMode.SetActive(true);
        }
    }

    public void NextPart()
    {
        if (part1.activeSelf)
        {
            part1.SetActive(false);
            part2.SetActive(true);
        }
        else if (part2.activeSelf)
        {
            part2.SetActive(false);
            part3.SetActive(true);
        }
        else if (part3.activeSelf)
        {
            part3.SetActive(false);
            part4.SetActive(true);
            next.SetActive(false);
            acceptMission.SetActive(true);
        }
    }

}
