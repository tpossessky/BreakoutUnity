using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BallPhysics : MonoBehaviour {
    [SerializeField] private GameObject ball;

    public float initialSpeed = 10f;
    private float speed;
    public float speedIncrement = 1f;
    private bool isMoving;

    private readonly Vector3 startingPosition = new(25, 0, 0);
    private Vector3 direction = new Vector3(1, 0, 2).normalized;
    private const float minX = -0.5f;
    private const float maxX = 50.5f;
    private const float minZ = -12f;
    private const float maxZ = 12f;
    public event Action OnPlayerMiss;
    public event Action OnPlayerScored;

    // Start is called before the first frame update
    void Start() {
        ball.transform.position = startingPosition;
        setRandDir();
        speed = initialSpeed;
    }

    void setRandDir() {
        var randomX = UnityEngine.Random.Range(-3f, 3f);
        var randomZ = UnityEngine.Random.Range(-3f, 3f);
        direction = new Vector3(randomX, 0, randomZ).normalized;
    }

    public void StartMove() {
        if (isMoving)
            return;

        speed = initialSpeed;
        setRandDir();
        isMoving = true;
    }


    // Update is called once per frame
    void Update() {
        if (!isMoving)
            return;

        // Move the ball
        ball.transform.Translate(direction * (speed * Time.deltaTime));

        // Ensure Y position stays at 0
        Vector3 position = ball.transform.position;
        ball.transform.position = new Vector3(position.x, 0, position.z);

        // Boundary checks
        if (position.x < minX || position.x > maxX) {
            direction = new Vector3(-direction.x, 0, direction.z).normalized;
            ball.transform.position = new Vector3(Mathf.Clamp(position.x, minX, maxX), 0, position.z);
        }
        if (position.z < minZ || position.z > maxZ) {
            direction = new Vector3(direction.x, 0, -direction.z).normalized;
            ball.transform.position = new Vector3(position.x, 0, Mathf.Clamp(position.z, minZ, maxZ));
        }
    }

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Player")) {
            collidePlayer(col);
        }
        else if (col.gameObject.CompareTag("Block") || col.gameObject.CompareTag("Wall") ||
                 col.gameObject.CompareTag("SideWall")) {
            if (col.gameObject.CompareTag("Block")) {
                collideWall(false);
                OnPlayerScored?.Invoke();
            }
            else if (col.gameObject.CompareTag("Wall")) {
                collideWall(false);
            }
            else {
                collideWall(true);

            }
        }
        else if (col.gameObject.CompareTag("PlayerNet")) {
            playerMissed();
        }
    }

    private void OnCollisionExit(Collision col) {
        if (col.gameObject.CompareTag("Block")) {
            Destroy(col.gameObject);
        }
    }


    void collidePlayer(Collision player) {
        var playerZLoc = player.gameObject.transform.position.z;
        var ballZLoc = ball.gameObject.transform.position.z;

        //value between 2.5 and -2.5
        var normalizedCollisionLocation = Math.Round(ballZLoc - playerZLoc, 1);
        var angle = normalizedCollisionLocation * 18.0f;

        direction = new Vector3(direction.x, 0, Mathf.Tan((float)angle * Mathf.Deg2Rad)).normalized;
        speed += speedIncrement;
    }

    void collideWall(bool isSideWall) {
        direction = !isSideWall ? 
            new Vector3(direction.x, 0, -direction.z).normalized : 
            new Vector3(-direction.x, 0, direction.z).normalized;
    }
    
    

    private void playerMissed() {
        ball.transform.position = startingPosition;
        isMoving = false;
        direction = Vector3.zero;

        OnPlayerMiss?.Invoke();
    }
}