using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace MikeSchweitzer.SelectionHistory.Editor
{
	[InitializeOnLoad]
	public static class SelectionHistoryWindowUtils
	{
	    private static readonly bool debugEnabled = false;

	    static SelectionHistoryWindowUtils()
	    {
		    Selection.selectionChanged += SelectionRecorder;
	    }
		
	    private static void SelectionRecorder()
	    {
		    if (Selection.activeObject != null) {
			    if (debugEnabled) {
				    Debug.Log ("Recording new selection: " + Selection.activeObject.name);
			    }

			    var selectionHistory = EditorTemporaryMemory.Instance.selectionHistory;
			    selectionHistory.UpdateSelection (Selection.activeObject);
		    } 
	    }
		
	    [MenuItem("Window/Selection History/Previous selection %#,")]
	    [Shortcut("Selection History/Previous Selection")]
	    public static void PreviousSelection()
	    {
		    var selectionHistory = EditorTemporaryMemory.Instance.selectionHistory;
		    selectionHistory.Previous ();
		    Selection.activeObject = selectionHistory.GetSelection ();
	    }

	    [MenuItem("Window/Selection History/Next selection %#.")]
	    [Shortcut("Selection History/Next Selection")]
	    public static void NextSelection()
	    {
		    var selectionHistory = EditorTemporaryMemory.Instance.selectionHistory;
		    selectionHistory.Next();
		    Selection.activeObject = selectionHistory.GetSelection ();
	    }

	    public static void PingEntry(SelectionHistory.Entry e)
	    {
		    if (e.GetReferenceState() == SelectionHistory.Entry.State.ReferenceUnloaded)
		    {
			    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(e.scenePath);
			    EditorGUIUtility.PingObject(sceneAsset);
		    } else
		    {
			    EditorGUIUtility.PingObject(e.reference);
		    }
	    }
	}
}