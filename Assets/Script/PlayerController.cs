using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 3.0f;//速度
    public float JumpPower = 6.0f;//ジャンプパワー

    Rigidbody rb;
    Ray ray;
    RaycastHit hit;
    bool isGround;
    public Camera Camera;
    [SerializeField] float ray_distance = 5;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        RayChack();//レイを管理するやつ
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            Vector3 camForward = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = camForward * z + Camera.transform.right * x;
            transform.position += moveForward * Speed * Time.deltaTime;

            if (moveForward.sqrMagnitude > 0.0f) transform.rotation = Quaternion.LookRotation(moveForward);

        if (isGround)
        {
            if (Input.GetButtonDown("Jump")) rb.AddForce(0.0f, JumpPower, 0.0f, ForceMode.Impulse);
        }

    }
    void RayChack()
    {
        ray= new Ray(transform.position,Vector3.down);
        if (Physics.Raycast(ray, out hit, ray_distance))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }


        Debug.DrawLine(ray.origin, ray.direction * ray_distance, Color.red);//レイを可視化するやつ
        Debug.Log(hit.collider.gameObject.name);

    }

}