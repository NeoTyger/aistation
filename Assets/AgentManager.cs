using UnityEngine;

public class AgentManager : MonoBehaviour {

    // An array of GameObjects to store all the agents
    GameObject[] agents;
    GameObject[] enemies;
    public Transform player;
    public float speed = 3.0f;

    void Start() {

        // Grab everything with the 'ai' tag
        agents = GameObject.FindGameObjectsWithTag("ai");
        enemies = GameObject.FindGameObjectsWithTag("enemy");
    }

    // Update is called once per frame
    void Update() {

        // Check for left mouse button
        if (Input.GetMouseButtonDown(0)) {

            // Store the hit info
            RaycastHit hit;

            // Check to see if the mouse has been pressed and project a ray 0f 100 into the screen and store what it hits in hit
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {

                // Loop through all the agents and tell unity AI to move them to their destinations
                foreach (GameObject a in agents) {

                    a.GetComponent<AIControl>().agent.SetDestination(hit.point);
                }
            }
        }
    }
}
