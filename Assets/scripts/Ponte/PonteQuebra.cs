using UnityEngine;
using System.Collections;


public class PonteQuebrando : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [Header("Configurações")]
    public float tempoParaCair = 4.0f;
    public float forcaDoTremor = 0.05f;

    private bool estaCaindo = false;
    private Rigidbody2D rb;
    private Vector3 posicaoInicial;
    public bool PlayerPassou = false;


    void awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicaoInicial = transform.position;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !estaCaindo)
        {
            StartCoroutine(Cair());
        }
    }

    IEnumerator Cair()
    {
        estaCaindo = true;
        float timer = 0f;
        while (timer < tempoParaCair)
        {
            timer += Time.deltaTime;
            
            float x = Random.Range(-1f, 1f) * forcaDoTremor;
            float y = Random.Range(-1f, 1f) * forcaDoTremor;
            transform.position = posicaoInicial + new Vector3(x, y, 0);

            yield return null; 
        }
        anim.SetBool("Passou", true);


        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
    }
}
