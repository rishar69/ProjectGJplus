using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

        if (health == null)
        {
            Debug.LogError("[PlayerDeathHandler] No Health component found on player!");
            return;
        }

        // Subscribe to health event
        health.OnDeath.AddListener(OnPlayerDied);
    }

    private void OnPlayerDied()
    {
        Debug.Log("PLAYER DIED  Trigger Game Over");


        GameOverManager.Instance.GameOver();
    }
}
