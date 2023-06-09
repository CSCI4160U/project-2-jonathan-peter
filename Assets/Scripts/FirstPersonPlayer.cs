using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonPlayer : MonoBehaviour {
    [Header("General")]
    [SerializeField] private float movementSpeed = 15f;
    [SerializeField] private CharacterController character;
    [SerializeField] private Transform chartrans;
    [SerializeField] Transform gun;
    [SerializeField] GameObject bullet;
    [SerializeField] float shotSpeed;
    [SerializeField] float firerate = 100;

    [Header("Falling")]
    [SerializeField] private float gravityFactor = 1f;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private LayerMask groundLayers;
    [Header("Jumping")]
    [SerializeField] private bool canAirControl = true;
    [SerializeField] private float jumpSpeed = 7f;
    [Header("Looking")]
    [SerializeField] private float mouseSensitivity = 1000f;
    [SerializeField] private Transform camera;
    private CharacterController controller;
    private float verticalRotation = 0f;
    private float verticalSpeed = 0f;
    private bool isGrounded = false;
    private bool crouching = false;
    private void Awake() {
        controller = GetComponent<CharacterController>();
    }
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        verticalRotation = 0f;
    }
    void Update() {
        // are we on the ground?
        RaycastHit collision;
        if (Physics.Raycast(groundPosition.position, Vector3.down, out
            collision, 0.5f, groundLayers)) {
            isGrounded = true;
        } 
        else {
            isGrounded = false;
        }
        // update vertical speed
        if (!isGrounded) {
            verticalSpeed += gravityFactor * -9.81f * Time.deltaTime;
        } 
        else {
            verticalSpeed = 0f;
        }
        // adjust rotations based on mouse position
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity *
        Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity *
        Time.deltaTime;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        camera.localEulerAngles = new Vector3(verticalRotation, 0f,
        0f);
        Vector3 x = Vector3.zero;
        Vector3 y = Vector3.zero;
        Vector3 z = Vector3.zero;
        // handle jumping
        if (isGrounded && Input.GetButtonDown("Jump")) {
            verticalSpeed = jumpSpeed;
            isGrounded = false;
            y = transform.up * verticalSpeed;
        } 
        else if (!isGrounded) {
            y = transform.up * verticalSpeed;
        }
        //handle crouching
        if(Input.GetKeyDown(KeyCode.C)){
            if(crouching){
                
                character.height += 1.8f;
                camera.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                crouching = false;
            }
            else{
                character.height -= 1.8f;  
                camera.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                crouching = true;
            }
        }
        if(Input.GetMouseButtonDown(0) && firerate>=100){
            shoot();
            firerate = 0;
        }

        // handle movement
        if (isGrounded || canAirControl) {
            x = transform.right * Input.GetAxis("Horizontal") *
            movementSpeed;
            z = transform.forward * Input.GetAxis("Vertical") *
            movementSpeed;
        }
        Vector3 movement = x + y + z;
        movement *= Time.deltaTime;
        controller.Move(movement);
    }
    void shoot(){
        GameObject bulletClone = Instantiate(bullet, gun.position, transform.rotation);
        
        bulletClone.GetComponent<Rigidbody>().AddForce(camera.transform.forward * shotSpeed);
        Destroy(bulletClone,10);
    }
    private void FixedUpdate() {
        firerate++;    
    }
}