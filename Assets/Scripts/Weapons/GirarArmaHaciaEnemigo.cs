using UnityEngine;

public class GirarArmaHaciaEnemigo : MonoBehaviour
{
    public float rangoDeteccion = 5f; // Rango para detectar al enemigo más cercano
    public LayerMask capaEnemigos; // Capa de los enemigos


     private void Update()
    {
        GirarHaciaEnemigo();
    }

    private void GirarHaciaEnemigo()
    {
        Collider2D[] enemigosDetectados = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion, capaEnemigos);

        if (enemigosDetectados.Length == 0) return;

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
