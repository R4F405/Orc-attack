using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gestiona la posición de las armas alrededor del jugador.
/// Lee las armas desde InventarioArmas y las reconstruye cuando el inventario cambia.
/// </summary>
public class PosicionarArmasJugador : MonoBehaviour
{
    [Header("Configuración de Posicionamiento")]
    public float distanciaHorizontal = 1.5f;
    public float distanciaVertical = 1.5f;

    private GameObject[] armasInstanciadas;
    private InventarioArmas inventario;

    /// <summary>
    /// Propiedad de compatibilidad: número de armas actual.
    /// </summary>
    public int numeroDeArmas => inventario != null ? inventario.CantidadArmas : 0;

    private void Start()
    {
        inventario = InventarioArmas.instancia;

        if (inventario == null)
        {
            GameObject obj = new GameObject("InventarioArmas");
            inventario = obj.AddComponent<InventarioArmas>();
        }

        inventario.OnInventarioCambiado += ReconstruirArmas;
        ReconstruirArmas();
    }

    private void OnDestroy()
    {
        if (inventario != null)
        {
            inventario.OnInventarioCambiado -= ReconstruirArmas;
        }
    }

    /// <summary>
    /// Destruye todas las armas actuales y las recrea desde el inventario.
    /// Se ejecuta automáticamente al cambiar el inventario.
    /// </summary>
    public void ReconstruirArmas()
    {
        // Destruir armas actuales
        if (armasInstanciadas != null)
        {
            foreach (GameObject arma in armasInstanciadas)
            {
                if (arma != null) Destroy(arma);
            }
        }

        // Destruir armas huérfanas por seguridad
        GameObject[] armasEnCapa = GameObject.FindGameObjectsWithTag("Armas");
        foreach (var arma in armasEnCapa)
        {
            Destroy(arma);
        }

        // Asegurar que GestorMejorasArmas existe
        if (GestorMejorasArmas.instancia == null)
        {
            GameObject obj = new GameObject("GestorMejorasArmas");
            obj.AddComponent<GestorMejorasArmas>();
        }

        // Recrear desde el inventario
        List<ArmaInstancia> armasInventario = inventario.ObtenerArmas();
        armasInstanciadas = new GameObject[armasInventario.Count];
        Vector3[] posiciones = ObtenerPosiciones(armasInventario.Count);

        for (int i = 0; i < armasInventario.Count; i++)
        {
            ArmaInstancia instancia = armasInventario[i];
            if (instancia.datos == null || instancia.datos.prefab == null) continue;

            GameObject armaObj = Instantiate(instancia.datos.prefab, transform.position + posiciones[i], Quaternion.identity);
            armasInstanciadas[i] = armaObj;

            // Inicializar el arma con los datos de la instancia (nivel, stats)
            ArmasMelee melee = armaObj.GetComponent<ArmasMelee>();
            ArmasDistancia distancia = armaObj.GetComponent<ArmasDistancia>();

            if (melee != null) melee.Inicializar(instancia);
            if (distancia != null) distancia.Inicializar(instancia);
        }
    }

    private void Update()
    {
        if (armasInstanciadas == null) return;

        Vector3[] posiciones = ObtenerPosiciones(armasInstanciadas.Length);

        for (int i = 0; i < armasInstanciadas.Length; i++)
        {
            if (armasInstanciadas[i] != null && i < posiciones.Length)
            {
                armasInstanciadas[i].transform.position = transform.position + posiciones[i];
            }
        }
    }

    /// <summary>
    /// Calcula las posiciones relativas de las armas según la cantidad.
    /// </summary>
    private Vector3[] ObtenerPosiciones(int cantidad)
    {
        float h = distanciaHorizontal;
        float v = distanciaVertical;

        return cantidad switch
        {
            1 => new[] { new Vector3(-h, 0, 0) },
            2 => new[] { new Vector3(-h, 0, 0), new Vector3(h, 0, 0) },
            3 => new[] { new Vector3(-h, 0, 0), new Vector3(h, 0, 0), new Vector3(0, v * 2f, 0) },
            4 => new[] { new Vector3(-h, -v, 0), new Vector3(h, -v, 0), new Vector3(-h, v, 0), new Vector3(h, v, 0) },
            5 => new[] { new Vector3(-h, -v, 0), new Vector3(h, -v, 0), new Vector3(-h, v, 0), new Vector3(h, v, 0), new Vector3(0, v * 2f, 0) },
            _ => new Vector3[0]
        };
    }

    /// <summary>
    /// Obtiene la posición actual de un arma específica en el mundo.
    /// </summary>
    public Vector3 ObtenerPosicionActualDelArma(GameObject arma)
    {
        if (armasInstanciadas == null) return arma.transform.position;

        Vector3[] posiciones = ObtenerPosiciones(armasInstanciadas.Length);
        for (int i = 0; i < armasInstanciadas.Length; i++)
        {
            if (armasInstanciadas[i] == arma && i < posiciones.Length)
            {
                return transform.position + posiciones[i];
            }
        }
        return arma.transform.position;
    }

    /// <summary>
    /// Devuelve las instancias actuales de armas (GameObjects en escena).
    /// </summary>
    public GameObject[] ObtenerArmasActuales()
    {
        return armasInstanciadas ?? new GameObject[0];
    }
}