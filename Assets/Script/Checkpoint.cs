using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint Tercapai!");

            GameManager.instance.AddCheckpoint();

            Destroy(gameObject);
        }
    }
}