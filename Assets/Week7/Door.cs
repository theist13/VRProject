using UnityEngine;
using UnityEngine.SceneManagement;
namespace Week7
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
