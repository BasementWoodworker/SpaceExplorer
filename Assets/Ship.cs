using UnityEngine;
using UnityEngine.InputSystem;

public class Ship : MonoBehaviour
{
    private GameObject shipGO;
    private float maxForwardSpeed = 10;
    private float maxReverseSpeed = 2;
    private float _thrust = 0;
    private float thrust
    {
        get { return _thrust; }
        set
        {
            _thrust = Mathf.Clamp(value, -maxReverseSpeed, maxForwardSpeed);
        }
    }
    private float acceleration = 10f;
    private float brakingStrength = 20f;
    private float speedDecay = 5f;
    private float turnSpeed = 0.5f;
    private InputAction moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipGO = this.gameObject;
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    // Called once per frame
    void HandleMovement()
    {
        Vector2 moveVal = moveAction.ReadValue<Vector2>();
        // Movement
        if ((thrust >= 0 && moveVal.y > 0) || (thrust < 0 && moveVal.y < 0))
        {
            // Accelerating
            thrust += Time.deltaTime * moveVal.y * acceleration / 1f;
        } else if ((thrust >= 0 && moveVal.y < 0) || (thrust < 0 && moveVal.y > 0))
        {
            // Braking
            thrust += Time.deltaTime * moveVal.y * brakingStrength / 1f;
        } else if (thrust != 0f)
        {
            // Natural speed decay
            thrust = thrust >= 0 ? Mathf.Max(0, thrust - Time.deltaTime * speedDecay) : Mathf.Min(0, thrust + Time.deltaTime * speedDecay);
        }
        transform.Translate(new Vector3(0, thrust * Time.deltaTime, 0));

        // Rotation
        if (moveVal.y < 0)
        {
            transform.Rotate(0, 0, moveVal.x * turnSpeed);
        }
        else
        {
            transform.Rotate(0, 0, -moveVal.x * turnSpeed);
        }
    }
}
