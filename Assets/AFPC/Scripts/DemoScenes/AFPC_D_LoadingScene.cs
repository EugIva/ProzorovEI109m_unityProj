using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AFPC_D_LoadingScene : MonoBehaviour {

	public Text progressText;
	public string standaloneSceneName, mobileSceneName;

	private Slider _slider;
	// Use this for initialization
	void Start () {
		_slider = GetComponent<Slider> ();
		#if UNITY_STANDALONE || UNITY_WEBGL
		LoadLevel(standaloneSceneName);
		#endif

		#if UNITY_ANDROID || UNITY_IOS
		LoadLevel(mobileSceneName);
		#endif 
	}

	public void ChangeStandaloneLevelName(string levelName)
	{
		standaloneSceneName = levelName;
		LoadLevel (standaloneSceneName);
	}
	public void ChangeMobileLevelName(string levelName)
	{
		mobileSceneName = levelName;
		LoadLevel (mobileSceneName);
	}
	void LoadLevel(string levelName)
	{
		StartCoroutine(LoadAynsc (levelName));
	}

	IEnumerator LoadAynsc(string levelName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync (levelName);

		if (_slider == null)
		{
			if(GetComponent<Slider>())
				_slider = GetComponent<Slider> ();
		}
		while (!operation.isDone) 
		{
			float progress = Mathf.Clamp01 (operation.progress / 0.9f);
			if(_slider != null)
				_slider.value = progress;

			if(progressText != null)
				progressText.text = ((int)(progress*100)).ToString() + "%";

			yield return null;
		}
	}

}
