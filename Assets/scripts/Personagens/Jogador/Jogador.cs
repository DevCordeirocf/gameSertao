using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Jogador : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    private Rigidbody2D rb;
    private Vector2 direcao;
    [SerializeField] private Animator anim;

    [Header("Defesa e Lamparina")]
    public Light2D luzLamparina;
    public float intensidadeMaxima = 1.0f;
    public float velocidadeSegura = 2.0f; 
    public float velocidadeDeApagar = 0.5f;
    public float velocidadeDeRecuperar = 0.2f;
    public bool estaDefendendo = false;
    public bool ventoVindoDaDireita = true;

    [Header("Input e Audio")]
    public InputActionReference inputDefender;

    private void OnEnable() { inputDefender.action.Enable(); }
    private void OnDisable() { inputDefender.action.Disable(); }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // Garante que a luz comece na intensidade certa
        if (luzLamparina != null) luzLamparina.intensity = intensidadeMaxima;
    }

    public void Mover(InputAction.CallbackContext context)
    {
        direcao = context.ReadValue<Vector2>();
    }
    void MoveAnim()
    {
        anim.SetFloat("HorizontalAnim", rb.linearVelocity.x);
    }

    private void FixedUpdate()
    {
        float velocidadeAtual = velocidade;

        if(estaDefendendo)
        {
            velocidadeAtual = velocidade * 0.5f;
            Debug.Log("Defendendo: Velocidade reduzida para " + velocidadeAtual);
        }

        rb.linearVelocity = new Vector2(direcao.x * velocidadeAtual, rb.linearVelocity.y);

        MoveAnim();
        // 1. Controle de Defesa
        if (inputDefender.action.IsPressed())
        {
            AtivarDefesa();
        }
        else 
        {
            estaDefendendo = false;
        }

        anim.SetBool("Defendendo", estaDefendendo);

        // 2. Movimentação Física
        if (!estaDefendendo)
        {
            rb.linearVelocity = new Vector2(direcao.x * velocidade, rb.linearVelocity.y);

            if (direcao.x > 0 && transform.localScale.x < 0)
                flip();
            else if (direcao.x < 0 && transform.localScale.x > 0)
                flip();
        }

        // 3. Mecânica da Lamparina
        ControlarChama();
    }

    public void AtivarDefesa()
    {
        estaDefendendo = true;
        
        // Vira o personagem para encarar o vento
        if (ventoVindoDaDireita && transform.localScale.x < 0) 
            flip(); // Vira pra direita
        else if (!ventoVindoDaDireita && transform.localScale.x > 0)
            flip(); // Vira pra esquerda
    }

    void ControlarChama()
    {
        if(luzLamparina == null) return;

        if(luzLamparina.intensity <= 0)
        {
            Debug.Log("A LAMPARINA APAGOU!");
            return;
        }

        float velocidadeAtual = rb.linearVelocity.magnitude;

        if (velocidadeAtual > velocidadeSegura && !estaDefendendo)
        {
            luzLamparina.intensity -= Time.deltaTime * velocidadeDeApagar;
            Tremeluzir(0.1f);
        }
        else
        {
            if (luzLamparina.intensity < intensidadeMaxima)
            {
                luzLamparina.intensity += Time.deltaTime * velocidadeDeRecuperar;
            }
            Tremeluzir(0.02f);
        }
    }

    void Tremeluzir(float forca)
    {
        float ruido = Random.Range(-forca, forca);
        luzLamparina.intensity += ruido;
    }

    private void flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
    }

}