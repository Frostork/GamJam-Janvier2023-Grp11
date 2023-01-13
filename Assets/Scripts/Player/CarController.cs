using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float movementSpeed = 15;
    public float boostBonus = 1;
    public Vector3 rotationSpeed = new Vector3(0,40,0);
    private Rigidbody rb;
    private Vector2 inputDirection;
 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
 
    private void Update()
    {
        //Vector2 inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //inputDirection = inputs.normalized;
    }
 
    private void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(inputDirection.x * rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
        rb.MovePosition(rb.position + transform.forward * movementSpeed * boostBonus * inputDirection.y * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputs = context.ReadValue<Vector2>();
        inputDirection = inputs.normalized;
    }
}