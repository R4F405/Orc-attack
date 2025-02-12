using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    public GameObject[] enemigosPrefabs; // Array de prefabricados de enemigos
    public float tiempoEntreGeneraciones = 2f; // Tiempo entre cada generación
    public Vector2 limiteInferior; // Limite inferior de la zona de generación
    public Vector2 limiteSuperior; // Limite superior de la zona de generación
    public int enemigosPorOleada = 5; // Número de enemigos por oleada
    public float tiempoEntreOleadas = 5f; // Tiempo entre oleadas

    private float temporizador; // Temporizador para gestionar las oleadas

    private void Start()
    {
        temporizador = tiempoEntreOleadas; // Inicia el temporizador para la primera oleada
    }

    private void Update()
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            GenerarOleada();
            temporizador = tiempoEntreOleadas; // Resetear temporizador para la siguiente oleada
        }
    }

    void GenerarOleada()
    {
        for (int i = 0; i < enemigosPorOleada; i++)
        {
            GenerarEnemigo();
        }
    }

    void GenerarEnemigo()
    {
        // Generar una posición aleatoria dentro de los límites establecidos
        float posX = Random.Range(limiteInferior.x, limiteSuperior.x);
        float posY = Random.Range(limiteInferior.y, limiteSuperior.y);

        Vector3 posicionGeneracion = new Vector3(posX, posY, 0f);

        // Elegir un enemigo aleatorio de los prefabricados
        int indiceEnemigo = Random.Range(0, enemigosPrefabs.Length);
        GameObject enemigo = Instantiate(enemigosPrefabs[indiceEnemigo], posicionGeneracion, Quaternion.identity);

    }
}
