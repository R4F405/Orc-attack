using UnityEngine;

public class GirarArmaHaciaEnemigo : MonoBehaviour
{
    public float rangoDeteccion = 5f; // Rango para detectar al enemigo más cercano
    public LayerMask capaEnemigos; // Capa de los enemigos

    private Quaternion rotacionInicial; // Guarda la rotación inicial
    private Vector3 escalaInicial; // Guarda la escala inicial

    private void Start()
    {
        rotacionInicial = transform.rotation; // Guardamos la rotación inicial
        escalaInicial = transform.localScale; // Guardamos la escala inicial
    }

    private void Update()
    {
        GirarHaciaEnemigo();
    }

    private void GirarHaciaEnemigo()
    {
        Collider2D[] enemigosDetectados = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion, capaEnemigos);

        if (enemigosDetectados.Length == 0) 
        {
            // Si no hay enemigos, restauramos la posición inicial
            // Comparamos la posición de la arma con la posición del jugador
            if (transform.position.x < GameObject.FindWithTag("Jugador").transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180f); // Mira a la izquierda
                transform.localScale = new Vector3(1, -1, 1); // Escala negativa para voltear horizontalmente
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

            // Aplicamos rotación
            transform.rotation = Quaternion.Euler(0f, 0f, angulo);

            // Si el enemigo está a la izquierda, volteamos el sprite horizontalmente
            if (enemigoMasCercano.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, -1, 1); // Invierte en eje Y
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1); // Mantiene normal
            }
        }
    }
}
