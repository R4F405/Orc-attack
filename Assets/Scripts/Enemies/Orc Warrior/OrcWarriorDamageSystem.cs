using UnityEngine;

public class OrcWarriorDamageSystem : MonoBehaviour
{
    public float daño = 10f;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Health saludJugador = col.gameObject.GetComponent<Health>();
            if (saludJugador != null)
            {
                saludJugador.RecibirDaño(daño);
            }
        }
    }
}
