using UnityEngine;
using Week7;

namespace Week7
{
    public class NPCInteraction : MonoBehaviour
    {
        [SerializeField] private float interactDistance = 2f;
        [SerializeField] private Transform player;
        [SerializeField] private NPCDialogue dialogue;

        private bool scoreGoalReached = false;
        private bool hasSpoken = false;

        private void Start()
        {
            ScoreManager.Instance.OnScoreGoalReached += HandleScoreGoalReached;
        }

        private void Update()
        {
            if (player == null) return;

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist <= interactDistance)
            {
                if (!hasSpoken)
                {
                    hasSpoken = true;
                    Speak();
                }
            }
            else
            {
                hasSpoken = false;
            }
        }

        private void Speak()
        {
            if (scoreGoalReached)
                dialogue.ShowGoalDialogue();
            else
                dialogue.ShowNormalDialogue();
        }

        private void HandleScoreGoalReached()
        {
            scoreGoalReached = true;

            // ถ้าผู้เล่นยืนอยู่ใกล้ตอน event เกิด ก็ให้พูดประโยค goal ทันที
            float dist = Vector3.Distance(player.position, transform.position);
            if (dist <= interactDistance)
            {
                dialogue.ShowGoalDialogue();
            }
        }
    }
}
