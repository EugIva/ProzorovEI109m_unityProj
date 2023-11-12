using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class AFPC_Door_KeyNTFC : MonoBehaviour {

	public Text text;
	private bool done = false;
	void Start () 
	{
		if (text != null)
			text.gameObject.SetActive (false);
	}
	public IEnumerator ShowNotification()
	{
		if (text == null)
			yield return null;
		
		text.gameObject.SetActive (true);
		yield return new WaitForSeconds (1f);
		text.gameObject.SetActive (false);
		done = true;
	}

	void Update()
	{
		if (done)
			StopCoroutine (ShowNotification ());
	}
}
