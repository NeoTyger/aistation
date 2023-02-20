using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BlueAgent : MonoBehaviour
{

    public GameObject redAgent;
    private NavMeshAgent blueAgent;
    private int puntActual = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        blueAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
         Patrullar();
         // Crear alguna condicion para pasar de patrullar a perseguir y etc.
    }

    private void Perseguir()
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

    private void Patrullar()
    {
        Vector3[] listaPuntos = new Vector3[2];
        listaPuntos[0] = new Vector3(18f,0f,7f);
        listaPuntos[1] = new Vector3(0f,0f,13f);
        if (Vector3.Distance(transform.position,  listaPuntos[puntActual]) < 2)
        {
            puntActual += 1;
        }

        if (puntActual == listaPuntos.Length)
        {
            puntActual = 0;
        }

        blueAgent.SetDestination(listaPuntos[puntActual]);
    }

    private void Fugir()
    {
        blueAgent.SetDestination(2 * transform.position - redAgent.transform.position);
    }

    private void Amagar()
    {
        //fletxa = obstacle - redAgent
        //posicioFinal= obstacle + fletxa.normalize * 2
    }
}
