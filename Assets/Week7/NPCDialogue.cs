using UnityEngine;

namespace Week7
{
    public class NPCDialogue : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI textBubble;
        [SerializeField] private string normalText = "Collect All Coins!!";
        [SerializeField] private string goalText = "Door is Open Now!!";

        public void ShowNormalDialogue()
        {
            textBubble.text = normalText;
        }

        public void ShowGoalDialogue()
        {
            textBubble.text = goalText;
        }
    }

}
