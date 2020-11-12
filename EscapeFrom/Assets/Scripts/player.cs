﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    float m_speed;
    [SerializeField]
    Vector3 m_dir;
    public Camera m_camera;
    public Transform m_cameraArm;

    Rigidbody m_rigidbody;
    CapsuleCollider m_collider;
    Animator m_Anim;

    private int m_JumpCount = 0;
    private float m_lookSensitivity = 3f;
    private float m_cameraRotationLimit = 50f;
    private float m_currentCameraRotationX;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();
        m_Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        character_Rotation();
        camera_Rotation();
        Move();

        if (Input.GetMouseButton(0))
        {
            m_Anim.SetBool("ATTACK", true);
        }
        else
        {
            m_Anim.SetBool("ATTACK", false);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            m_Anim.SetBool("PICKUP", true);
        }
        else
        {
            m_Anim.SetBool("PICKUP", false);
        }

        if(m_JumpCount < 1 && Input.GetButtonDown("Jump"))
        {
            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 6, m_rigidbody.velocity.z);
            m_JumpCount++;
        }
        m_Anim.SetFloat("JUMP", m_rigidbody.velocity.y);

        ///////////HP =0 -> Death로 수정
        //if (Input.GetKey(KeyCode.D))
        //{
        //    m_Anim.SetBool("DEATH", true);
        //}
        //else
        //{
        //    m_Anim.SetBool("DEATH", false);
        //}


        /////////////쳐맞으면 HIT 모션 취하는걸로 수정
        //if (Input.GetKey(KeyCode.X))
        //{
        //    m_Anim.SetBool("HIT", true);
        //}
        //else
        //{
        //    m_Anim.SetBool("HIT", false);
        //}

        //else if (Input.GetKey(KeyCode.Space))
        //{
        //    m_Anim.SetTrigger("JUMP");
        //}

        //if (m_dir != Vector3.zero)
        //{
        //    m_Anim.SetBool("WALK", true);
        //  //  m_Anim.transform.forward = m_dir;
        //}                             
        //else
        //{
        //    m_Anim.SetBool("WALK", false);
        //}

    }

    void character_Rotation()
    {
        float YRotation = Input.GetAxisRaw("Mouse X");
        Vector3 charRotationY = new Vector3(0, YRotation, 0) * m_lookSensitivity;
        m_rigidbody.MoveRotation(m_rigidbody.rotation * Quaternion.Euler(charRotationY));
    }

    void camera_Rotation()
    {
        float XRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = XRotation * m_lookSensitivity;
        m_currentCameraRotationX -= cameraRotationX;
        m_currentCameraRotationX = Mathf.Clamp(m_currentCameraRotationX, -m_cameraRotationLimit, m_cameraRotationLimit);

        m_camera.transform.localEulerAngles = new Vector3(m_currentCameraRotationX, 0, 0);
    }

    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        m_Anim.SetBool("WALK", isMove);

        if (isMove)
        {
            Vector3 lookForward = new Vector3(m_cameraArm.forward.x, 0f, m_cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(m_cameraArm.right.x, 0f, m_cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            this.transform.forward = lookForward;
            transform.position += moveDir * Time.deltaTime * m_speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_JumpCount = 0;
        }
    }

}
