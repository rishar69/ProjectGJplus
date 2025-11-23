using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;

    void Start()
    {
        // Main menu BGM otomatis mulai
        AudioManager.Instance.PlayMenuMusic();

        startButton.onClick.AddListener(() => {
            SceneLoader.Load("Gameplay");
        });

        settingsButton.onClick.AddListener(() => {
            // aktifkan panel settings
        });

        quitButton.onClick.AddListener(() => {
            SceneLoader.Quit();
        });
    }
}
