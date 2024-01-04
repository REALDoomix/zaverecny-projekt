using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTrigger : MonoBehaviour
{   
    public string playerTag = "Player";
    public string finishTag = "Finish";
    public string deathBorderTag = "DeathBorder";

    private void OnTriggerEnter(Collider other)
    {
        
        // Check if the entering object has a specific tag (optional)
        if (gameObject.CompareTag(finishTag))
        {
            // Check if the object the script is attached to has the required tag
            if (other.CompareTag(playerTag))
            {
                SceneManager.LoadScene("FinishScene");
            }
        }
        if (gameObject.CompareTag(deathBorderTag)){
            if (other.CompareTag(playerTag))
            {
                SceneManager.LoadScene("DeathMenu");
            }
        }
    }
}