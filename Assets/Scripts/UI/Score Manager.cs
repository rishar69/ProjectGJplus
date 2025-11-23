using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public event System.Action<int> onScoreChanged;

    public int Score { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ResetScore()
    {
        Score = 0;

        onScoreChanged?.Invoke(Score);
    }

    public void AddScore(int amount)
    {
        Score += amount;
        onScoreChanged?.Invoke(Score);
    }
}
