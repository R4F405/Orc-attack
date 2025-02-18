using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    private int objetosRecolectados = 0; // Contador de objetos

    private void Update()
    {
        RecogerObjetosCercanos();
    }

    private void RecogerObjetosCercanos()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, 2f, LayerMask.GetMask("ObjetoDropeado"));

        foreach (Collider2D objeto in objetos)
        {
            RecogerObjeto recolector = objeto.GetComponent<RecogerObjeto>();
            if (recolector != null)
            {
                recolector.IntentarRecoger();
            }
        }
    }

    public void AgregarObjeto(int cantidad)
    {
        objetosRecolectados += cantidad;
        Debug.Log("Objetos recogidos: " + objetosRecolectados);
    }
}
