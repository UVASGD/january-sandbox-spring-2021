using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 cameraOffset;

    private Transform playerPosition;
    private float lerpTime = .3f;
    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {       
        Vector3 temp = playerPosition.position + cameraOffset;
        Vector3 nextPosition = Vector3.Lerp(transform.position, temp, lerpTime);
        transform.position = nextPosition;
        
    }
}
