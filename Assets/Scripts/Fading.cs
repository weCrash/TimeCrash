using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour
{

	public Texture2D fadeOutTexture; //Textura que vai tomar lugar na tela durante a transição
	public float fadeSpeed = 0.8f; //Tempo em que vai durar a transição

	private int drawDepth = -1000; //"Profundidade" do preenchimento (So pra ter certeza de que vai cobrir tudo mesmo)
	private float alpha = 1.0f; //Transparencia da imagem
	private int fadeDir = -1; //Direção para onde vai ocorrer o fade

	void OnGUI()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;

		alpha = Mathf.Clamp01(alpha); //Mathf.Clamp01 armazena valores entre 0 e 1

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}


	public float BeginFade(int direction)
	{
		fadeDir = direction;
		return (fadeSpeed);
	}

	void OnLevelLoaded(Scene scene, LoadSceneMode mode)//Quando a fase começar, iniciara o Fade
	{
		BeginFade(-1);
	}

	//make the onlevelwasloaded legit
	void OnEnable() { SceneManager.sceneLoaded += OnLevelLoaded; }
	void OnDisable() { SceneManager.sceneLoaded -= OnLevelLoaded; }
}
