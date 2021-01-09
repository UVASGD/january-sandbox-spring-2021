using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 cameraOffset;

    private Transform playerPosition;
    private float lerpTime = .3f;
    private BoxCollider cameraCollider;
    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        cameraCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayerWithScreens();
    }
    
    private void FollowPlayerWithScreens()
    {
        if (GameObject.Find("Boundary"))
        {
            Vector3 temp = new Vector3(Mathf.Clamp(playerPosition.position.x, GameObject.Find("Boundary").GetComponent<BoxCollider>().bounds.min.x + cameraCollider.size.x / 2, GameObject.Find("Boundary").GetComponent<BoxCollider>().bounds.max.x - cameraCollider.size.x / 2)
                                        , playerPosition.position.y
                                        , Mathf.Clamp(playerPosition.position.z, GameObject.Find("Boundary").GetComponent<BoxCollider>().bounds.min.z + cameraCollider.size.z / 2, GameObject.Find("Boundary").GetComponent<BoxCollider>().bounds.max.z - cameraCollider.size.z / 2));
            Debug.Log(temp);
            Vector3 realTemp = temp + cameraOffset;
            Vector3 nextPosition = Vector3.Lerp(transform.position, realTemp, lerpTime);
            transform.position = nextPosition;
        }
    }
}
