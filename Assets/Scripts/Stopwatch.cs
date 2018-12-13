using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour {

    //Variaveis Unity
    public GameObject Hud, N1_1, N1_10, N1_100, N2_1, N2_10, N3_1, N3_10;
    public Sprite[] number;


    internal static int miliseconds = 0, seconds = 0, minute = 0;
    InputHandler input;
    static bool active = false;

	void Start () {

        //recebe os inputs do player
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
		MemoryManager.OnMemoryReset += resetTimer;
	}

	private void resetTimer(object sender, EventArgs e) {
		this.Playtime = 0;
	}

	private void FixedUpdate()
    {
		MemoryManager.Memory.Playtime = Playtime;

        //coverte um milisegundo para frame
        miliseconds += 16;

        //quando atingir um segundo, zerar o contador de milisegundo a adicionar 1 ao contador de segundos
        if(miliseconds >= 1000)
        {
            miliseconds -= 1000;
            seconds += 1;
        }

        //quando atingir um minuto, zerar o contador de segundos e adicionar 1 ao contador de minutos
        if (seconds >= 60)
        {
            seconds = 0;
            minute += 1;
        }

        //quando atingir uma hora, zerar tudo
        if (minute >= 60)
        {
            minute = 0;
            seconds = 0;
            miliseconds = 0;
        }

        //se a interface do cronometro estiver ativa
        if (active)
        {
            Hud.SetActive(true);

            //separando em centena, dezena e unidade
            int d100, d10, d1;

            //retira a unidade do contador de milisegundos
            d1 = miliseconds % 10;
            put_number(d1, N1_1);

            //retira a dezena do contador de milisegundos
            d10 = ((miliseconds % 100) - d1) / 10;
            put_number(d10, N1_10);

            //retira a centena do contador de milisegundos
            d100 = (miliseconds - (d10 * 10) - d1) / 100;
            put_number(d100, N1_100);


            //retira a unidade do contador de segundos
            d1 = seconds % 10;
            put_number(d1, N2_1);

            //retira a dezena do contador de segundos
            d10 = ((seconds % 100) - d1) / 10;
            put_number(d10, N2_10);


            //retira a unidade do contador de minutos
            d1 = minute % 10;
            put_number(d1, N3_1);

            //retira a dezena do contador de minutos
            d10 = ((minute % 100) - d1) / 10;
            put_number(d10, N3_10);

        }
        else Hud.SetActive(false);
    }

	void put_number(int index, GameObject obj)
    {
        //modifica o sprite do obj de acordo com o numero index
        Image img = obj.GetComponent<Image>();
        switch (index)
        {
            case 0:
                img.sprite = number[0];
                break;
            case 1:
                img.sprite = number[1];
                break;
            case 2:
                img.sprite = number[2];
                break;
            case 3:
                img.sprite = number[3];
                break;
            case 4:
                img.sprite = number[4];
                break;
            case 5:
                img.sprite = number[5];
                break;
            case 6:
                img.sprite = number[6];
                break;
            case 7:
                img.sprite = number[7];
                break;
            case 8:
                img.sprite = number[8];
                break;
            case 9:
                img.sprite = number[9];
                break;
        }
    }
    // Update is called once per frame
    void Update () {

        //ativa a interface do cronometro
        if (input.activityDown("Stopwatch")) {
			active = !active;
        }
	}

	public long Playtime {
		get {
			long playtime = 0; 
			playtime += miliseconds;
			playtime += seconds * 1000;
			playtime += minute * 60000;

			return playtime;
		}
		set {
			miliseconds = (int)value % 1000;
			seconds = (int)(value / 1000) % 60 ;;
			minute = (int)((value / (1000 * 60)) % 60);
		}
	}
}
