using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject startButtonRoot;

    private GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();

        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
            return;
        }

        if (scoreText != null)
            scoreText.text = $"{gm.LeftScore} - {gm.RightScore}";

        if (startButtonRoot != null)
            startButtonRoot.SetActive(!gm.GameStarted);

        if (winText != null)
        {
            if (gm.GameOver)
            {
                winText.gameObject.SetActive(true);
                winText.text = (gm.Winner == 0) ? "Right Player Wins!" : "Left Player Wins!";
            }
            else
            {
                winText.gameObject.SetActive(false);
            }
        }
    }
}