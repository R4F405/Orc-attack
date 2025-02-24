using UnityEngine;

public class GeneradorCajas : MonoBehaviour
{
    public GameObject cajaPrefab; // Prefabricado de Caja
    public Vector2 limiteInferior; // Límite inferior de la zona de generación
    public Vector2 limiteSuperior; // Límite superior de la zona de generación
    public int cajasPorTiempo = 1; // Número de cajas por oleada
    public float tiempo = 10f; // Tiempo entre oleadas

    private float temporizador; // Temporizador para gestionar las oleadas

    private void Start()
    {
        temporizador = tiempo; // Inicia el temporizador para la primera oleada
    }

    private void Update()
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            GenerarOleada();
            temporizador = tiempo; // Reiniciar temporizador para la siguiente oleada
        }
    }

    void GenerarOleada()
    {
        for (int i = 0; i < cajasPorTiempo; i++)
        {
            GenerarCaja();
        }
    }

    void GenerarCaja()
    {
        // Generar una posición aleatoria dentro de los límites
        float posX = Random.Range(limiteInferior.x, limiteSuperior.x);
        float posY = Random.Range(limiteInferior.y, limiteSuperior.y);
        Vector3 posicionGeneracion = new Vector3(posX, posY, 0f);

        // Instanciar caja
        Instantiate(cajaPrefab, posicionGeneracion, Quaternion.identity);
    }

    public void DismininuirTiempo(float disminucion) 
    {
        if (tiempo > disminucion) 
        {
            tiempo = tiempo - disminucion;
        }
    }
}
