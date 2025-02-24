using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    private int calaverasRecolectadas = 0; // Contador de calaveras

    private void Update()
    {
        RecogerCalaverasCercanas();
    }

    private void RecogerCalaverasCercanas()
    {
        // Detecta las calaveras dentro del rango de recolección
        Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, 2f, LayerMask.GetMask("Calavera"));

        foreach (Collider2D objeto in objetos)
        {
            RecogerObjeto recolector = objeto.GetComponent<RecogerObjeto>();
            if (recolector != null)
            {
                recolector.IntentarRecoger();
            }
        }
    }

    public void AgregarCalavera(int cantidad)
    {
        calaverasRecolectadas += cantidad;
        Debug.Log("Calaveras recogidas: " + calaverasRecolectadas);
    }

    public int ObtenerCantidadCalaveras()
    {
        return calaverasRecolectadas;
    }
}
