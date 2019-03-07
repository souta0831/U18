using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //ステータス
    public float Speed = 3.0f;
    public float JumpPower = 6.0f;

    //リジットボディ
    Rigidbody rb;

    //カメラ
    public Camera Camera;

    //プレイヤーの状態
    enum PlayerState
    {
        Ground,
        Air,
        Glider
    }
    PlayerState State = PlayerState.Ground;

    [SerializeField] private GameObject Pivot = null;

    float rote_y;



    //bool FlyFlag = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //ステート別操作
        switch (State)
        {
            case PlayerState.Ground:
                Move_Grounding();
                break;

            case PlayerState.Air:
                Move_Air();
                break;

            case PlayerState.Glider:
                Move_Glider();
                break;
        }
    }

    //地上時の移動
    void Move_Grounding()
    {
        rote_y = transform.localEulerAngles.y;//角度

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        Vector3 camForward = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = camForward * z + Camera.transform.right * x;
        transform.position += moveForward * Speed * Time.deltaTime;

        if (moveForward.sqrMagnitude > 0.0f) transform.rotation = Quaternion.LookRotation(moveForward);

        //ジャンプ
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(0.0f, JumpPower, 0.0f, ForceMode.Impulse);
            State = PlayerState.Air;
        }

    }

    //空中時の移動
    void Move_Air()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        Vector3 camForward = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = camForward * z + Camera.transform.right * x;
        transform.position += moveForward * Speed * Time.deltaTime;

        if (moveForward.sqrMagnitude > 0.0f) transform.rotation = Quaternion.LookRotation(moveForward);

        //滑空
        if (Input.GetButtonDown("Jump")) State = PlayerState.Glider;

        //落下中の処理
        if (IsFall())
        {
            //着地判定
            if (IsGround())
            {
                State = PlayerState.Ground;
                rb.velocity = Vector3.zero;
            }

        }
    }

    //滑空時の移動
    void Move_Glider()
    {
        var x = -Input.GetAxis("Vertical");
        var z = -Input.GetAxis("Horizontal");

        //落下中の処理
        if (IsFall())
        {
            rb.velocity += transform.up * 5.0f * Time.deltaTime;//進行方向の力
            rb.velocity += Vector3.up * 8.0f * Time.deltaTime;//重力の力
            transform.rotation = Quaternion.Euler(new Vector3(90 + x * 30, rote_y, z * 50));

            //解除
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = Vector3.zero;
                State = PlayerState.Air;
            }
        }
    }

    //地上にいるかどうか判定
    bool IsGround()
    {
        //ジャンプ中はfalse
        if (rb.velocity.y > 0f) return false;

        //地面判定用のレイ
        Ray down_ray;
        RaycastHit rayhit;
        down_ray = new Ray(Pivot.transform.position, Vector3.down);
        Debug.DrawRay(down_ray.origin, down_ray.direction * 1.0f, Color.red, 30.0f, false);

        return Physics.Raycast(down_ray, out rayhit, 1.0f);
    }

    //落下中か判定
    bool IsFall()
    {
        return rb.velocity.y < 0f;
    }

}
