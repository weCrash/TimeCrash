using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class SwitchSceneOnCollider : MonoBehaviour {

	[Header("If (< 0) goes to imediatly next (+1)")]
	public int nextSceneIndex; //Unity

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag != "Player"){
			return;
		}
		if (nextSceneIndex < 0){
			nextSceneIndex = SceneManager.GetActiveScene().buildIndex +1;
		}
        MemoryManager.Memory.Checkpoint.ID = -1;
		MemoryManager.Memory.Checkpoint.SceneIndex = nextSceneIndex;
		MemoryManager.ConsolidateMemory();

		SceneManager.LoadScene(nextSceneIndex);
	}
}
