using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public Animator anim;

    public float speed = 12f;
    public float gravity = -9.18f;
    Vector3 velocity;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if(x != 0 || z != 0)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Repare", false);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        if(Input.GetAxis("Fire1") == 1)
        {
            anim.SetBool("Repare", true);
        }

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
