using UnityEngine;

public class GameplayMusic: MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayGameplayMusic();
    }
}
