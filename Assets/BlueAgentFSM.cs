using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BlueAgentFSM : MonoBehaviour {

    // An array of GameObjects to store all the agents
    public GameObject Redagent;
    private NavMeshAgent Blueagent;
    State currentState;
    public float blueLife = 100f; // To store the blue agent life


    void Start() {

        // Grab everything with the 'ai' tag
        Blueagent=this.GetComponent<NavMeshAgent>();
        currentState = new Idle(this.gameObject, Blueagent, Redagent.transform); // Create our first state.
    }

    // Update is called once per frame
    void Update() {
        // Trobam a l'enemic
        currentState = currentState.Process(); // Calls Process method to ensure correct state is set.
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ai") && blueLife > 0)
        {
            blueLife = blueLife - 10f * Time.deltaTime;
        }
    }
}
