using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class AFPC_PlatformBuilder : EditorWindow {

	private GameObject Platform;
	private Vector3 spawnPos;
	private Vector3 platformSize;

	[MenuItem("AFPC/Create/Platform")]
	public static void ShowWindow()
	{
		GetWindow<AFPC_PlatformBuilder> ("Create Platform");
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical ("ShurikenEffectBg");
		Platform = (GameObject)EditorGUILayout.ObjectField ("Platform To Spawn: ", Platform, typeof(GameObject), true);
		spawnPos = EditorGUILayout.Vector3Field ("Platform Spawn Position: ", spawnPos);
		platformSize = EditorGUILayout.Vector3Field ("Platform Size: ", platformSize);
		EditorGUILayout.HelpBox ("Platform Size is the Local Scale of the Platform Prefab", MessageType.Info);
		GUILayout.Box (GUIContent.none, "horizontalSlider");
		if (GUILayout.Button ("Create Platform!")) 
		{
			CreatePlatform ();
		}

		EditorGUILayout.EndVertical ();
	}


	private void CreatePlatform()
	{
		if (Platform != null) 
		{
			GameObject platform = Instantiate (Platform, spawnPos, Platform.transform.rotation) as GameObject;
			string platformNewName = platform.name;
			platformNewName =	platformNewName.Remove (platformNewName.Length - 7, 7);
			platform.name = platformNewName;
			GameObject platformParent = new GameObject ("PlatformParent");
			platformParent.transform.position = spawnPos;
			platform.transform.SetParent (platformParent.transform, true);
			platformParent.AddComponent<BoxCollider> ();
			BoxCollider boxColl = platformParent.GetComponent<BoxCollider> ();
			boxColl.size = platformSize;
			boxColl.isTrigger = false;
			platformParent.AddComponent<AFPC_Platform> ();
		} else {
			Debug.LogError ("No Platform Selected to Spawn!");
		}
	}
}
#endif
