using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    // Referencing the obstacleManager so that the collision can trigger game over
    GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameManager.Lose();
        }
    }
}
