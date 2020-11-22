using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public CharacterController CC;

    //[SerializeField] private Animator anim;

    private bool isWalking;

    public float vitesse = 6f;

    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.position.y<=-6)
        {
            transform.parent.position = new Vector3(30, 1.25f, -15.7f);
        }
       /*if (isWalking)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }*/

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            isWalking = true;
            CC.Move(moveDir.normalized * vitesse * Time.deltaTime);
        }
        else
        {
            isWalking = false;
        }
    }
}
