using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BlueAgent : MonoBehaviour
{

    public GameObject redAgent;
    private NavMeshAgent blueAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        blueAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //El enemigo persigue siempre al player
        //blueAgent.SetDestination(redAgent.transform.position);
        
        //El enemigo persigue al player a una distancia determinada
        /*Vector3 direction = redAgent.transform.position - transform.position;

        if (direction.magnitude < 10)
        {
            blueAgent.isStopped = false;
            blueAgent.SetDestination(redAgent.transform.position);
        }
        else
        {
            blueAgent.isStopped = true;
        }*/
        
        //El enemigo persigue al player si esta dentro de su rango de vision
        Vector3 direction = redAgent.transform.position - transform.position;
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 10))
        {
            if (hit.transform.gameObject.CompareTag("ai"))
            {
                if (Vector3.Angle(transform.forward, direction) < 60)
                { 
                    blueAgent.isStopped = false;
                    blueAgent.SetDestination(redAgent.transform.position);
                }else
                {
                    blueAgent.isStopped = true;
                }
            }else
            {
                blueAgent.isStopped = true;
            }
        }else
        {
            blueAgent.isStopped = true;
        }

    }
}
