using UnityEngine;

public class CheckpointInfo {
	private int sceneIndex;
	private Vector3 savedPosition;
	private int id = -1;

	public int SceneIndex {
		get { return sceneIndex; } 
		set { this.sceneIndex = value; }
	}

	public int ID {
		get { return id; } 
		set { this.id = value; }
	}

	public Vector3 SavedPosition {
		get { return savedPosition; } 
		set { this.savedPosition = value; }
	}
}