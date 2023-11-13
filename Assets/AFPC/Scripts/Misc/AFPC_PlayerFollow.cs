using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_PlayerFollow : MonoBehaviour {

	[SerializeField]
	private bool unfollowPlayerOnDeath = false; // If true and the target is player, then if the player is dead, this transform will not follow the player!
	[SerializeField]
	private bool followAllAxis = false, followXAxis = true, followYAxis = true, followZAxis = false;
	[SerializeField]
	private bool followRotAllAxis = false, followRotXAxis = true, followRotYAxis = true, followRotZAxis = false;
	[SerializeField]
	private Vector3 offset;
	[SerializeField]
	private Transform target;
	[SerializeField]
	private float timeToReachTargetPosition = 0f, timeToReachTargetRotation = 0f;
	private float Xpos, Ypos, Zpos, Xrot, Yrot, Zrot;
	private Vector3 currV, currV1;
	private float v1, v2, v3, v4, v5, v6;
	// Use this for initialization
	void Start () {
		if (target == null)
			target = GameObject.FindObjectOfType<AFPC_PlayerMovement> ().transform;
	}

	void FollowTarget()
	{
		if (followAllAxis)
		{
			transform.position = Vector3.SmoothDamp (transform.position, target.position + offset, ref currV, timeToReachTargetPosition);
		} else {
			Xpos = transform.position.x;
			Ypos = transform.position.y;
			Zpos = transform.position.z;
			if (followXAxis)
				Xpos = Mathf.SmoothDamp (Xpos, target.position.x + offset.x, ref v1, timeToReachTargetPosition);
			if (followYAxis)
				Ypos = Mathf.SmoothDamp (Ypos, target.position.y + offset.y, ref v2, timeToReachTargetPosition);
			if (followZAxis)
				Zpos = Mathf.SmoothDamp (Zpos, target.position.z + offset.z, ref v3, timeToReachTargetPosition);

			transform.position = new Vector3 (Xpos, Ypos, Zpos);
		}

		if (followRotAllAxis) {
			transform.eulerAngles = Vector3.SmoothDamp (transform.eulerAngles, target.eulerAngles, ref currV1, timeToReachTargetRotation);
		} else {
			Xrot = transform.eulerAngles.x;
			Yrot = transform.eulerAngles.y;
			Zrot = transform.eulerAngles.z;
			if (followRotXAxis)
				Xrot = Mathf.SmoothDamp (Xrot, target.eulerAngles.x, ref v4, timeToReachTargetRotation);
			if (followRotYAxis)
				Yrot = Mathf.SmoothDamp (Yrot, target.eulerAngles.y, ref v5, timeToReachTargetRotation);
			if (followRotZAxis)
				Zrot = Mathf.SmoothDamp (Zrot, target.eulerAngles.z, ref v6, timeToReachTargetRotation);

			transform.eulerAngles = new Vector3 (Xrot, Yrot, Zrot);
		}
	}
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			if (!Transform.ReferenceEquals(target, GameObject.FindObjectOfType<AFPC_PlayerMovement> ().transform))
			{
				FollowTarget ();
			}else
			{
				if(unfollowPlayerOnDeath)
				{
					if (!target.GetComponent<AFPC_SpawnManager> ().HasDied)
						FollowTarget ();
				}else
				{
					if (target.GetComponent<AFPC_SpawnManager> ().HasDied)
					{
						if(transform.GetComponentInChildren<AFPC_Legs>())
							followRotZAxis = true; 	// If this transform contains player legs, then rotation of legs should be that of AFPC_Player
					}
					FollowTarget ();
				}
			}
		}
	}

}
