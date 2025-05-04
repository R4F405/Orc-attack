using UnityEngine;

/// <summary>
/// Gestiona el inventario y la recolección de recursos del jugador.
/// </summary>
/// <remarks>
/// Esta clase se encarga de detectar y recoger automáticamente los objetos cercanos,
/// gestionar la cantidad de calaveras recolectadas y aplicar multiplicadores.
/// </remarks>
public class InventarioJugador : MonoBehaviour
{
    /// <summary>
    /// Cantidad total de calaveras que el jugador ha recolectado.
    /// </summary>
    private int calaverasRecolectadas = 0; // Contador de calaveras
    
    /// <summary>
    /// Multiplicador que se aplica a las calaveras recolectadas.
    /// </summary>
    private int multiplicadorCalaveras = 1;

    /// <summary>
    /// Busca y recoge automáticamente las calaveras cercanas al jugador.
    /// </summary>
    private void Update()
    {
        RecogerCalaverasCercanas();
    }

    /// <summary>
    /// Detecta calaveras dentro de un radio alrededor del jugador y las recoge.
    /// </summary>
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

    /// <summary>
    /// Añade calaveras al inventario aplicando el multiplicador actual.
    /// </summary>
    /// <param name="cantidad">Cantidad base de calaveras a añadir.</param>
    public void AgregarCalavera(int cantidad)
    {
        calaverasRecolectadas += (cantidad * multiplicadorCalaveras);
    }

    /// <summary>
    /// Devuelve la cantidad total de calaveras en el inventario.
    /// </summary>
    /// <returns>Número de calaveras recolectadas.</returns>
    public int ObtenerCantidadCalaveras()
    {
        return calaverasRecolectadas;
    }

    /// <summary>
    /// Establece el multiplicador para la recolección de calaveras.
    /// </summary>
    /// <param name="multiplicador">Valor del multiplicador a aplicar.</param>
    /// <remarks>
    /// Si se pasa 0 como multiplicador, se establecerá a 1 por defecto.
    /// </remarks>
    public void MultiplicadorCalaveras (int multiplicador) 
    {
        if (multiplicador == 0) {
            multiplicador = 1;
        }
        multiplicadorCalaveras = 1 * multiplicador;
    }

    /// <summary>
    /// Reduce la cantidad de calaveras en el inventario.
    /// </summary>
    /// <param name="cantidad">Cantidad de calaveras a restar.</param>
    public void RestarCalaveras(int cantidad) 
    {
        calaverasRecolectadas -= cantidad;
    }
}
