using UnityEngine;

public class Bala : MonoBehaviour
{
    private LayerMask capaEnemigos; 
    private int danio;
    private Transform objetivo;
    private Rigidbody2D rb;
    private float velocidad; // Ahora la velocidad se configura desde ArmasDistancia

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Destruir la bala después de 5 segundos si no impacta con nada
        Destroy(gameObject, 3f);
    }

    public void ConfigurarBala(int nuevoDaño, float nuevaVelocidad, LayerMask nuevaCapaEnemigos, Collider2D colliderJugador, Transform enemigo)
    {
        danio = nuevoDaño;
        velocidad = nuevaVelocidad;
        capaEnemigos = nuevaCapaEnemigos;
        objetivo = enemigo;

        // Ignorar colisión con el jugador
        Collider2D miCollider = GetComponent<Collider2D>();
        if (miCollider != null && colliderJugador != null)
        {
            Physics2D.IgnoreCollision(miCollider, colliderJugador);
        }
    }

    private void Update()
    {
        if (objetivo != null)
        {
            // Calcular la dirección hacia el enemigo
            Vector2 direccion = ((Vector2)objetivo.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = direccion * velocidad;
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (((1 << otro.gameObject.layer) & capaEnemigos) != 0)
        {
            Vida salud = otro.GetComponent<Vida>();
            if (salud != null)
            {
                salud.RecibirDaño(danio);
            }
            Destroy(gameObject); // Destruir la bala al impactar con un enemigo
        }
    }
}
