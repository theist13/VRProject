using UnityEngine;

namespace Week7
{
    public class Coin : MonoBehaviour, IScoreSource
    {
        [SerializeField] private int score = 1;

        public int GetScore() => score;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ScoreManager.Instance.AddScore(GetScore());
                Destroy(gameObject);
            }
        }
    }
}
