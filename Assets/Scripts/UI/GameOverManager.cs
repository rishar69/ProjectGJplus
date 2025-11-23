using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("UI")]
    public CanvasGroup gameOverPanel;

    private void Awake()
    {
        Instance = this;

        // Hide at start
        gameOverPanel.alpha = 0;
        gameOverPanel.blocksRaycasts = false;
        gameOverPanel.interactable = false;
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");

        gameOverPanel.alpha = 1;
        gameOverPanel.blocksRaycasts = true;
        gameOverPanel.interactable = true;

        Time.timeScale = 0f;
    }
}
