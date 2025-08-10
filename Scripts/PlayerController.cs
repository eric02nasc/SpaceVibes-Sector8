using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private FixedJoystick _joystick;

    public GameObject Player;
    public Animator animator;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    public float speed;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public TrailRenderer trail;
    public TrailRenderer trail2;
    public VisualEffect visualEffect;
    public VisualEffect visualEffect2;
    public bool IsShinning;
    public bool trailON;

    public Material [] shaders;
    public Material [] shaders2;
    public Color[] colors;

    public float spinForce;
    public float normalizeTime;
    private float currentSpinForce;
    private float spinTime;
    public float fatorChange;
    public Vector2 direction;
    public Color currentColor;
    public Color colorA;
    public Color colorB;
    public float velocidade;
    public bool particulasDeAtrito;

    public float rotationOffset = -90f;

    public float rotationSpeed = 5f; // Velocidade de rota��o da cabe�a
    public ParticleSystem particleSparks;
    public ParticleSystem particleStrokes;
   // public Vector3 strokeTransform;
    public ParticleSystemRenderer systemRenderer;

    private Vector2 lastVelocity;    // �ltima velocidade registrada do astronauta
    public Transform playerPos;

    private void Start()
    {
       // strokeTransform = particleStrokes.transform.position;
        particleStrokes.Stop();
        visualEffect2.Stop();
        particleSparks.Stop();
        playerPos = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        trailON = true;
        systemRenderer = particleSparks.GetComponent<ParticleSystemRenderer>();
        animator = Player.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)

            
        {
           // particleStrokes.gameObject.transform.parent = null;
            particleStrokes.gameObject.SetActive(false);
            particleStrokes.Stop();
            horizontalMove = _joystick.Horizontal * speed;
            verticalMove = _joystick.Vertical * speed;


            Vector3 movement = new Vector3(horizontalMove, verticalMove, 0f) * Time.deltaTime;
            rb.velocity = movement;
            //transform.position += movement;

            velocidade = rb.velocity.magnitude;
            if (trailON)
            {
                if (velocidade < 1f)
                {
                    trail.time = 10f;
                    trail.emitting = false;
                }
                else
                {
                    trail.emitting = true;
                    trail.time = 4.5f;

                }
            }
            if (particulasDeAtrito)
            {
                if (velocidade > 1.9f)
                {
                    particleSparks.Play();
                    visualEffect2.Play();
                }
                else
                {
                    particleSparks.Stop();
                    visualEffect2.Stop();
                }
                if (velocidade > 1.98f)
                {
                    visualEffect2.gameObject.SetActive(true);
                    visualEffect2.Play();
                }
                else
                {
                    visualEffect2.gameObject.SetActive(false);
                    visualEffect2.Stop();
                }
            }
            

            Vector3 localScale = transform.localScale;

            if (_joystick.Horizontal > 0)
            {
                spriteRenderer.flipX = true;
                systemRenderer.flip = localScale;
               // particleSparks.transform.rotation = Quaternion.Euler(0f, 0, 0f);
            }
            else
            {
                spriteRenderer.flipX = false;
               // particleSparks.transform.rotation = Quaternion.Euler(0f, 180, 0f);
            }
        }
        else if(rb.velocity.magnitude> 1.3f)
        {
            
           // particleStrokes.gameObject.transform.parent = this.gameObject.transform;
           // particleStrokes.gameObject.transform.position = strokeTransform;
            particleStrokes.gameObject.SetActive(true);
            particleStrokes.Play();
        }
       
        /* Verificar se houve uma mudan�a completa de dire��o
        if (HasDirectionChanged())
        {

            // Atualizar a for�a de rota��o para o valor m�ximo
            currentSpinForce = spinForce;
            // Resetar o tempo de rota��o
            spinTime = 0f;
        }

        // Verificar se o jogador est� girando
        if (currentSpinForce > 0f)
        {
            // Calcular a rota��o no eixo z
            float rotationZ = currentSpinForce * Time.deltaTime;
            transform.Rotate(0f, 0f, rotationZ);

            // Reduzir gradualmente a for�a de rota��o
            currentSpinForce = Mathf.Lerp(spinForce , 0f, spinTime / normalizeTime);
            // Atualizar o tempo de rota��o
            spinTime += Time.deltaTime;
        }*/

        if (rb.velocity != lastVelocity)
        {
            // Calcula a dire��o em que o astronauta est� se movendo
            Vector2 direction = rb.velocity.normalized;

            // Calcula o �ngulo para a nova dire��o
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;

            //Calcula o �ngulo atual da cabe�a
            //float currentAngle = playerPos.rotation.eulerAngles.z;

            //Calcula o �ngulo interpolado suavemente
            float newAngle = Mathf.LerpAngle(playerPos.rotation.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

            // Aplica a rota��o � cabe�a do astronauta
            playerPos.rotation = Quaternion.Euler(0f, 0f, newAngle);
        }

        // Atualiza a �ltima velocidade registrada
        lastVelocity = rb.velocity;
        animator.SetFloat("speed", lastVelocity.magnitude);


    }
    public void ChangeEffect()
    {
        visualEffect.SetVector4("Color", colors[Random.Range(0, colors.Length )]);
        trail.material = shaders[Random.Range(0, shaders.Length )];
        trail2.material = shaders2[Random.Range(0, shaders2.Length )];
        
    }

    public void setAtrito()
    {
        if(particulasDeAtrito)
        {
            particulasDeAtrito = false;
            particleSparks.Stop();
            visualEffect2.Stop();
        }
        else
        {
            particulasDeAtrito = true; 
        }
    }
    public void ControlTrail()
    {
        if (IsShinning)
        {
            trailON = false;
            IsShinning = false;
            trail.emitting = false;
            trail2.emitting = false;
            visualEffect.Stop();
        
        }
        else
        {
            trailON = true;
            IsShinning = true;
            trail.emitting = true;
            trail2.emitting = true;
            visualEffect.Play();
            
        }


    }


    private bool HasDirectionChanged()
    {
        Vector2 previousDirection = GetCurrentDirection(); // Obt�m a dire��o anterior
        Vector2 currentDirection = GetNewDirection(); // Obt�m a dire��o atual
        
        // Calcula o �ngulo entre as dire��es anterior e atual
        float angle = Vector2.Angle(previousDirection, currentDirection);

        // Define os limites de �ngulo para determinar a magnitude da mudan�a
        float angleThreshold = 30f; // Limite para uma mudan�a leve
        float angleThreshold2 = 90f; // Limite para uma mudan�a moderada
        float angleThreshold3 = 180f; // Limite para uma mudan�a dr�stica

        // Define os valores m�nimo e m�ximo para a changeForce
        float changeForceMin = 1f;
        float changeForceMax = 4f;

        // Verifica o �ngulo e atribui a changeForce com base na magnitude da mudan�a de dire��o
        float changeForce = changeForceMin;
        if (angle > angleThreshold && angle <= angleThreshold2)
        {
            changeForce = Mathf.Lerp(changeForceMin, changeForceMax, (angle - angleThreshold) / (angleThreshold2 - angleThreshold));
        }
        else if (angle > angleThreshold2 && angle <= angleThreshold3)
        {
            changeForce = changeForceMax;
        }
        fatorChange = changeForce;
        // Retorne true se houver uma mudan�a completa de dire��o, caso contr�rio, retorne false
        return changeForce > 0;
    }


    private Vector2 GetCurrentDirection()
    {
        // Implemente a l�gica para obter a dire��o atual do jogador
        // Isso depende do seu jogo e de como a dire��o do jogador � atualizada

        // Exemplo de implementa��o:

        direction = rb.velocity.normalized;

       
        return direction;

        // return Vector2.zero; // Altere para a sua implementa��o real
    }

    private Vector2 GetNewDirection()
    {
        // Implemente a l�gica para obter a nova dire��o do jogador
        // Isso tamb�m depende do seu jogo e de como a dire��o do jogador � atualizada

        // Exemplo de implementa��o:
         Vector2 input = new Vector2(_joystick.Horizontal, _joystick.Vertical);
       // Debug.Log("GetNewDirection() : " + input.normalized);
        return input.normalized;

       // return Vector2.zero; // Altere para a sua implementa��o real
    }
}
