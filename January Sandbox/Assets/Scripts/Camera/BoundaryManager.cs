using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject boundary;

    private Transform player;
    private BoxCollider manageBoundary;

    private void Start()
    {
        manageBoundary = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        activateBoundary();
    }

    private void activateBoundary()
    {
        if(player.position.x > manageBoundary.bounds.min.x && player.position.x < manageBoundary.bounds.max.x &&
            player.position.z > manageBoundary.bounds.min.z && player.position.z < manageBoundary.bounds.max.z)
        {
            boundary.SetActive(true);
        }
        else
        {   
            boundary.SetActive(false);
        }
    }
}
