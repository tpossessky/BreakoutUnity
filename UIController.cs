using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Image[] hearts;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI gameOver;
    
    private static string scoreText = "Score: ";


    private void Awake() {
        gameOver.enabled = false;
    }

    public void RemoveHeart(int index) {

        hearts[index].enabled = false;
    }

    public void ResetGame() {
        hearts[0].enabled = true;
        hearts[1].enabled = true;
        hearts[2].enabled = true;
        gameOver.enabled = false;
        ResetScore();
    }

    public void SetScore(int newScore) {
        score.text = scoreText + newScore;
    }

    public void ResetScore() {
        score.text = scoreText + "0";
    }

    public void ShowGameOver() {
        gameOver.enabled = true;
    }
    
}
