using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManagerSpawner : MonoBehaviour {

	public Memory starterMemoryInThisScene;

	private static int lastSceneIndex = -1;

	void Start () {
		//se já tiver um Memory Manager na scene, não crie
		if (FindObjectOfType<MemoryManager>() != null) {
			resetMemory();
			return;
		}

		//Cria e instancia o MemoryManager
		GameObject OnSceneReloadManager = new GameObject("OnSceneReloadManager");
		OnSceneReloadManager.AddComponent<MemoryManager>();
		//last scene index = index atual
		lastSceneIndex = gameObject.scene.buildIndex;
	}

	//se já existir, checa se mudou ou não de scene, se mudou, reseta o memoryManager
	void resetMemory() {
		if(lastSceneIndex == gameObject.scene.buildIndex) {
			return;
		}
		lastSceneIndex = gameObject.scene.buildIndex;
		MemoryManager.ForgetCollectable();
	}
}
