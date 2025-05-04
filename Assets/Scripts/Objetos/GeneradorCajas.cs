using UnityEngine;

/// <summary>
/// Controla la generación automática de cajas en el juego.
/// </summary>
/// <remarks>
/// Esta clase gestiona la aparición periódica de cajas en ubicaciones aleatorias
/// dentro de un área definida, permitiendo ajustar la frecuencia de generación.
/// </remarks>
public class GeneradorCajas : MonoBehaviour
{
    /// <summary>
    /// Prefab de la caja a generar.
    /// </summary>
    public GameObject cajaPrefab;
    
    /// <summary>
    /// Coordenada inferior izquierda de la zona de generación.
    /// </summary>
    public Vector2 limiteInferior;
    
    /// <summary>
    /// Coordenada superior derecha de la zona de generación.
    /// </summary>
    public Vector2 limiteSuperior;
    
    /// <summary>
    /// Número de cajas a generar en cada oleada.
    /// </summary>
    public int cajasPorTiempo = 1;
    
    /// <summary>
    /// Intervalo de tiempo en segundos entre oleadas de cajas.
    /// </summary>
    public float tiempo = 10f;

    /// <summary>
    /// Temporizador para controlar cuándo generar la siguiente oleada.
    /// </summary>
    private float temporizador;

    /// <summary>
    /// Inicializa el generador con el temporizador para la primera oleada.
    /// </summary>
    private void Start()
    {
        temporizador = tiempo; // Inicia el temporizador para la primera oleada
    }

    /// <summary>
    /// Actualiza el temporizador y genera cajas cuando llega a cero.
    /// </summary>
    private void Update()
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            GenerarOleada();
            temporizador = tiempo; // Reiniciar temporizador para la siguiente oleada
        }
    }

    /// <summary>
    /// Genera una oleada de cajas según la cantidad configurada.
    /// </summary>
    void GenerarOleada()
    {
        for (int i = 0; i < cajasPorTiempo; i++)
        {
            GenerarCaja();
        }
    }

    /// <summary>
    /// Genera una caja individual en una posición aleatoria dentro de los límites.
    /// </summary>
    void GenerarCaja()
    {
        // Generar una posición aleatoria dentro de los límites
        float posX = Random.Range(limiteInferior.x, limiteSuperior.x);
        float posY = Random.Range(limiteInferior.y, limiteSuperior.y);
        Vector3 posicionGeneracion = new Vector3(posX, posY, 0f);

        // Instanciar caja
        Instantiate(cajaPrefab, posicionGeneracion, Quaternion.identity);
    }

    /// <summary>
    /// Reduce el tiempo entre oleadas de cajas.
    /// </summary>
    /// <param name="disminucion">Cantidad en segundos a restar del intervalo actual.</param>
    public void DismininuirTiempo(float disminucion) 
    {
        if (tiempo > disminucion) 
        {
            tiempo = tiempo - disminucion;
        }
    }
}
