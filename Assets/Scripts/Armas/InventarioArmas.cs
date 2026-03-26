using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Singleton persistente que gestiona el inventario de armas del jugador.
/// Permite agregar, eliminar y fusionar (mejorar) armas.
/// </summary>
public class InventarioArmas : MonoBehaviour
{
    public static InventarioArmas instancia;

    [Header("Configuración")]
    public int maxArmas = 5;

    [Header("Armas Iniciales (solo para la primera partida)")]
    public DatosArma[] armasIniciales;

    private List<ArmaInstancia> armas = new List<ArmaInstancia>();

    /// <summary>
    /// Evento disparado cada vez que el inventario cambia (agregar, eliminar, mejorar).
    /// PosicionarArmasJugador escucha este evento para reconstruir las armas.
    /// </summary>
    public event System.Action OnInventarioCambiado;

    public int CantidadArmas => armas.Count;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            InicializarArmasIniciales();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InicializarArmasIniciales()
    {
        if (armasIniciales == null || armas.Count > 0) return;

        foreach (DatosArma datos in armasIniciales)
        {
            if (datos != null)
            {
                armas.Add(new ArmaInstancia(datos, 1));
            }
        }
    }

    /// <summary>
    /// Devuelve una copia de la lista de armas actual.
    /// </summary>
    public List<ArmaInstancia> ObtenerArmas()
    {
        return new List<ArmaInstancia>(armas);
    }

    /// <summary>
    /// Obtiene un arma por su índice en el inventario.
    /// </summary>
    public ArmaInstancia ObtenerArma(int indice)
    {
        if (indice < 0 || indice >= armas.Count) return null;
        return armas[indice];
    }

    public bool PuedeAgregarArma()
    {
        return armas.Count < maxArmas;
    }

    /// <summary>
    /// Agrega una nueva arma al inventario con el nivel especificado.
    /// </summary>
    public bool AgregarArma(DatosArma datos, int nivel = 1)
    {
        if (!PuedeAgregarArma() || datos == null) return false;

        armas.Add(new ArmaInstancia(datos, nivel));
        OnInventarioCambiado?.Invoke();
        return true;
    }

    /// <summary>
    /// Elimina un arma del inventario por su índice.
    /// </summary>
    public bool EliminarArma(int indice)
    {
        if (indice < 0 || indice >= armas.Count) return false;

        armas.RemoveAt(indice);
        OnInventarioCambiado?.Invoke();
        return true;
    }

    /// <summary>
    /// Busca una pareja fusionable: misma arma y mismo nivel, distinta al índice dado.
    /// Devuelve el índice de la pareja o -1 si no existe.
    /// </summary>
    public int BuscarParejaParaMejora(int indice)
    {
        if (indice < 0 || indice >= armas.Count) return -1;

        ArmaInstancia arma = armas[indice];
        if (!arma.PuedeMejorar) return -1;

        for (int i = 0; i < armas.Count; i++)
        {
            if (i != indice && arma.MismoTipoYNivel(armas[i]))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Fusiona el arma en el índice dado con su pareja del mismo tipo y nivel.
    /// La arma seleccionada sube de nivel y la pareja se elimina.
    /// </summary>
    public bool MejorarArma(int indice)
    {
        int indicePar = BuscarParejaParaMejora(indice);
        if (indicePar == -1) return false;

        // Subir nivel del arma seleccionada
        armas[indice].nivel++;

        // Eliminar la pareja
        armas.RemoveAt(indicePar);

        OnInventarioCambiado?.Invoke();
        return true;
    }
}
