using UnityEngine;

public class MeleWeapons : MonoBehaviour
{
    public int danio = 10; // Daño del arma
    public float alcance = 2f; // Alcance del ataque
    public LayerMask capaEnemigos; // Capa de los enemigos

    private Animator animador;

    private void Start()
    {
        animador = GetComponent<Animator>(); // Obtener el Animator
    }

    private void Update()
    {
        DetectarEnemigos();
    }

    private void DetectarEnemigos()
    {
        Collider2D[] enemigosEnRango = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos);

        foreach (Collider2D enemigo in enemigosEnRango)
        {
            Health salud = enemigo.GetComponent<Health>();
            if (salud != null)
            {
                salud.RecibirDaño(danio);
                if (animador != null)
                {
                    animador.SetTrigger("Atacar");
                }
            }
        }
    }
}
