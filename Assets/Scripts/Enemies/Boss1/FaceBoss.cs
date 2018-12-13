using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBoss : MonoBehaviour
{


    [Header("Rest")]
    public float rest_time;

    [Header("Floating")]
    public float float_time = 2;
    public float max_gravity_scale = 0.02f;
    float frames_cos_value, cos_relation_max;

    int float_count = 0;

    bool float_enable = false;

    [Header("Teleport")]
    public Vector2 max_vertex;
    public Vector2 min_vertex;
    public GameObject warning_sign;
    public float disappearance_time = 0.5f,
        warning_sign_time = 1,
        appear_time = 0.2f;

    int teleport_count = 0,
        warning_sign_count = 0,
        appear_count = 0;

    bool start_teleport = false,
        warning_sign_appear = false,
        appear_start = false;

    [Header("Hide")]
    public float hide_time = 0.5f;

    int hide_count = 0;
    bool hide_enable = false;

    [Header("Show")]
    public float show_time = 0.5f;

    int show_count = 0;
    bool show_enable = false;

    [Header("Platform Attack")]
    public GameObject plataform1;
    public GameObject plataform2;
    public GameObject hookpoint1;
    public GameObject hookpoint2;
    public GameObject spikes1;
    public GameObject spikes2;
    public GameObject fire;
    public GameObject plataform_warning;
    public float plataform1_position_x, plataform2_position_x,
        hookpoint1_position_y, hookpoint2_position_y,
        spikes1_position_x, spikes2_position_x,
        fire_position_y;
    public float plataform_moving_speed = 3,
        fire_delay = 1,
        spikes_start_delay = 2,
        spikes_recall_delay = 1.8f,
        spikes_attack_time = 0.1f,
        spikes_total_attacks = 5;
    public Vector2 warning_bot_location;
    public Vector2 warning_left_location;
    public Vector2 warning_right_location;

    int spikes_attacks_count = 0, spike_index;
    bool plataform_attack_teleport = false, plataform_attack_hide = false;
    Vector2 plataform1_velocity, plataform2_velocity,
        hookpoint1_velocity, hookpoint2_velocity,
        spikes1_start_position, spikes2_start_position,
        fire_velocity;

    [Header("EnemyAttack")]
    public GameObject banamo_plataform;
    public GameObject jumo;
    public float banamo_plataform_position_y;
    public float banamo_plataform_delay = 2, jumo_spawn_delay = 1;
    public int total_jumo_spawn = 7;
    public int max_jumos_spawn_actives = 3;
    float banamo_plataform_velocity_y;
    bool acitve_jumo_spawn = false, jumo_multi_spawn;
    bool enemy_attack_teleport = false, enemy_attack_hide = false;
    List<GameObject> jumos_objects;

    [Header("CreazyMonkey")]
    public GameObject monkey;
    public float monkey_frequence = 1;
    public int monkey_amount = 10;
    public Vector2 right_head_position;
    public Vector2 left_head_position;

    [Header("MegaDash")]
    public float dash_velocity;
    public float dash_delay;
    public float head_attack_scale;
    Rigidbody2D rb_boss;
    SpriteRenderer sr_boss, sr_warning_sign;
    GameObject player;

    [Header("MachineGun")]
    public GameObject banana;
    public float teleport_frequence, shot_frequence;
    public float max_teleports, max_shot;

    Vector2 pattern_position, player_distance, teleport_position;
    bool shake_on, more_damage;


    int direction, last_index;



    float velocity_x,
        velocity_y;

    bool can_take_damage = true, stay_hide = false;

    public int Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb_boss = gameObject.GetComponent<Rigidbody2D>();
        sr_boss = gameObject.GetComponent<SpriteRenderer>();
        sr_warning_sign = warning_sign.GetComponent<SpriteRenderer>();
        sr_warning_sign.color = new Color(1, 1, 1, 0);
        Physics2D.IgnoreLayerCollision(19, 9, false);

        //torna o Boss ativo
        rb_boss.simulated = true;

        StartCoroutine(Rest());

    }

    private void FixedUpdate()
    {
        player_distance = player.transform.position - transform.position;
        Direction = (int)Mathf.Sign(player_distance.x);
        if (Direction == 1) sr_boss.flipX = true;
        else sr_boss.flipX = false;

        if(stay_hide) Physics2D.IgnoreLayerCollision(19, 9);


        velocity_x = rb_boss.velocity.x;
        velocity_y = rb_boss.velocity.y;

        Float();
        Teleport();
        Hide();
        PlataformAttack();
        EnemyAttack();
        Show();
        JumoSpawn();
        Shake();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && can_take_damage)
        {
            if(more_damage)
                player.GetComponentInChildren<PlayerLife>().TakeDamage(5);
            else
                player.GetComponentInChildren<PlayerLife>().TakeDamage(2);
        }
    }

    IEnumerator Rest()
    {
        float_enable = true;
        yield return new WaitForSeconds(rest_time);
        RandomAttack();
    }


    IEnumerator Wait()
    {
        show_enable = true;
        yield return new WaitForSeconds(show_time);
        teleport_position = RandomPosition();
        start_teleport = true;
        yield return new WaitForSeconds(appear_time + warning_sign_time + disappearance_time);
        StartCoroutine(Rest());
    }

    IEnumerator StartPlataformAttack()
    {
        hide_enable = true;
        plataform_warning.SetActive(true);
        plataform_warning.transform.position = warning_bot_location;

        CircleCollider2D collider1 = hookpoint1.GetComponent<CircleCollider2D>();
        CircleCollider2D collider2 = hookpoint2.GetComponent<CircleCollider2D>();
        collider1.enabled = false;
        collider2.enabled = false;

        plataform1.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityToMove(plataform1_position_x, plataform1.transform.position.x, plataform_moving_speed), 0);
        plataform1_velocity = plataform1.GetComponent<Rigidbody2D>().velocity;
        plataform2.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityToMove(plataform2_position_x, plataform2.transform.position.x, plataform_moving_speed), 0);
        plataform2_velocity = plataform2.GetComponent<Rigidbody2D>().velocity;
        hookpoint1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocityToMove(hookpoint1_position_y, hookpoint1.transform.position.y, plataform_moving_speed));
        hookpoint1_velocity = hookpoint1.GetComponent<Rigidbody2D>().velocity;
        hookpoint2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocityToMove(hookpoint2_position_y, hookpoint2.transform.position.y, plataform_moving_speed));
        hookpoint2_velocity = hookpoint2.GetComponent<Rigidbody2D>().velocity;

        yield return new WaitForSeconds(plataform_moving_speed);

        plataform1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        plataform2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        hookpoint1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        hookpoint2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        collider1.enabled = true;
        collider2.enabled = true;


        spikes1_start_position = spikes1.transform.position;
        spikes2_start_position = spikes2.transform.position;
        plataform_warning.SetActive(false);
        fire.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocityToMove(fire_position_y, fire.transform.position.y, fire_delay));
        fire_velocity = fire.GetComponent<Rigidbody2D>().velocity;

        yield return new WaitForSeconds(fire_delay);

        fire.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        StartCoroutine(SpikeAttackDelay());
    }


    IEnumerator SpikeAttackDelay()
    {
        if (spikes_attacks_count == 0)
        {
            float distance_x1 = Mathf.Abs(spikes1.transform.position.x - player.transform.position.x);
            float distance_x2 = Mathf.Abs(spikes2.transform.position.x - player.transform.position.x);

            if (distance_x1 < distance_x2)
                spike_index = 1;
            else
                spike_index = 2;
        }

        plataform_warning.SetActive(true);
        if (spike_index == 1)
            plataform_warning.transform.position = warning_left_location;
        else
            plataform_warning.transform.position = warning_right_location;

        yield return new WaitForSeconds(spikes_start_delay);

        spikes_attacks_count++;
        plataform_warning.SetActive(false);
        if (spike_index == 1)
            spikes1.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityToMove(spikes1_position_x, spikes1.transform.position.x, spikes_attack_time), 0);
        else
            spikes2.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityToMove(spikes2_position_x, spikes2.transform.position.x, spikes_attack_time), 0);

        yield return new WaitForSeconds(spikes_attack_time);

        if (spike_index == 1)
        {
            spikes1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            spike_index = 2;
            StartCoroutine(RecallSpike(1));
        }
        else
        {
            spikes2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            spike_index = 1;
            StartCoroutine(RecallSpike(2));
        }

        if (spikes_attacks_count < spikes_total_attacks)
            StartCoroutine(SpikeAttackDelay());
        else
            StartCoroutine(RecallPlataformAttack());


    }

    IEnumerator RecallSpike(int SpikeIndex)
    {

        if (SpikeIndex == 1)
            spikes1.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityToMove(spikes1_start_position.x, spikes1.transform.position.x, spikes_recall_delay), 0);
        else
            spikes2.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityToMove(spikes2_start_position.x, spikes2.transform.position.x, spikes_recall_delay), 0);

        yield return new WaitForSeconds(spikes_recall_delay);

        if (SpikeIndex == 1)
            spikes1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        else
            spikes2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }


    IEnumerator RecallPlataformAttack()
    {
        spikes_attacks_count = 0;
        fire.GetComponent<Rigidbody2D>().velocity = fire_velocity * -1;
        yield return new WaitForSeconds(fire_delay);
        fire.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        CircleCollider2D collider1 = hookpoint1.GetComponent<CircleCollider2D>();
        CircleCollider2D collider2 = hookpoint2.GetComponent<CircleCollider2D>();
        player.GetComponent<newHook>().enabled = false;
        player.GetComponent<DistanceJoint2D>().enabled = false;
        player.GetComponent<newHook>().enabled = true;
        collider1.enabled = false;
        collider2.enabled = false;
        plataform1.GetComponent<Rigidbody2D>().velocity = plataform1_velocity * -1;
        plataform2.GetComponent<Rigidbody2D>().velocity = plataform2_velocity * -1;
        hookpoint1.GetComponent<Rigidbody2D>().velocity = hookpoint1_velocity * -1;
        hookpoint2.GetComponent<Rigidbody2D>().velocity = hookpoint2_velocity * -1;
        yield return new WaitForSeconds(plataform_moving_speed);
        plataform1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        plataform2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        hookpoint1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        hookpoint2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        StartCoroutine(Wait());
    }

    void PlataformAttack()
    {
        if (plataform_attack_teleport)
        {

            teleport_position = RandomPosition();
            start_teleport = true;
            plataform_attack_teleport = false;
            plataform_attack_hide = true;
        }
        if (plataform_attack_hide)
        {
            if (!start_teleport && !warning_sign_appear && !appear_start)
            {
                StartCoroutine(StartPlataformAttack());
                plataform_attack_hide = false;
            }
        }
    }


    IEnumerator BanamoStart()
    {
        hide_enable = true;
        banamo[] banamos = banamo_plataform.gameObject.GetComponentsInChildren<banamo>();
        foreach (banamo banamo in banamos)
            banamo.enabled = false;
        banamo_plataform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocityToMove(banamo_plataform_position_y, banamo_plataform.transform.position.y, banamo_plataform_delay));
        banamo_plataform_velocity_y = banamo_plataform.GetComponent<Rigidbody2D>().velocity.y;
        yield return new WaitForSeconds(banamo_plataform_delay);
        banamo_plataform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        foreach (banamo banamo in banamos)
            banamo.enabled = true;
        jumos_objects = new List<GameObject>();
        acitve_jumo_spawn = true;

    }

    IEnumerator JumoMultiSpawn(int index)
    {
        for (int i = 0; i < index; i++)
        {
            jumos_objects[i].SetActive(true);
            yield return new WaitForSeconds(jumo_spawn_delay);
        }
        jumo_multi_spawn = true;
    }

    IEnumerator EnemyAttackEnd()
    {
        acitve_jumo_spawn = false;
        jumo_multi_spawn = false;
        banamo_plataform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, banamo_plataform_velocity_y * -1);
        yield return new WaitForSeconds(banamo_plataform_delay);
        banamo_plataform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        StartCoroutine(Wait());
    }

    void EnemyAttack()
    {
        if (enemy_attack_teleport)
        {
            teleport_position = RandomPosition();
            start_teleport = true;
            enemy_attack_teleport = false;
            enemy_attack_hide = true;
        }
        if (enemy_attack_hide)
        {
            if (!start_teleport && !warning_sign_appear && !appear_start)
            {
                StartCoroutine(BanamoStart());
                enemy_attack_hide = false;
            }
        }
    }

    void JumoSpawn()
    {
        if (acitve_jumo_spawn)
        {
            if (jumos_objects.Count == 0)
            {
                GameObject exemple_jumo = Instantiate(jumo);
                exemple_jumo.SetActive(false);
				exemple_jumo.GetComponent<EnemyBase>().enabled = false;
                exemple_jumo.transform.position = transform.position;

                for (int i = 0; i < total_jumo_spawn; i++)
                {
                    jumos_objects.Add(exemple_jumo);
                    jumos_objects[i] = Instantiate(exemple_jumo);
                }
                Destroy(exemple_jumo);

            }
            else
            {
                if (jumos_objects.Count > max_jumos_spawn_actives -1)
                {
                    foreach (GameObject jun in jumos_objects)
                    {
                        if (jun == null)
                        {
                            jumos_objects.Remove(jun);
                            if (jumos_objects.Count > max_jumos_spawn_actives - 1)
                                jumos_objects[max_jumos_spawn_actives - 1].SetActive(true);
                            break;
                        }
                    }
                }

                if (!jumo_multi_spawn)
                {
                    int x = 0;
                    jumo_multi_spawn = true;
                    foreach (GameObject jum in jumos_objects)
                    {
                        if (jum.activeInHierarchy) x++;

                    }
                    StartCoroutine(JumoMultiSpawn(max_jumos_spawn_actives));
                }

                if(jumos_objects[0] == null)
                {
                    acitve_jumo_spawn = false;
                    jumo_multi_spawn = false;
                    jumos_objects = new List<GameObject>();
                    StartCoroutine(EnemyAttackEnd());
                }

            }

        }
    }

    IEnumerator CrazyMonkeyAttack()
    {
        rb_boss.bodyType = RigidbodyType2D.Kinematic;
        if (Random.Range(1, 2) == 1)
            teleport_position = right_head_position;
        else teleport_position = left_head_position;
        start_teleport = true;
        yield return new WaitForSeconds(appear_time + warning_sign_time + disappearance_time);
        for(int i = 0; i < monkey_amount; i++)
        {
            GameObject x = Instantiate(monkey);
            x.transform.position = transform.position;
            yield return new WaitForSeconds(monkey_frequence);
        }
        rb_boss.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(Wait());
    }

    IEnumerator HeadAttack()
    {
        more_damage = true;
        hookpoint1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocityToMove(hookpoint1_position_y, hookpoint1.transform.position.y, plataform_moving_speed));
        hookpoint1_velocity = hookpoint1.GetComponent<Rigidbody2D>().velocity;
        hookpoint2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocityToMove(hookpoint2_position_y, hookpoint2.transform.position.y, plataform_moving_speed));
        hookpoint2_velocity = hookpoint2.GetComponent<Rigidbody2D>().velocity;

        yield return new WaitForSeconds(plataform_moving_speed);

        hookpoint1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        hookpoint2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        hookpoint1.GetComponent<CircleCollider2D>().enabled = true;
        hookpoint2.GetComponent<CircleCollider2D>().enabled = true;
        float_enable = false;
        teleport_position = new Vector2(205.6f, -1.15f);
        start_teleport = true;
        Vector3 original_scale;
        original_scale = transform.localScale;
        
        yield return new WaitForSeconds(appear_time + warning_sign_time + disappearance_time);
        shake_on = true;
        Physics2D.IgnoreLayerCollision(19, 9, false);
        transform.localScale = new Vector3(transform.localScale.x * head_attack_scale, transform.localScale.y * head_attack_scale, 1);
        yield return new WaitForSeconds(dash_delay);

        Physics2D.IgnoreLayerCollision(19, 9);
        shake_on = false;
        rb_boss.velocity = new Vector2(dash_velocity * -1, 0);

        yield return new WaitForSeconds(1f);
        transform.localScale = original_scale;
        player.GetComponent<newHook>().enabled = false;
        player.GetComponent<DistanceJoint2D>().enabled = false;
        player.GetComponent<newHook>().enabled = true;
        hookpoint1.GetComponent<CircleCollider2D>().enabled = false;
        hookpoint2.GetComponent<CircleCollider2D>().enabled = false;
        hookpoint1.GetComponent<Rigidbody2D>().velocity = hookpoint1_velocity * -1;
        hookpoint2.GetComponent<Rigidbody2D>().velocity = hookpoint2_velocity * -1;

        yield return new WaitForSeconds(plataform_moving_speed);
        hookpoint1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        hookpoint2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        more_damage = false;

        StartCoroutine(Wait());
    }

    IEnumerator MachineGun()
    {
        //teleporta o objeto
        teleport_position = new Vector2(Random.Range(max_vertex.x, min_vertex.x), 6.98f);
        start_teleport = true;

        //espera o tempo de teleport acorrer
        yield return new WaitForSeconds(appear_time + warning_sign_time + disappearance_time);

        //repete teleportes
        for (int i = 0; i < max_teleports; i ++)
        {
            //em um maximo de bananas atiradas em um tempo shot_frequence
            for(int d = 0; d < max_shot; d++)
            {
                //instancia a banana na posição do objeto
                GameObject x = Instantiate(banana);
                x.transform.position = transform.position;
                yield return new WaitForSeconds(shot_frequence);
            }
            //teleporta para um lugar aleatorio
            yield return new WaitForSeconds(teleport_frequence);
            teleport_position = new Vector2(Random.Range(max_vertex.x, min_vertex.x), 6.98f);
            start_teleport = true;
            yield return new WaitForSeconds(appear_time + warning_sign_time + disappearance_time);

        }
        //inicia a coroutine wait()
        StartCoroutine(Wait());
    }

    void RandomAttack()
    {
        int x = Random.Range(1, 5);

        //impede que um ataque ocorra repetida vezes
        if (last_index == x)
            x++;
        if (x == 6) x = 1;

        //escolhe um ataque aleatorio e o inicia
        switch (x)
        {
            case 1:
                plataform_attack_teleport = true;
                last_index = 1;
                break;
            case 2:
                enemy_attack_teleport = true;
                last_index = 2;
                break;
            case 3:
                StartCoroutine(CrazyMonkeyAttack());
                last_index = 3;
                break;
            case 4:
                StartCoroutine(HeadAttack());
                last_index = 4;
                break;
            case 5:
                StartCoroutine(MachineGun());
                last_index = 5;
                break;
        }
    }

    void Float()
    {

        if (float_enable)
        {
            //converte para frames e descobre qual é o 60 frames = 90° 1 frame = x
            cos_relation_max = float_time / 4 * 60;
            frames_cos_value = 90 / cos_relation_max;
            //usa função do cos para modificar a escala de gravidade
            rb_boss.gravityScale = max_gravity_scale * Mathf.Cos(frames_cos_value * Mathf.Deg2Rad * float_count);
            if (float_count >= float_time * 60) float_count = 0;
            float_count++;
        }
        else
        {
            //desativa a flutação junto com as propriedades de flutuação do boss
            rb_boss.velocity = new Vector2(velocity_x, 0);
            rb_boss.gravityScale = 0;
            float_count = 0;
        }
    }

    void Hide()
    {
        if (hide_enable)
        {
            //desativa colisão com o player e balas
            Physics2D.IgnoreLayerCollision(19, 9);
            Physics2D.IgnoreLayerCollision(19, 14);
            can_take_damage = false;
            stay_hide = true;
            float
                hide_time_frames = hide_time * 60,
                hide_frame = 0.55f / hide_time_frames;
            //muda a transparencia de acordo com o contador
            sr_boss.color = new Color(1, 1, 1, 1 - hide_frame * hide_count);
            if (hide_count >= hide_time_frames)
                hide_enable = false;
            hide_count++;
        }
        else
            hide_count = 0;
    }

    void Show()
    {

        if (show_enable)
        {
            //ativa a colisão com player e balas
            stay_hide = false;
            Physics2D.IgnoreLayerCollision(19, 9, false);
            Physics2D.IgnoreLayerCollision(19, 14, false);
            can_take_damage = true;
            float
                show_time_frames = show_time * 60,
                show_frame = 0.55f / show_time_frames;
            //muda a transparencia de acordo com o contador
            sr_boss.color = new Color(1, 1, 1, sr_boss.color.a + (1 - show_frame * hide_count));
            if (show_count >= show_time_frames)
                show_enable = false;
            show_count++;
        }
        else
            show_count = 0;
    }

    void Shake()
    {
        //faz o Objeto chaqualhar-se com a função de seno
        if(shake_on)
        {
            transform.position = (Vector2)transform.position - new Vector2(Mathf.Sin(Time.time * 100f), velocity_y);
        }
    }

    void Teleport()
    {
        if (start_teleport)
        {
            //ignora a colisão
            Physics2D.IgnoreLayerCollision(19, 9);
            Physics2D.IgnoreLayerCollision(19, 14);
            can_take_damage = false;
            //converte tempo de segundos para frames
            float
                disapperance_time_frames = disappearance_time * 60;

            //transparece o Boss de acorco com o tempo
            sr_boss.color = new Color(1, 1, 1, (disapperance_time_frames - teleport_count) / disapperance_time_frames);

            //quando o tempo acaba, desativa o inicio do teleport, teleporta e começa o inicio da placa
            if (teleport_count >= disapperance_time_frames)
            {
                float_enable = false;
                transform.position = teleport_position;
                start_teleport = false;
                warning_sign_appear = true;
            }
            teleport_count++;
        }
        else
            teleport_count = 0;

        if (warning_sign_appear)
        {
            //converte tempo de segundos para frames
            float
                warn_sign_time_frames = warning_sign_time * 60;

            if (warning_sign_count <= (int)warn_sign_time_frames / 2)
                //faz a placa aparecer na metade dos frames
                sr_warning_sign.color = new Color(1, 1, 1, warning_sign_count / warn_sign_time_frames / 2);
            else
            {
                //faz a placa transparecer na outra metade
                float desappear_half_frames = (warning_sign_count % ((warn_sign_time_frames / 2) + 1) + 1);
                sr_warning_sign.color = new Color(1, 1, 1, ((warn_sign_time_frames / 2 - desappear_half_frames) / (warn_sign_time_frames / 2)));
            }

            //desativa a placa e começa o aparecimento do Boss
            if (warning_sign_count >= warn_sign_time_frames)
            {
                warning_sign_appear = false;
                appear_start = true;
            }
            warning_sign_count++;
        }
        else
            warning_sign_count = 0;

        if (appear_start)
        {
            //converte tempo de segundos para frames
            float
                appear_time_frames = appear_time * 60;

            //faz o Boss aparecer de acordo com os frames
            sr_boss.color = new Color(1, 1, 1, appear_count / appear_time_frames);

            //desativa o teleport e começa icinia a flutuar
            if (appear_count >= appear_time_frames)
            {
                appear_start = false;
                can_take_damage = true;
                Physics2D.IgnoreLayerCollision(19, 9, false);
                Physics2D.IgnoreLayerCollision(19, 14, false);
            }
            appear_count++;
        }
        else
            appear_count = 0;
    }

    Vector2 RandomPosition()
    {
        //retorna uma posião aleatoria
        return new Vector2(Random.Range(max_vertex.x, min_vertex.x), Random.Range(max_vertex.y, min_vertex.y));
    }

    float velocityToMove(float position1, float position2, float time)
    {
        //retorna uma velocidade com a distacia entre dois pontos e o tempo percorrido
        float community_distance;
        community_distance = position1 - position2;
        return community_distance / time;
    }

    void ActiveAction(bool action)
    {
        //ativa uma ação
        action = true;
    }

}