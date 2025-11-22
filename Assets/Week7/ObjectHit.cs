using UnityEngine;

namespace Week7
{
    public class ObjectHit : MonoBehaviour, IScoreSource , IInteractable
    {
        [SerializeField] private int score = 1;

        public int GetScore() => score;

        public void Hit()
        {
            ScoreManager.Instance.AddScore(GetScore());
        }

        public void Interact(GameObject interactor)
        {
            Hit();
        }
    }
}
