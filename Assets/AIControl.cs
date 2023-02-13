using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

    // Storage for the prefab
    public NavMeshAgent agent;

    void Start() {

        // Grab hold of the agents NavMeshAgent
        agent = this.GetComponent<NavMeshAgent>();
    }
}
