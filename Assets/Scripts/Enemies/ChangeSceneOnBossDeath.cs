using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnBossDeath : MonoBehaviour {

	[Header("If (< 0) goes to imediatly next (+1)")]
	public int NextSceneIndex = 1;

	private void Start() {
		FindObjectOfType<BossLife>().OnEnemyDead += WhenBossDie;
	}

	private void WhenBossDie(object sender, System.EventArgs e) {
		if (NextSceneIndex < 0) {
			NextSceneIndex = this.gameObject.scene.buildIndex+1;
		}
		SceneManager.LoadScene(NextSceneIndex);
	}
}
