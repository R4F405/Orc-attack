using UnityEngine;

public class Bala : MonoBehaviour
{
    private LayerMask capaEnemigos;
    private LayerMask capaCajas;
    private int danio;
    private Transform objetivo;
    private Rigidbody2D rb;
    private float velocidad;
    private VidaJugador vidaJugador;
    private GameObject jugador;
    private int probabilidadRobarVida = 5;
    
    private void Start()
    {
        ObtenerJugador();
        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }

        rb = GetComponent<Rigidbody2D>();
        
        // Destruir la bala después de x segundos si no impacta con nada
        Destroy(gameObject, 3f);

        // Ignorar colisión entre balas
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Balas"), LayerMask.NameToLayer("Balas"));
    }

    public void ConfigurarBala(int nuevoDaño, float nuevaVelocidad, LayerMask nuevaCapaEnemigos, LayerMask nuevaCapaCajas, Collider2D colliderJugador, Transform enemigo, int nuevaProbabilidad)
    {
        danio = nuevoDaño;
        velocidad = nuevaVelocidad;
        capaEnemigos = nuevaCapaEnemigos;
        capaCajas = nuevaCapaCajas;
        objetivo = enemigo;
        probabilidadRobarVida = nuevaProbabilidad;

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
        int capaOtro = 1 << otro.gameObject.layer;

        if ((capaOtro & capaEnemigos) != 0)
        {
            VidaEnemigo salud = otro.GetComponent<VidaEnemigo>();
            if (salud != null)
            {
                salud.RecibirDaño(danio);
                RobarVida();
            }
            Destroy(gameObject); // Destruir la bala al impactar con un enemigo
        }
        else if ((capaOtro & capaCajas) != 0)
        {
            RomperCaja caja = otro.GetComponent<RomperCaja>();
            if (caja != null)
            {
                caja.RecibirGolpe();
            }
            Destroy(gameObject); // La bala también desaparece al impactar la caja
        }
    }

    private void RobarVida() 
    {
        // Genera un número aleatorio entre 0 y 100
        int probabilidad = Random.Range(0, 100);
        // Si el número aleatorio es menor que la probabilidad de robar vida, se cura
        if (probabilidad < probabilidadRobarVida)
        {
            if (vidaJugador != null)
            {
                vidaJugador.Curar(1); // Recupera 1 de vida
                Debug.Log("Vida Robada");
            }
        }    
    }

    private void ObtenerJugador()
    {
        // Intentar obtener el jugador con el tag 'Jugador'
         jugador = GameObject.FindWithTag("Jugador");

        if (jugador != null)
        {
            vidaJugador = jugador.GetComponent<VidaJugador>();
        }
    }
}
