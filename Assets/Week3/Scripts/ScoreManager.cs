using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; }

    public Text scoreText;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Score = 0;
    }

    public void Add(int amount)
    {
        Score += amount;
        if(scoreText)
        {
            scoreText.text = $"Score: {Score}";
        }
        Debug.Log($"Score: {Score}");
    }
}
