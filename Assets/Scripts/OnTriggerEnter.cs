using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    // Set the target scene name in the Inspector
    
    public string requiredTagForTrigger = "Player";
    public string requiredTagForScriptObject = "Finish";

    private void OnTriggerEnter(Collider other)
    {
        
        // Check if the entering object has a specific tag (optional)
        if (gameObject.CompareTag(requiredTagForScriptObject))
        {
            Debug.Log(other.tag);
            // Check if the object the script is attached to has the required tag
            if (other.CompareTag(requiredTagForTrigger))
            {
                // Change the scene when the trigger is entered
                SceneManager.LoadScene("FinishScene");
            }
        }
    }
}