using System.Collections;
using UnityEngine;

public class StartGate : MonoBehaviour
{
    public GameObject startMessage;

    private bool hasStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player") && !hasStarted)
        {
            hasStarted = true;

            if (GameManager.instance != null)
            {
                GameManager.instance.StartTimer();
            }

            StartCoroutine(ShowStartMessage());
        }
    }

    IEnumerator ShowStartMessage()
    {
        if (startMessage != null)
        {
            startMessage.SetActive(true);

            yield return new WaitForSeconds(2f);

            startMessage.SetActive(false);
        }
    }
}