using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject deathCanvas;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(player);
            deathCanvas.SetActive(true);
            Invoke("ResetScene", 2f);
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(0);
    }

}
