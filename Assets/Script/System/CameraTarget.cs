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
        Ray rayon = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayon, out hit, Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
        {
            Vector3 mousePos = hit.point;
            Vector3 targetPos = (player.position + mousePos) / 2f;

            Vector3 playerToMouse = mousePos - player.position;

            playerToMouse = playerToMouse.normalized;

            player.LookAt(playerToMouse * 150);
            Vector3 eulerAngles = player.rotation.eulerAngles;
            eulerAngles = new Vector3(0, eulerAngles.y, 0);
            player.rotation = Quaternion.Euler(eulerAngles);

            targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
            targetPos.z = Mathf.Clamp(targetPos.z, -threshold + player.position.z, threshold + player.position.z);
            targetPos.y = 0;

            transform.position = targetPos;
            //Debug.DrawRay(player.position, targetPos * 100);

        }
    }
}
