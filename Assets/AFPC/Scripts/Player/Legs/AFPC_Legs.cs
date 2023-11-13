using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_Legs : MonoBehaviour {

	private Animator animator;
	private float horizontal, vertical;
	private bool isWalking = false, isSprinting = false, isJumping = false;
	private bool isCrouching = false, isProne = false, isProneMoving = false;
	private bool isClimbing = false, isSwimming = false;
	private AFPC_PlayerMovement _afpcPlayer;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		_afpcPlayer = GameObject.FindObjectOfType<AFPC_PlayerMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateAnimator ();
	}

	void UpdateAnimator()
	{
		if (_afpcPlayer == null)
			return;
		horizontal = _afpcPlayer.inputX;
		vertical = _afpcPlayer.inputY;
		animator.SetFloat ("Horizontal", horizontal);
		animator.SetFloat ("Vertical", vertical);

		isJumping = _afpcPlayer.IsJumping;
		isSprinting = _afpcPlayer.IsRunning;
		isWalking = _afpcPlayer.IsWalking;
		isProne = _afpcPlayer.IsProne;
		isCrouching = _afpcPlayer.IsCrouching;
		isSwimming = _afpcPlayer.IsSwimming;
		isClimbing = _afpcPlayer.CanClimb;
		if (isClimbing)
			isJumping = false;
		if (isSwimming)
			isJumping = false;
		if (isSwimming && isClimbing)
		{		
			isSwimming = false;
		}
		if (isProne)
		{
			if ((horizontal != 0 || vertical != 0) && _afpcPlayer.GetComponent<Rigidbody> ().velocity.sqrMagnitude > 0.05f)
				isProneMoving = true;
			else
				isProneMoving = false;
			animator.SetBool ("isProneMoving", isProneMoving);
		} else
		{
			animator.SetBool ("isProneMoving", false);
		}
		if(_afpcPlayer.GetComponent<Rigidbody> ().velocity.y < 0 && !_afpcPlayer.isGrounded && !_afpcPlayer.IsCrouching && !_afpcPlayer.IsProne && !_afpcPlayer.CanClimb && !_afpcPlayer.IsSliding && !_afpcPlayer.IsSwimming)
		{
			//Player is falling and we should play the jump loop
			isJumping = true;
		}
		animator.SetBool ("isCrouching", isCrouching);
		animator.SetBool ("isProne", isProne);
		animator.SetBool ("isWalking", isWalking);
		animator.SetBool ("isSprinting", isSprinting);
		animator.SetBool ("isJumping", isJumping);
		animator.SetBool ("isClimbing", isClimbing);
		animator.SetBool ("isSwimming", isSwimming);

	}
}
