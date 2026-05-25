using UnityEngine;

public class StartLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("START!");
        }
    }
}