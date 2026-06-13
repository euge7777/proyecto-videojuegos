using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverPanel;

    public float gameOverPanelDelay = 1.025f;

    private void Awake()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;

        StartCoroutine(ShowGameOverWithDelay());
    }

    private IEnumerator ShowGameOverWithDelay()
    {
        yield return new WaitForSecondsRealtime(gameOverPanelDelay);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RiseAgain()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}