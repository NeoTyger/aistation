using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Added since we're using a navmesh.

public class State
{
    // 'States' that the NPC could be in.
    public enum STATE
    {
        IDLE, PATROL, ATTACK
    };

    // 'Events' - where we are in the running of a STATE.
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name; // To store the name of the STATE.
    protected EVENT stage; // To store the stage the EVENT is in.
    protected GameObject npc; // To store the NPC game object.
    protected Transform player; // To store the transform of the player. This will let the guard know where the player is, so it can face the player and know whether it should be shooting or chasing (depending on the distance).
    protected State nextState; // This is NOT the enum above, it's the state that gets to run after the one currently running (so if IDLE was then going to PATROL, nextState would be PATROL).
    protected NavMeshAgent agent; // To store the NPC NavMeshAgent component.

    // Constructor for State
    public State(GameObject _npc, NavMeshAgent _agent, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        stage = EVENT.ENTER;
        player = _player;
    }

    // Phases as you go through the state.
    public virtual void Enter() { stage = EVENT.UPDATE; } // Runs first whenever you come into a state and sets the stage to whatever is next, so it will know later on in the process where it's going.
    public virtual void Update() { stage = EVENT.UPDATE; } // Once you are in UPDATE, you want to stay in UPDATE until it throws you out.
    public virtual void Exit() { stage = EVENT.EXIT; } // Uses EXIT so it knows what to run and clean up after itself.

    // The method that will get run from outside and progress the state through each of the different stages.
    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState; // Notice that this method returns a 'state'.
        }
        return this; // If we're not returning the nextState, then return the same state.
    }


}

// Constructor for Idle state.
public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Transform _player)
                : base(_npc, _agent, _player)
    {
        name = STATE.IDLE; // Set name of current state.
    }

    public override void Enter()
    {
        agent.isStopped = true;
        base.Enter(); // Sets stage to UPDATE.
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            nextState = new Patrol(npc, agent, player);
            stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }
        // The only place where Update can break out of itself. Set chance of breaking out at 10%.
        else if(Input.GetKeyDown(KeyCode.A))
        {
            nextState = new Attack(npc, agent, player);
            stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }
    }
    public override void Exit()
    {
        agent.isStopped = false;
        base.Exit();
    }
}

// Constructor for Patrol state.
public class Patrol : State
{
    int puntActual=0;
    Vector3[] listaPuntos = new Vector3[2];
    public Patrol(GameObject _npc, NavMeshAgent _agent, Transform _player)
                : base(_npc, _agent, _player)
    {
        name = STATE.PATROL; // Set name of current state.
        agent.speed = 2; // How fast your character moves ONLY if it has a path. Not used in Idle state since agent is stationary.
        agent.isStopped = false; // Start and stop agent on current path using this bool.
    }

    public override void Enter()
    {
        listaPuntos[0]=new Vector3(18f,0f,7f);
        listaPuntos[1]=new Vector3(0f,0f,13f);
        float lastDist = Mathf.Infinity; // Store distance between NPC and waypoints.

        // Calculate closest waypoint by looping around each one and calculating the distance between the NPC and each waypoint.
        for (int i = 0; i < 2; i++)
        {
            float distance = Vector3.Distance(npc.transform.position, listaPuntos[i]);
            if(distance < lastDist)
            {
                puntActual=i;
                lastDist = distance;
            }
        }
        base.Enter();
    }

    public override void Update()
    {
        // Check if agent hasn't finished walking between waypoints.
        if(Vector3.Distance(npc.transform.position, listaPuntos[puntActual]) < 1)
        {
           puntActual+=1;
            }
        if (puntActual==listaPuntos.Length){
            puntActual=0;
        }

        agent.SetDestination(listaPuntos[puntActual]); // Set agents destination 

        if (Input.GetKeyDown(KeyCode.I))
        {
            nextState = new Idle(npc, agent, player);
            stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }
        // The only place where Update can break out of itself. Set chance of breaking out at 10%.
        else if(Input.GetKeyDown(KeyCode.A))
        {
            nextState = new Attack(npc, agent, player);
            stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}


public class Attack : State
{
    public Attack(GameObject _npc, NavMeshAgent _agent, Transform _player)
                : base(_npc, _agent,_player)
    {
        name = STATE.ATTACK; // Set name to correct state.
        agent.speed = 4; // How fast your character moves
    }

    public override void Enter()
    {
        agent.isStopped = false; 
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position); // Set agents destination 

        if (Input.GetKeyDown(KeyCode.I))
        {
            nextState = new Idle(npc, agent, player);
            stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }
        // The only place where Update can break out of itself. Set chance of breaking out at 10%.
        else if(Input.GetKeyDown(KeyCode.P))
        {
            nextState = new Patrol(npc, agent, player);
            stage = EVENT.EXIT; // The next time 'Process' runs, the EXIT stage will run instead, which will then return the nextState.
        }   
       
    }

    public override void Exit()
    {
        base.Exit();
    }
}