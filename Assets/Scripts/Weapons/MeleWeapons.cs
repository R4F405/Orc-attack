using UnityEngine;

public class MeleWeapons : MonoBehaviour
{
    public int danio = 10; // Daño que realiza el arma
    public float alcance = 2f; // Alcance del arma, en unidades de distancia
    public LayerMask capaEnemigos; // Capa de los enemigos (asegúrate de que los enemigos estén en esta capa)
    
    private Animator animador;

    private void Start()
    {
        animador = GetComponent<Animator>(); // Obtener el Animator del arma
    }

    private void Update()
    {
        // Detectar enemigos dentro del alcance del arma
        DetectarEnemigos();
    }

    private void DetectarEnemigos()
    {
        // Crear un círculo en la posición del arma para detectar enemigos dentro del rango
        Collider2D[] enemigosEnRango = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        foreach (Collider2D enemigo in enemigosEnRango)
        {
            // Intentamos acceder al componente de salud del enemigo y aplicar el daño
            Health salud = enemigo.GetComponent<Health>();
            if (salud != null)
            {
                salud.RecibirDaño(danio);

                if (animador != null)
                {
                    animador.SetTrigger("Atacar");
                    Debug.Log("Si realiza el trigger");
                }
            }
        }
    }

}
