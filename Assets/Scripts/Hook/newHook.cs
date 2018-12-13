using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class newHook : MonoBehaviour
{

    //Variaveis Unity ---
	[Header("Line Renderer")]
	public string sortingLayer;
	public int sortingOrder;

	[Header("Mechanics")]
    public float max_distance;
    public string hook_point_tag = "HookPoint";
    public LayerMask where_can_hook;
    public HookArrow arrow_script;
    public int animation_frames = 30;

    //---

    //private float x_position_animation;
    
	private bool has_hook;
    private bool is_hooked;
    private GameObject arrow;
    private LineRenderer line;
    private DistanceJoint2D joint;
    private InputHandler input;
    private PlayerController player;
    

    private List<Vector3> all_points = new List<Vector3>(); //Recebe todos os pontos que o hook ativa


    private GameObject collider_hit;

	void Start()
    {
        //Recebe o controle, os inputs e uma referencia para a direção do hook
        input = GetComponent<InputHandler>();
        player = GetComponent<PlayerController>();
        arrow = GameObject.FindGameObjectWithTag("HookArrow");

        //Recebe o DistanceJoint2D, principal objeto do hook, que conecta o player em algum outro objeto
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;

        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.sortingOrder = sortingOrder;
		line.sortingLayerName = sortingLayer;

		MemoryManager.OnSave += passHookToMemory;
	}

	void OnDestroy() {
		MemoryManager.OnSave -= passHookToMemory;
	}

	private void passHookToMemory(object sender, System.EventArgs e) {
		MemoryManager.Memory.HasHook = has_hook;
	}

    void Update()
    {
		if(!has_hook) {
			return;
		}
        //Se não existir algo na array all_points, adiciona a posição do player, se não, torna a primeira variavel Vector3 da array a posição do player
        if (all_points.Count == 0)
            all_points.Add(transform.position);
        else
            all_points[0] = transform.position;


    #region active_hook

        //Caso precione o botão de Hook e não esteja preso nem no chão
        if ((input.activityOf("Hook") || input.activityDown("Jump")) && !Is_hooked && !player.grounded)
        {

            //cria um raio de comprimento max_distance na direção do player ao arrow. Só recebera algo se atingir algum objeto que estaja em uma das layers em where_can_hook
            RaycastHit2D hit =
                Physics2D.Raycast(transform.position,
                arrow.transform.position - transform.position, max_distance, where_can_hook);
			hookRaycast = hit;

            //se o RaycastHit2D atingir algo, realiza a conexão
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                //insere um novo ponto ao line_render, insere o ponto atingido na array all_points, ativa o joint e muda o alvo de ponto no script HookArrow
                line.positionCount = 2;
                Is_hooked = true;
                joint.enabled = true;
                all_points.Add(connector(hit));
                arrow_script.change_target();

                collider_hit = hit.collider.gameObject;
            } else if (hit.collider == null)
            {
                line.positionCount = 2;
                //x_position_animation = transform.position.x;
                StartCoroutine(Animation());
            }

        }

        #endregion


    #region during_hook

        //caso esteja conectado
        else if(Is_hooked)
        {

        #region make_line

            //define a quantidade de pontos do line_render, torna sua posição 0 o primeiro ponto atingido e torna sua ultima posição na posição do player
            line.positionCount = all_points.Count;
            line.SetPosition(all_points.Count - 1, new Vector3(all_points[0].x, all_points[0].y + 1, 100));
            line.SetPosition(0, new Vector3(all_points[1].x, all_points[1].y, 100));


            //define as outras posições que o hook atingiu
            if(all_points.Count > 2) 

                for(int i = 1; i <= all_points.Count - 2; i ++)
                {
                    line.SetPosition(i, new Vector3(all_points[i + 1].x, all_points[i + 1].y, 100));
                }

        #endregion
            //cria um raycastHit2D para ver se o hook precisa ser "dobrado"
            RaycastHit2D hook_break =
                Physics2D.Raycast(transform.position, all_points[all_points.Count - 1] - transform.position, 
                Vector2.Distance(transform.position, all_points[all_points.Count - 1])- 0.002f,
                where_can_hook);
            //caso necessite, realiza uma nova conexão e adiciona um novo ponto na array all_points
            if (hook_break.collider != null && hook_break.collider.gameObject.GetComponent<Rigidbody2D>() != null && 
                (hook_break.collider.tag != hook_point_tag || hook_break.collider.transform.position != all_points[all_points.Count - 1]))
            {
                all_points.Add(connector(hook_break));

            }

            //Desativa o hook
            if(input.activityDown("Hook") || input.activityDown("Jump") || player.grounded)
            {

                //Reset nas variaveis 
                line.positionCount = 2;
                line.SetPosition(0, new Vector3(0, 0, 0));
                line.SetPosition(1, new Vector3(0, 0, 1));
                Is_hooked = false;
                joint.enabled = false;
                all_points = new List<Vector3>();
				hookRaycast = default(RaycastHit2D);

            }

        }

    #endregion
    }

    IEnumerator Animation()
    {
        for (int i = 1; i <= animation_frames/2; i++)
        {
            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, new Vector2(transform.position.x, transform.position.y + (max_distance / animation_frames) * i));
            yield return new WaitForFixedUpdate();
        }
        for (int i = animation_frames/2 ; i >= 1; i--)
        {
            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, new Vector2(transform.position.x, transform.position.y + (max_distance / animation_frames) * i));
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        line.positionCount = 0;
    }

    #region Builders

    private Vector2 position_of_hit(RaycastHit2D ray)
    {
        //realiza os calculos para retorna a posição em que o RaycastHit2D atingiu o objeto
        Vector2 connectPoint = ray.point - new Vector2(ray.collider.transform.position.x, ray.collider.transform.position.y);

        connectPoint.x = connectPoint.x / ray.collider.transform.localScale.x;
        connectPoint.y = connectPoint.y / ray.collider.transform.localScale.y;
        return connectPoint;
    }

    private Vector3 connector(RaycastHit2D hit)
    {
        //Checa se o objeto atingido é possui tag hook_point_tag
        if (hit.collider.tag.Equals(hook_point_tag))
        {
            //caso seja, conecta o player ao centro do objeto atingido e retorna assa posição
            connect_to_object(new Vector2(0, 0), hit.collider,
                Vector2.Distance(transform.position, hit.collider.transform.position) - 1);
            return hit.collider.transform.position;
        }
        else
        {

            //se nãom conecta ao ponto que atingiu o objeto, retornando essa posição
            connect_to_object(position_of_hit(hit), hit.collider,
                Vector2.Distance(transform.position, hit.point));
            return hit.point;
        }
    }

    private void connect_to_object(Vector2 anchor, Collider2D body, float distance)
    {
        //ajusta as propriedades do joint para conecta-lo do player ao objeto em uma certa distancia
        joint.connectedAnchor = anchor;
        joint.connectedBody = body.gameObject.GetComponent<Rigidbody2D>();
        joint.distance = distance;
    }

    public GameObject Collider_hit
    {
        get
        {
            return collider_hit;
        }

        set
        {
            collider_hit = value;
        }
    }

    public bool Is_hooked
    {
        get
        {
            return is_hooked;
        }

        set
        {
            is_hooked = value;
        }
    }

	public bool Has_hook {
		get { return has_hook; }
		set { has_hook = value; }
	}

	public RaycastHit2D hookRaycast { get; private set; }
	#endregion

}
