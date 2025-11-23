using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1f;
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ExitGame()
    {
        Debug.Log("EXIT GAME CALLED");

        #if UNITY_EDITOR
                // For Unity Editor
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // For actual build
                Application.Quit();
        #endif
    }
}
