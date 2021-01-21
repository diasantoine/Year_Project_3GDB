using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private Transform player;

    [SerializeField] private float threshold;


    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray camRay = cam.ScreenPointToRay((Input.mousePosition));

        RaycastHit FloorHit;

        if (Physics.Raycast(camRay,out FloorHit,Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
        {
            Vector3 playerToMouse = FloorHit.point - transform.position;

            playerToMouse.y = 0;
            
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            
            player.GetComponent<CharacterMovement>().ConteneurRigibody.MoveRotation(newRotation);
            
            Vector3 mousePos = FloorHit.point;
            Vector3 targetPos = (player.position + mousePos) / 2f;

            targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
            targetPos.z = Mathf.Clamp(targetPos.z, -threshold + player.position.z, threshold + player.position.z);
            targetPos.y = 0;

            transform.position = targetPos;
        }
    }
}
