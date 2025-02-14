using UnityEngine;

public class ArmasMelee : MonoBehaviour
{
    public int danio = 10; // Daño del arma
    public float alcance = 2f; // Alcance del ataque
    public float recarga = 1f; // Tiempo de recarga entre ataques
    public LayerMask capaEnemigos; // Capa de los enemigos

    private Animator animador;
    private float tiempoSiguienteAtaque = 0f; // Tiempo hasta el próximo ataque permitido

    private void Start()
    {
        animador = GetComponent<Animator>(); // Obtener el Animator
    }

    private void Update()
    {
        // Verificar si se puede atacar
        if (Time.time >= tiempoSiguienteAtaque)
        {
            DetectarEnemigos();
        }
    }

    private void DetectarEnemigos()
    {
        Collider2D[] enemigosEnRango = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        foreach (Collider2D enemigo in enemigosEnRango)
        {
            Vida salud = enemigo.GetComponent<Vida>();
            if (salud != null)
            {
                salud.RecibirDaño(danio);
                
                if (animador != null)
                {
                    animador.SetTrigger("Atacar");
                }

                // Actualizar el tiempo del siguiente ataque permitido
                tiempoSiguienteAtaque = Time.time + recarga;
            }
        }
    }
}
