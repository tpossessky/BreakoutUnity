using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {

    [SerializeField] private BallPhysics ball;

    [SerializeField] private UIController uiController;

    private int curHealth = 3;
    private int curScore;
    private bool isGameOver;
    
    // Start is called before the first frame update
    void Start() {
        ball.OnPlayerMiss += HandlePlayerMiss;
        ball.OnPlayerScored += HandlePlayerScored;
    }

    public void StartBall(InputAction.CallbackContext context) {
        if (context.performed) {
            if (isGameOver) {
                uiController.ResetGame();
            }
            ball.StartMove();
        }
    }


    private void HandlePlayerMiss() {
        curHealth--;
        if (curHealth < 0) {
            uiController.ShowGameOver();
            isGameOver = true;
        }
        else {
            uiController.RemoveHeart(curHealth);
        }
    }

    private void HandlePlayerScored() {
        curScore++;
        uiController.SetScore(curScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
