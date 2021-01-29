using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public NavMeshAgent agent;
    Vector3 playerPoition;
  
    // Update is called once per frame
    void Update()
    {
        playerPoition = GameObject.FindGameObjectWithTag("Player").transform.position;
        agent.SetDestination(playerPoition);
    }
}
