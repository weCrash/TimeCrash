using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OpenWhenAllEnemysDie : MonoBehaviour {

    public List<GameObject> enemys;
    public float open_time = 2, open_distance;
    public Direction direction;
    float position_of_size;
    Vector2 direction_in_vector;
    Rigidbody2D rb;

    private void Start()
    {
        position_of_size = transform.position.x - transform.localScale.x / 2;
        rb = GetComponent<Rigidbody2D>();

        switch ((int)direction)
        {

            case 1:
                direction_in_vector = new Vector2(0, 1);
                break;
            case 2:
                direction_in_vector = new Vector2(1, 0);
                break;
            case 3:
                direction_in_vector = new Vector2(-1, 0);
                break;
            case 4:
                direction_in_vector = new Vector2(0, -1);
                break;
        }
        
    }

    // Update is called once per frame
    void Update () {

        List<GameObject> remove_enemys = new List<GameObject>();

        foreach (GameObject g in enemys) if(g == null) remove_enemys.Add(g);

        foreach (GameObject g in remove_enemys) enemys.Remove(g);

        if (enemys.Count <= 0) StartCoroutine(Open_Door());


    }

    IEnumerator Open_Door()
    {

        rb.velocity = direction_in_vector * open_distance;

        for(int i = 0; i <= open_time * 60; i ++)
        {
            transform.localScale = new Vector3(Mathf.Abs((transform.position.x - position_of_size)), transform.localScale.y);
            yield return new WaitForEndOfFrame();
        }


        rb.velocity = new Vector2(0, 0);
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        Destroy(this);

    }

    public enum Direction
    {
        Up = 1, Right = 2, Left = 3, Down = 4
    }
}
