using UnityEngine;
using StarterAssets; // Assure-toi que tu as bien ce namespace

public class SpeedManager : MonoBehaviour
{
    private ThirdPersonController playerController;
    private StarterAssetsInputs input; // Référence aux inputs du joueur

    public float minSpeed = 2.0f;
    public float maxSpeed = 10.0f;
    public float acceleration = 5.0f;
    public float deceleration = 8.0f;

    private void Start()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<ThirdPersonController>();
        }

        if (input == null)
        {
            input = FindObjectOfType<StarterAssetsInputs>();
        }

        if (playerController != null)
        {
            playerController.MoveSpeed = minSpeed;
        }
    }

    private void Update()
    {
        if (playerController == null || input == null) return;

        // Vérifie si le joueur appuie sur une touche de déplacement
        bool isMoving = Mathf.Abs(input.move.x) > 0.1f || Mathf.Abs(input.move.y) > 0.1f;

        if (isMoving)
        {
            playerController.MoveSpeed = Mathf.Min(playerController.MoveSpeed + acceleration * Time.deltaTime, maxSpeed);
        }
        else
        {
            playerController.MoveSpeed = Mathf.Max(playerController.MoveSpeed - deceleration * Time.deltaTime, minSpeed);
        }
    }
}