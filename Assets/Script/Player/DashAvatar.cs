using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAvatar : MonoBehaviour
{
    [SerializeField] private detectDead detectD;
    [SerializeField] private Rigidbody ConteneurRigibody;
    [SerializeField] private float DashSpeed = 20;
    private Vector3 HitPosition;

    RaycastHit floorHit;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire3") /*&& detectD.deadList.Count>=7*/)
        {
            Ray MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(MousePosition, out RaycastHit hit,Mathf.Infinity,LayerMask.GetMask("Sol")))
            {
                if (hit.transform.tag != "Collider")
                {
                    Vector3 HitPoint = hit.point;
                    HitPosition = HitPoint;
                    Vector3 playerToMouse = HitPoint - transform.parent.position;
                    playerToMouse.y = 0;
                    playerToMouse = playerToMouse.normalized;
                    transform.parent.GetComponent<CharacterMovement>().OnDash = true;
                    transform.gameObject.layer = 12;
                    transform.parent.GetComponent<CapsuleCollider>().enabled = enabled;
                    transform.parent.tag = "Player";
                    //Vector3 Dir = (hit.transform.position - transform.parent.position).normalized;
                
                    ConteneurRigibody.velocity = playerToMouse*DashSpeed;
                    //ConteneurRigibody.AddForce(playerToMouse*DashSpeed, ForceMode.Impulse);
                }
            }
            /*if(detectD.deadList.Count > 0)
            {
            
                for (int i = 0; i < 5; i++)
                {
                    takeCadavre TC = detectD.deadList[i].GetComponent<takeCadavre>();
                    if(TC.isMunitions)
                    {
                        TC.isMunitions = false;
                    }
                    detectD.deadList.Remove(detectD.deadList[i]);
                    Destroy(TC.transform.gameObject);
                }
            }*/
        }

        if (transform.parent.GetComponent<CharacterMovement>().OnDash)
        {
            if (transform.parent.GetComponent<Rigidbody>().velocity.magnitude < 3)
            {
                transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.parent.GetComponent<CharacterMovement>().OnDash = false;
                transform.parent.GetComponent<CapsuleCollider>().enabled = !enabled;
                transform.gameObject.layer = 9;
                transform.parent.tag = "Untagged";
            }
            Debug.Log(Vector3.Distance(HitPosition,transform.position));
            if (Vector3.Distance(HitPosition,transform.position)<2)
            {
                transform.parent.GetComponent<CharacterMovement>().OnDash = false;
                transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.parent.GetComponent<CapsuleCollider>().enabled = !enabled;
                transform.gameObject.layer = 9;
                transform.parent.tag = "Untagged";
            }
        }
    }
}
