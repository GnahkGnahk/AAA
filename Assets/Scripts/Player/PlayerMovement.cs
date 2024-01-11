using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbodyPlayer;
    [SerializeField] float force, speed;

    void Start()
    {
        
    }

    Vector3 moveDir;

    // Update is called once per frame
    void Update()
    {
        MoveController();
        RotationController();
    }

    void RotationController()
    {
        //gameObject.transform.forward = moveDir;
    }

    void MoveController()
    {
        moveDir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbodyPlayer.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDir = Vector3.forward;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDir = Vector3.back;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDir = Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDir = Vector3.right;
        }
        rigidbodyPlayer.AddForce(speed * Time.deltaTime * moveDir, ForceMode.Impulse);

    }
}