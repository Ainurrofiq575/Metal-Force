using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 100f;

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Coin berhasil di ambil");

            GameManager.instance.AddPoint();

            Destroy(gameObject);
        }
    }
}