using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 cameraOffset;

    private Transform playerPosition;
    private float lerpTime = .3f;
    private void Start()
    {
        playerPosition = player.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 nextPosition = playerPosition.position + cameraOffset;
        Vector3 temp = Vector3.Lerp(transform.position, nextPosition, lerpTime);
        transform.position = temp;

        transform.LookAt(playerPosition);
    }
}
