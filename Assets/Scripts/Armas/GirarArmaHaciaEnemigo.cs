using UnityEngine;

public class GirarArmaHaciaEnemigo : MonoBehaviour
{
    public float rangoDeteccion = 5f;
    public LayerMask capaEnemigos;

    private Quaternion rotacionInicial;
    private Vector3 escalaInicial;
    private Animator animador;

    private void Start()
    {
        rotacionInicial = transform.rotation;
        escalaInicial = transform.localScale;
        animador = GetComponent<Animator>(); // Obtener el Animator del arma
    }

    private void Update()
    {
        // Si la animación de ataque está activa, no giramos el arma
        if (animador != null && animador.GetCurrentAnimatorStateInfo(0).IsName("Atacar"))
        {
            return;
        }

        GirarHaciaEnemigo();
    }

    private void GirarHaciaEnemigo()
    {
        Collider2D[] enemigosDetectados = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion, capaEnemigos);

        if (enemigosDetectados.Length == 0) 
        {
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

            transform.rotation = Quaternion.Euler(0f, 0f, angulo);

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
