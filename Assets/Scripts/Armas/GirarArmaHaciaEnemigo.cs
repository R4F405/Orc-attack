using UnityEngine;

/// <summary>
/// Controla la rotación y orientación del arma hacia el enemigo más cercano.
/// </summary>
/// <remarks>
/// Esta clase se encarga de detectar enemigos dentro de un rango específico
/// y girar el arma para que apunte hacia el enemigo más cercano. Tiene un
/// comportamiento especial para las armas tipo "pincha" (lanzas) y gestiona
/// la inversión de escala según la dirección del enemigo.
/// </remarks>
public class GirarArmaHaciaEnemigo : MonoBehaviour
{
    /// <summary>
    /// Radio en unidades del mundo en el que se detectarán enemigos.
    /// </summary>
    public float rangoDeteccion = 5f;
    
    /// <summary>
    /// Capa de colisión que define qué objetos son considerados enemigos.
    /// </summary>
    public LayerMask capaEnemigos;
    
    /// <summary>
    /// Indica si el arma es de tipo lanza, lo que cambia su comportamiento de rotación.
    /// </summary>
    public bool pincha;

    /// <summary>
    /// Rotación original del arma cuando no apunta a ningún enemigo.
    /// </summary>
    private Quaternion rotacionInicial;
    
    /// <summary>
    /// Escala original del arma cuando no apunta a ningún enemigo.
    /// </summary>
    private Vector3 escalaInicial;
    
    /// <summary>
    /// Componente Animator para controlar las animaciones del arma.
    /// </summary>
    private Animator animador;
    
    /// <summary>
    /// Indica si el arma está actualmente ejecutando un ataque.
    /// </summary>
    private bool atacando = false;

    /// <summary>
    /// Inicializa las referencias de componentes y guarda la rotación y escala iniciales.
    /// </summary>
    private void Start()
    {
        rotacionInicial = transform.rotation;
        escalaInicial = transform.localScale;
        animador = GetComponent<Animator>();
    }

    /// <summary>
    /// Actualiza la rotación del arma para que apunte al enemigo más cercano.
    /// Se ejecuta a intervalos fijos de tiempo.
    /// </summary>
    private void FixedUpdate()
    {
        if (atacando) return; // No girar si está atacando
         
        // Si la animación de ataque está activa, no giramos el arma
        if (animador != null && animador.GetCurrentAnimatorStateInfo(0).IsName("Atacar"))
        {
            return;
        }

        GirarHaciaEnemigo();
    }

    /// <summary>
    /// Detecta el enemigo más cercano y gira el arma para que apunte hacia él.
    /// Si no hay enemigos cerca, el arma vuelve a su posición original.
    /// </summary>
    private void GirarHaciaEnemigo()
    {
        Collider2D[] enemigosDetectados = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion, capaEnemigos);

        if (enemigosDetectados.Length == 0)
        {
            if (pincha)
            {
                // Si es una lanza y no hay enemigos, vuelve a su posición original
                transform.rotation = rotacionInicial;
                transform.localScale = escalaInicial;
            }
            else
            {
                // Si no es lanza, se voltea según la posición del jugador
                if (transform.position.x < GameObject.FindWithTag("Jugador").transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    transform.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    transform.rotation = rotacionInicial;
                    transform.localScale = escalaInicial;
                }
            }
            return;
        }

        Transform enemigoMasCercano = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (Collider2D enemigo in enemigosDetectados)
        {
            float distancia = Vector2.Distance(transform.position, enemigo.transform.position);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                enemigoMasCercano = enemigo.transform;
            }
        }

        if (enemigoMasCercano != null)
        {
            Vector2 direccion = (enemigoMasCercano.position - transform.position).normalized;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            if (pincha)
            {
                angulo -= 90f; // Ajuste para que la parte superior apunte al enemigo
            }

            transform.rotation = Quaternion.Euler(0f, 0f, angulo);

            // Solo invertimos la escala si NO es una lanza
            if (!pincha)
            {
                if (enemigoMasCercano.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    /// <summary>
    /// Configura el estado de ataque del arma. Cuando está atacando,
    /// el arma no rota hacia nuevos enemigos.
    /// </summary>
    /// <param name="estado">True si el arma está atacando, false en caso contrario.</param>
    public void SetAtacando(bool estado)
    {
        atacando = estado;
    }
}
