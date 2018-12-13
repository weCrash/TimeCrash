using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{

    //Varivaveis Unity
    public int imunity_blink_interval = 5;
    public float health = 10, imunity_time = 3;
    public SpriteRenderer player_sprite;
    public GameObject hud, d10, d1;
    public Sprite high_life, mid_life, low_life;
    public Sprite[] number;
    public bool active_damage = true;

    float max_health;
    Image hud_image, d10_image, d1_image;
    bool imunity = false;
    int imunity_count = 0, imunity_time_frames;

    public void Start()
    {
        //Ativa a colisão do player com inimigos
        Physics2D.IgnoreLayerCollision(9, 8, false);
        Physics2D.IgnoreLayerCollision(9, 19, false);
        Physics2D.IgnoreLayerCollision(9, 20, false);

        //converte o tempo de imunidade de segundos para frames
        imunity_time_frames = (int)imunity_time * 60;

        //define a vida maxima
        max_health = health;

        //defini os objetos na interface de vida
        hud_image = hud.GetComponent<Image>();
        d10_image = d10.GetComponent<Image>();
        d1_image = d1.GetComponent<Image>();

        MemoryManager.OnSave += passHealthToMemory;
    }

    private void passHealthToMemory(object sender, EventArgs e)
    {
		health = max_health;
        MemoryManager.Memory.Health = health;
    }


    void Update()
    {
        //Modifica a imagem do display de vida de acordo com a quantidade de vida em porcentagem
        if (health * 100 / max_health <= 33)
            hud_image.sprite = low_life;

        else if (health * 100 / max_health <= 66)
            hud_image.sprite = mid_life;

        else
            hud_image.sprite = high_life;

        #region LifeDisplay

        //define o numero de unidade de acordo com a vida
        switch ((int)health / 10)
        {
            case 0:
                d10_image.sprite = number[0];
                break;
            case 1:
                d10_image.sprite = number[1];
                break;
            case 2:
                d10_image.sprite = number[2];
                break;
            case 3:
                d10_image.sprite = number[3];
                break;
            case 4:
                d10_image.sprite = number[4];
                break;
            case 5:
                d10_image.sprite = number[5];
                break;
            case 6:
                d10_image.sprite = number[6];
                break;
            case 7:
                d10_image.sprite = number[7];
                break;
            case 8:
                d10_image.sprite = number[8];
                break;
            case 9:
                d10_image.sprite = number[9];
                break;
        }

        //define o numero de decimal de acordo com a vida
        switch ((int)health % 10)
        {
            case 0:
                d1_image.sprite = number[0];
                break;
            case 1:
                d1_image.sprite = number[1];
                break;
            case 2:
                d1_image.sprite = number[2];
                break;
            case 3:
                d1_image.sprite = number[3];
                break;
            case 4:
                d1_image.sprite = number[4];
                break;
            case 5:
                d1_image.sprite = number[5];
                break;
            case 6:
                d1_image.sprite = number[6];
                break;
            case 7:
                d1_image.sprite = number[7];
                break;
            case 8:
                d1_image.sprite = number[8];
                break;
            case 9:
                d1_image.sprite = number[9];
                break;
        }
        #endregion

        if (!active_damage)
        {
            Physics2D.IgnoreLayerCollision(9, 8);
            Physics2D.IgnoreLayerCollision(9, 19);
            Physics2D.IgnoreLayerCollision(9, 20);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(9, 8, false);
            Physics2D.IgnoreLayerCollision(9, 19, false);
            Physics2D.IgnoreLayerCollision(9, 20, false);
        }
    }

    private void FixedUpdate()
    {

        //se a imunidade estiver ativa
        if(imunity)
        {

            //ignora colisão com inimigos
            Physics2D.IgnoreLayerCollision(9, 8);
            Physics2D.IgnoreLayerCollision(9, 20);
            Physics2D.IgnoreLayerCollision(9, 19);

            //se o tempo de imunidade acabar, desiga a colisão do player com inimigos, ativa o sprite_renderer do player e desativa a umanidade
            if (imunity_count >= imunity_time_frames)
            {
                Physics2D.IgnoreLayerCollision(9, 8, false);
                Physics2D.IgnoreLayerCollision(9, 20, false);
                Physics2D.IgnoreLayerCollision(9, 19, false);
                player_sprite.enabled = true;
                imunity = false;
            }

            //checa se o intervalo de tempo do piscar do player passou para ativar ou desativar o sprite_renderer do player
            if(imunity_count % imunity_blink_interval == 0 && imunity)
            {
                if (player_sprite.enabled)
                    player_sprite.enabled = false;
                else
                    player_sprite.enabled = true;
            }
            imunity_count++;

        }
    }


    private void OnDestroy()
    {
        MemoryManager.OnSave -= passHealthToMemory;
    }

    public void set00()
    {
        health = 0;
        d10_image.sprite = number[0];
        d1_image.sprite = number[0];
    }

    public void TakeDamage(float damage)
    {
        //se a imunidade não estiver ativa, tira vida do personagem e ativa a imunidade
        if (!Imunity && active_damage)
        {
            health -= damage;
            imunity_count = 0;
            imunity = true;
        }
    }

    public bool Imunity
    {
        get
        {
            return imunity;
        }

        set
        {
            imunity = value;
        }
    }
}
