using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {
    public Tank playerTank, aiTank;
    public Text playerScoreText, aiScoreText;

    void Update() {
        playerScoreText.text = "Player Score: " + playerTank.score.ToString();
        aiScoreText.text = "AI Score: " + aiTank.score.ToString();
    }
}
