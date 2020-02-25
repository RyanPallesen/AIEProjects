using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    public float speed = 5;
    public float mouseSensitivity = 1;


    public float distanceToGround = 0f;
    public bool isGrounded = false;
    public bool mantleObjectExists = false;
    public bool mantleEmptySpaceExists = false;
    public bool CanVault => (mantleObjectExists && mantleEmptySpaceExists);

    private Rigidbody rb;
    private new Camera camera;

    public new Transform transform;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        Vector2 movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        ResolveMovement(movement);

        Vector2 mouseMovement;
        mouseMovement.x = Input.GetAxisRaw("Mouse X");
        mouseMovement.y = Input.GetAxisRaw("Mouse Y");
        ResolveCamera(mouseMovement);

        ResolveIsGrounded();
        ResolveMantleObjectExists();
        ResolveMantleEmptySpaceExists();

        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                Vector3 force = new Vector3(0, 5 - rb.velocity.y, 0);
                rb.AddForce(force, ForceMode.Impulse);
                isGrounded = false;
            }
            else if (CanVault)
            {
                rb.velocity = Vector3.zero;
                Vector3 force = new Vector3(0, 5, 0);
                rb.AddForce(force, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    private void ResolveMantleObjectExists()
    {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out RaycastHit hit, 1f))
        {
            mantleObjectExists = true;
        }
        else
        {
            mantleObjectExists = false;
        }
    }

    private void ResolveMantleEmptySpaceExists()
    {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2), transform.position.z), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out RaycastHit hit, 1f))
        {
            mantleEmptySpaceExists = false;
        }
        else
        {
            mantleEmptySpaceExists = true;
        }

    }

    private void ResolveMovement(Vector2 v2)
    {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction);
        bool rayHit = Physics.Raycast(ray, 0.7f);

        if (!rayHit && !isGrounded)
        {
            ray = new Ray(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 4), transform.position.z), transform.forward);
            Debug.DrawRay(ray.origin, ray.direction);

            rayHit = Physics.Raycast(ray, 0.7f);
        }
        if (!rayHit && !isGrounded)
        {
            ray = new Ray(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2.1f), transform.position.z), transform.forward);
            Debug.DrawRay(ray.origin, ray.direction);

            rayHit = Physics.Raycast(ray, 0.7f);
        }

        if (rayHit)
        {
            v2.y = -0.05f;
        }


        if (isGrounded)
        {
            rb.MovePosition(transform.position + (((v2.y * transform.forward) + (v2.x * transform.right)) * Time.deltaTime * speed));
        }
        else
        {
            rb.MovePosition(transform.position + ((((v2.y * transform.forward) + (v2.x * transform.right)) * Time.deltaTime * speed)));
        }



    }

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private void ResolveCamera(Vector2 movement)
    {
        yaw += movement.x * mouseSensitivity;
        pitch -= movement.y * mouseSensitivity;

        if (pitch > 80)
        {
            pitch = 80;
        }
        else if (pitch < -80)
        {
            pitch = -80;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yaw, transform.eulerAngles.z);
        camera.transform.eulerAngles = new Vector3(pitch, camera.transform.eulerAngles.y, camera.transform.eulerAngles.z);
    }

    private void ResolveIsGrounded()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        RaycastHit hit;
        distanceToGround = 128f;

        ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), -transform.up);
        if (Physics.SphereCast(ray, 0.5f, out hit))
        {
            Debug.DrawRay(ray.origin, ray.direction);
            if(Vector3.Distance(transform.position, hit.point) < distanceToGround)
            {
                distanceToGround = Vector3.Distance(transform.position, hit.point);
            }
            distanceToGround -= transform.localScale.y/2;
        }

        if (distanceToGround <= 0.6f)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

}
