using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class AFPC_AFPCwindow : EditorWindow {

	[MenuItem("AFPC/Contact Us")]
	public static void ShowWebSite()
	{
		Application.OpenURL ("https://gamedevtips9854.wixsite.com/hyperbolt");
	}
}
#endif
