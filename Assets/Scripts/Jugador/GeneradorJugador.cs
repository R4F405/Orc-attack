using UnityEngine;

public class GeneradorJugador : MonoBehaviour
{
    public GameObject prefabJugador; // Prefab del jugador
    
    private GameObject jugadorInstanciado;

    void Start()
    {
        GenerarJugador();
    }

    void Update()
    {
        if (jugadorInstanciado == null)
        {
            GenerarJugador();        
        }
    }

    public void GenerarJugador()
    {
        if (jugadorInstanciado == null)
        {
            jugadorInstanciado = Instantiate(prefabJugador, Vector3.zero, Quaternion.identity);
        }
    }
}
