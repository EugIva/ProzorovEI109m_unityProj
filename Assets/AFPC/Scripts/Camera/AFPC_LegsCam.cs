using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_LegsCam : MonoBehaviour {

	[SerializeField]
	private LayerMask cullingMask, legsLayer, playerCameraCullingMask; 
	//playerCameraCullingMask - includes all the layers the mainCamera/Player Camera Will render except the layer(s) which needs to be renderered above legs in this case Water Layer
	//Culling mask includes all the layers you want to render ABOVE legs + legs layer!
	private AFPC_PlayerMovement _afpcPlayer;
	private LayerMask playerCameraInitCullingMask;
	private Camera _cam;
	// Use this for initialization
	void Start () {
		_cam = GetComponent<Camera> ();
		_afpcPlayer = GameObject.FindObjectOfType<AFPC_PlayerMovement> ();
		if(_afpcPlayer != null)
		{
			if (_afpcPlayer.fpsCamera != null)
				playerCameraInitCullingMask = _afpcPlayer.fpsCamera.cullingMask;
		}
	}

	void RenderThingsOnTopOfLegs()
	{
		_cam.cullingMask = cullingMask;
		_cam.clearFlags = CameraClearFlags.Nothing;
		if(_afpcPlayer.fpsCamera != null)
			_afpcPlayer.fpsCamera.cullingMask = playerCameraCullingMask;
	}
	void RenderThingsBelowLegs()
	{
		_cam.cullingMask = legsLayer;
		_cam.clearFlags = CameraClearFlags.Depth;
		if(_afpcPlayer.fpsCamera != null)
			_afpcPlayer.fpsCamera.cullingMask = playerCameraInitCullingMask;
	}
	// Update is called once per frame
	void Update () 
	{
		if (_afpcPlayer == null || _cam == null)
			return;

		if (_afpcPlayer.IsSwimming)
			RenderThingsOnTopOfLegs ();
		else
			RenderThingsBelowLegs ();
	}
}
