using UnityEngine;
using System;
using TMPro;
namespace Week7
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        public event Action OnScoreGoalReached;

        [SerializeField] private int goalScore = 10;
        private int currentScore = 0;

        [SerializeField] private TextMeshProUGUI scoreText;

        private void Awake()
        {
            Instance = this;
        }

        public void AddScore(int amount)
        {
            currentScore += amount;

            if (scoreText)
            {
                scoreText.text = $"Score : {currentScore}";
            }

            if (currentScore >= goalScore)
            {
                OnScoreGoalReached?.Invoke();
            }
        }
    }

}

