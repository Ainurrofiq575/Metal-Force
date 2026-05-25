using System.Collections;
using UnityEngine;

public class StartGate : MonoBehaviour
{
    public GameObject startMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            StartCoroutine(ShowStartMessage());
        }
    }

    IEnumerator ShowStartMessage()
    {
        startMessage.SetActive(true);

        yield return new WaitForSeconds(2f);

        startMessage.SetActive(false);
    }
}