using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_Platform : MonoBehaviour {

	private AFPC_PlayerMovement _afpcPlayer;
	private Transform playerParent;

	void Start()
	{
		_afpcPlayer = GameObject.FindObjectOfType<AFPC_PlayerMovement> ();
		if (_afpcPlayer.transform.parent != null)
			playerParent = _afpcPlayer.transform.parent;
		_afpcPlayer = null;
	}
	void OnCollisionStay(Collision coll)
	{
		if (_afpcPlayer != null)
			return;
		if(coll.gameObject.GetComponent<AFPC_PlayerMovement>())
		{
			_afpcPlayer = coll.gameObject.GetComponent<AFPC_PlayerMovement> ();
			_afpcPlayer.gameObject.transform.SetParent (this.transform, true);
		}
	}
	void OnCollisionExit(Collision coll)
	{
		if (_afpcPlayer == null)
			return;
		if(playerParent != null)
			_afpcPlayer.transform.parent = playerParent;
		else
			_afpcPlayer.transform.parent = null;
		_afpcPlayer = null;
	}

}
