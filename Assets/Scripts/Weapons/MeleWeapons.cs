using UnityEngine;

public class MeleWeapons : MonoBehaviour
{
    // Daño que realiza el arma
    public int danio = 10;

    // Método que se ejecuta cuando el arma entra en colisión con un objeto
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Verificamos si el objeto con el que colisionó tiene el tag de "Enemigo"
        if (col.CompareTag("Enemy"))
        {
            // Intentamos acceder al componente de salud del enemigo y aplicar el daño
            Health salud = col.GetComponent<Health>();
            if (salud != null)
            {
                salud.RecibirDaño(danio);
            }
        }
    }
}
