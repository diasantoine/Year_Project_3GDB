using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : MonoBehaviour
{

    [SerializeField] private float fieldOfView;
    [SerializeField] private float distanceOfView;
    [SerializeField] private float disToGround;
    [SerializeField] private protected Rigidbody RB;
    [SerializeField] private protected Animator AnimatorConteneur;

    public NavMeshAgent agent;
    public ennemyState health;
    public Transform player;

    private protected bool JustHit;
    private protected bool Grounded;
    private protected bool SeeThePlayer;

    public void VisionCone(Transform player)
    {
        var rayDirection = player.transform.position - transform.position;
        if (Vector3.Angle(rayDirection, transform.forward) < fieldOfView
            && Vector3.Distance(transform.position, player.transform.position) < distanceOfView 
            && player.GetComponent<CharacterMovement>().Grounded)
        {
            // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Player")))
            {
                SeeThePlayer = true;               
                Debug.DrawRay(new Vector3(transform.position.x, 1, transform.position.z), player.position - transform.position, Color.blue);

            }

        }
        else
        {
            SeeThePlayer = false;
            //Debug.Log(SeeThePlayer);
        }
    }

    public void Ground(RaycastHit hit)
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, disToGround, LayerMask.GetMask("Sol", "Wall")))
        {
            if (hit.collider.CompareTag("sol") || hit.collider.CompareTag("Mur"))
            {

                Grounded = true;
                RB.constraints = RigidbodyConstraints.FreezeRotation;
                RB.constraints = RigidbodyConstraints.FreezePositionY;
                //Debug.Log(Grounded);
            }
        }
        else
        {
            Grounded = false;
            agent.enabled = false;
            RB.constraints = RigidbodyConstraints.None;
            //Debug.Log(Grounded);

        }
    }

}
