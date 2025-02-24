using UnityEngine;

public class GirarArmaHaciaEnemigo : MonoBehaviour
{
    public float rangoDeteccion = 5f;
    public LayerMask capaEnemigos;
    public bool pincha; // Si es una lanza

    private Quaternion rotacionInicial;
    private Vector3 escalaInicial;
    private Animator animador;
    private bool atacando = false;

    private void Start()
    {
        rotacionInicial = transform.rotation;
        escalaInicial = transform.localScale;
        animador = GetComponent<Animator>(); // Obtener el Animator del arma
    }

    private void Update()
    {
         if (atacando) return; // No girar si está atacando
         
        // Si la animación de ataque está activa, no giramos el arma
        if (animador != null && animador.GetCurrentAnimatorStateInfo(0).IsName("Atacar"))
        {
            return;
        }

       //GirarHaciaCaja();
        GirarHaciaEnemigo();

    }

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

    public void SetAtacando(bool estado)
    {
        atacando = estado;
    }
}
