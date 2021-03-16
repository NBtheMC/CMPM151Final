using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	public Transform target;
	public Vector3 offset;

	private float smoothcam = 6f;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredpos = target.position + offset;
        Vector3 smoothedpos = Vector3.Lerp(transform.position, desiredpos, smoothcam);
        transform.position = smoothedpos;
    }
}
