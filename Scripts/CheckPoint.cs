using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public AudioClip checkPointSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlayClipAt(checkPointSound, transform.position); // joue la musique spécifiée à l'endroit spécifié
            CurrentSceneManager.instance.respawnPoint = transform.position;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}
