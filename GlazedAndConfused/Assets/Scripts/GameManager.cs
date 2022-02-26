using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // My obstacles
    public GameObject[] obstacles;

    // Flagged obstacles for deletion <- change to object pooling at some point
    public List<GameObject> flaggedObstacles;

    [Header("Obstacle Settings")]
    public int whenDelete;
    public float offset = 0f;
    public float obstLength = 2.4f;
    public int numberOfObstacles;
    float obstSpeed = 2.0f;
    public float obstSpeedNormal = 2.0f;
    public float obstSpeedFast = 4.0f;
    public float obstSpeedSlow = 1.0f;

    [Header("Tutorial Platform at 0?")]
    public bool tutorialStartOnly;

    [Header("References")]
    public GameOver gameOverScreen;
    public Transform playerPos;
    public PlayerUI scores;

    // Properties
    public float ObstacleSpeed
    {
        get { return obstSpeed; } set { obstSpeed = value; }
    }

    void Start()
    {
        // Spawn first set of tiles
        for (int i = 0; i <= numberOfObstacles; i++)
        {
            if (i == 0)
                SpawnFirstObstacles(0);
            if (tutorialStartOnly) SpawnFirstObstacles(Random.Range(1, obstacles.Length));
            else SpawnFirstObstacles(Random.Range(0, obstacles.Length));
        }
    }

    void Update()
    {
        // check obstacles for deletion
        check_deleteObstacles();
    }

    private void FixedUpdate()
    {
        MoveObstacles();
    }

    // Checking flagged obstacles to DeleteObstacle() when cond. met
    void check_deleteObstacles()
    {
        // when first flagged tile goes behind player, delete it and spawn new one 
        if (flaggedObstacles[0].transform.position.x <= playerPos.position.x - obstLength * whenDelete)
        {
            DeleteObstacle();
            if (tutorialStartOnly) SpawnObstacle(Random.Range(1, obstacles.Length));
            else SpawnObstacle(Random.Range(0, obstacles.Length));
        }
    }

    // Move obstacles
    public void MoveObstacles()
    {
        // Obstacle Manager moves each obstacle based on a speed.
        foreach (GameObject obst in flaggedObstacles)
        {
            Rigidbody rb = obst.GetComponent<Rigidbody>();
            // Tell it to move...
            Vector3 pos = rb.position;
            pos.x -= obstSpeed * Time.deltaTime;
            rb.MovePosition(pos);
        }
    }
    
    // Spawn first set of obstacles
    public void SpawnFirstObstacles(int index)
    {
        GameObject obst = Instantiate(obstacles[index], transform.right * offset, transform.rotation);
        flaggedObstacles.Add(obst);
        offset += obstLength;
    }

    // Continually spawn obstacles
    public void SpawnObstacle(int index)
    {
        // Create, then flag
        GameObject obst = Instantiate(obstacles[index], flaggedObstacles[flaggedObstacles.Count - 1].transform.position + new Vector3(obstLength, 0, 0), transform.rotation);
        flaggedObstacles.Add(obst);
    }

    // Delete Obstacle
    private void DeleteObstacle()
    {
        Destroy(flaggedObstacles[0]);
        flaggedObstacles.RemoveAt(0);
    }

    // functions accessed anywhere
    // Change object approach speed
    public void ChangeSpeed(float speed)
    {
        if (speed == 1) ObstacleSpeed = obstSpeedFast;
        else if (speed == -1) ObstacleSpeed = obstSpeedSlow;
        else ObstacleSpeed = obstSpeedNormal;
    }

    // Tell Game Over Screen to reveal and insert values from UI scores into it
    public void Lose()
    {
        // Freeze time, then ask UI to show itself with relevant information
        Time.timeScale = 0;
        gameOverScreen.Setup(scores.FinalScore, scores.FinalTime);
    }

    public void ChangeScore(int value)
    {
        scores.Points += value;
    }
}
