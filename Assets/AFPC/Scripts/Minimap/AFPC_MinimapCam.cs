/*This Script is used to remove Shadows From the Minimap Camera, If You want shadow in minimap cam, simply Disable this Script!
 */
using System.Collections;
using UnityEngine;

public class AFPC_MinimapCam : MonoBehaviour {

	private float initShadowDistance;

	private void OnPreRender()
	{
		initShadowDistance = QualitySettings.shadowDistance;
		QualitySettings.shadowDistance = 0f;
	}
	private void OnPostRender()
	{
		QualitySettings.shadowDistance = initShadowDistance;
	}

}
