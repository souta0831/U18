using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Star  private CharacterController controller;
    private CharacterController controller;
    private Vector3 velocity;//速度

    private Rigidbody rigidBody;
    public Rigidbody parentRigidBody;　//本体
    private float glindStartedY;
    private Vector3 glindStartedVelocity;
    private RaycastHit hit;
    private Ray ray;


    [SerializeField] private float MoveSpeed = 3.0f;//移動速度
    [SerializeField] private float RotateSpeed = 20.0f;//回転速度
    [SerializeField] private float JumpPower;//ジャンプパワー
    [SerializeField] private float Grabity = 20.0f;//重力速度

    //　レイを飛ばす位置
    [SerializeField] private Transform rayPosition;
    //　レイの距離
    [SerializeField] private float rayRange = 0.85f;
    //　レイが地面に到達しているかどうか
    private bool isGround = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }
    void PlayerMove()//動き系を統括してるやつ
    {
        ray = new Ray(transform.position, transform.forward);

        RayCheck();
        if (isGround)
        {

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y += JumpPower;
            }

            //　地面に接地してる時は速度を初期化
            if (controller.isGrounded)
            {
                velocity = Vector3.zero;
                //　レイを飛ばして接地確認の場合は重力だけは働かせておく、前後左右は初期化
            }
            else
            {
                velocity = new Vector3(0f, velocity.y, 0f);
            }

            var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if (input.magnitude > 0f)
            {
                transform.LookAt(transform.position + input);
                velocity += input * MoveSpeed;
            }
            //transform.rotation = Quaternion.AngleAxis(0, new Vector3(1, 0, 0));

        }

        velocity.y += Physics.gravity.y * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    //地面に当たってるか調べるやつ
    void RayCheck()
    {

        if (Physics.Raycast(ray, out hit, 10.0f))
        {

            isGround = true;


        }
        else
        {
            isGround = false;

        }
            
    }
    //滑空してるときの処理
    void Flait()
    {

        if (transform.localEulerAngles.x < 45)
        {
            transform.Rotate(new Vector3(1, 0, 0),3);
        }
    }
}
