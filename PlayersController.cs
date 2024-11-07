using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayersController : MonoBehaviour
{
    [SerializeField] private GameObject goplayer;
    private CharacterController player;

    [SerializeField] private float speed = 1f;

    private Vector3 playerDirection;

    private const float minX = 2f;    // Minimum X bound
    private const float maxX = 48f;   // Maximum X bound
    private const float fixedZ = -11f; // Fixed Z position

    void Start()
    {
        playerDirection = Vector3.zero;
    }

    private void Awake()
    {
        player = goplayer.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get current X position
        var playerPosX = goplayer.transform.position.x;

        // Check bounds for player position on X-axis
        var newPlayerPosX = playerPosX + (playerDirection.x * speed * Time.deltaTime);
        if (newPlayerPosX < minX || newPlayerPosX > maxX)
        {
            Debug.Log("Outside bounds");
            // Stop player from moving beyond bounds
            playerDirection.x = 0;
        }

        // Move player in X direction and reset Z position
        if (player.enabled)
        {
            player.Move(playerDirection * (speed * Time.deltaTime));
            // Reset Z position to ensure it remains at -11
            goplayer.transform.position = new Vector3(goplayer.transform.position.x, goplayer.transform.position.y, fixedZ);
        }
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>().x;
        playerDirection = new Vector3(input, 0f, 0f); // Movement only on X-axis
    }

    public void ResetPosition()
    {
        player.enabled = false;
        player.transform.position = new Vector3(0, 0, fixedZ); // Start with Z at -11
    }

    public void StartRound()
    {
        player.enabled = true;
    }

}
