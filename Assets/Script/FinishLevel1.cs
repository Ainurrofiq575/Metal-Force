using System.Collections;
using UnityEngine;

public class FinishLevel1 : MonoBehaviour
{
    public GameObject finishMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            if (GameManager.instance.checkpoint >= GameManager.instance.maxCheckpoint)
            {
                StartCoroutine(ShowFinishMessage());
            }
            else
            {
                Debug.Log("Lewati semua checkpoint dulu!");
            }
        }
    }

    IEnumerator ShowFinishMessage()
    {
        finishMessage.SetActive(true);

        yield return new WaitForSeconds(2f);

        finishMessage.SetActive(false);
    }
}