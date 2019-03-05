using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 3.0f;
    public float JumpPower = 6.0f;

    Rigidbody rb;
    public Camera Camera;

    bool AGBuffer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        Vector3 camForward = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = camForward * z + Camera.transform.right * x;
        transform.position += moveForward * Speed * Time.deltaTime;

        if (moveForward.sqrMagnitude > 0.0f) transform.rotation = Quaternion.LookRotation(moveForward);

        if (Input.GetButtonDown("Jump")) rb.AddForce(0.0f, JumpPower, 0.0f, ForceMode.Impulse);

       // if (AGBuffer != IsGround())
       // {
            if (IsGround())
            {
            rb.velocity = Vector3.zero;
            Debug.Log("地上にいますわ!");
            }
            else
            {
                Debug.Log("空中にいますわ!");
                //落下中なら滑空 
                if (IsFall())
                {
                rb.velocity += transform.forward * 12.0f * Time.deltaTime;
                rb.velocity += Vector3.up * 8.0f * Time.deltaTime;
               }
            }
      //  }
       // AGBuffer = IsGround();

    }

    //地上時の移動
    void Move_Grounding()
    {

    }

    //空中時の移動
    void Move_Frying()
    {

    }

    //地上にいるかどうか判定
    bool IsGround()
    { 
       return rb.velocity.y == 0.0f;
    }

    //落下中か判定
    bool IsFall()
    {
        return rb.velocity.y < 0;
    }

}
