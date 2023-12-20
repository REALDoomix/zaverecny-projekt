using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    // Set the target scene name in the Inspector
    public string FinishScene;

    public string requiredTagForTrigger = "Player";
    public string requiredTagForScriptObject = "FinishNode";

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object has a specific tag (optional)
        if (other.CompareTag(requiredTagForTrigger))
        {
            // Check if the object the script is attached to has the required tag
            if (gameObject.CompareTag(requiredTagForScriptObject))
            {
                // Change the scene when the trigger is entered
                SceneManager.LoadScene(FinishScene);
            }
        }
    }
}
