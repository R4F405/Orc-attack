using UnityEngine;

public class ArmasMelee : MonoBehaviour
{
    public int danio = 10; // Daño del arma
    public float alcance = 2f; // Alcance del ataque
    public float recarga = 1f; // Tiempo de recarga entre ataques
    public LayerMask capaEnemigos; // Capa de los enemigos

    private Animator animador;
    private float tiempoSiguienteAtaque = 0f;  // Tiempo hasta el próximo ataque permitido
    private bool atacando = false; // Controla si el ataque está en curso

    private void Start()
    {
        animador = GetComponent<Animator>(); // Obtener el Animator
    }

    private void Update()
    {
        if (!atacando && Time.time >= tiempoSiguienteAtaque)
        {
            DetectarEnemigos();
        }
    }

    private void DetectarEnemigos()
    {
        Collider2D[] enemigosEnRango = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        if (enemigosEnRango.Length > 0)
        {
            atacando = true; // Marcar que el ataque ha comenzado
            
            if (animador != null)
            {
                animador.SetTrigger("Atacar"); // Activar animación
            }

            // Esperar un poco para que el ataque se sincronice con la animación
            Invoke("AplicarDaño", 0.1f); 

            // Establecer el tiempo para el siguiente ataque
            tiempoSiguienteAtaque = Time.time + recarga;
        }
    }

    private void AplicarDaño()
    {
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            VidaEnemigo salud = enemigo.GetComponent<VidaEnemigo>();
            if (salud != null)
            {
                salud.RecibirDaño(danio); // Aplicar daño
            }
        }

        // Finalizar el ataque
        atacando = false;
    }
}
