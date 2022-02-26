using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Reference to Text objects to modify
    public Text scoreText;
    public Text timeText;

    // Function to instantiate it, will take SCORE as a param later
    public void Setup(string score, string time)
    {
        scoreText.text = "YOUR SCORE: " + "\n" + score;
        timeText.text = "TIME LASTED: " + "\n" + time;
        // Add another text object and assign its value to be final score
        gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
