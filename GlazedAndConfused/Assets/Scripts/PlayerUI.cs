using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // References
    public Text scoreText;
    public Text timeText;
    Player player;
    public Image karma;

    //Karma 
    float playerKarma;
    float playerMaxKarma;

    // Elapsed Time variables
    private float elapsedTime;
    private int minutes;

    // Score increment
    private float tickTimer;
    public float timeToTick;
    public int pointsIncrement = 10;
    private int points;

    // String variables formatted for Text GameObjects
    private string time;
    private string score;


    // PROPERTIES 
    public string FinalTime { get { return time; }}
    public string FinalScore { get { return score; }} 
    public int Points { get { return points; } set { points = value; } }



    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerMaxKarma = player.maxKarma;
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;
        elapsedTime += Time.deltaTime;
        playerKarma = player.Karma;

        if (elapsedTime >= 60)
        {
            elapsedTime = 0;
            minutes += 1;
        }

        if (tickTimer >= timeToTick)
        {
            tickTimer = 0;
            points += pointsIncrement;
        }

        // Construct time string
        time = string.Format("TIME: {0:00}:{1:00}", minutes, elapsedTime);
        timeText.text = time;

        // Construct score string
        score = string.Format("SCORE: {0}", points);
        scoreText.text = score;

        // UPDATE KARMA BAR WITH PLAYER STAT... 
        karma.fillAmount = playerKarma / playerMaxKarma;
    }
}
