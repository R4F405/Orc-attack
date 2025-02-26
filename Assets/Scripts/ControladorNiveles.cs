using UnityEngine;
using TMPro;

public class ControladorNiveles : MonoBehaviour
{
    public int nivelActual = 1;
    public float tiempoRestante = 20f;
    public GameObject panelTienda;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoTiempo;
    public GameObject GeneradorOrcos;
    public GameObject GeneradorMagos;
    public GameObject GeneradorCajas;

    private bool nivelEnCurso = true;

    void Start()
    {
        ActualizarUI();
        panelTienda.SetActive(false); // Asegurar que la tienda esté desactivada al inicio
    }

    void Update()
    {
        if (nivelEnCurso)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarUI();

            if (tiempoRestante <= 0)
            {
                FinalizarNivel();
            }
        }
    }

    void FinalizarNivel()
    {
        GeneradorCajas.SetActive(false); //Desactiva generador
        GeneradorOrcos.SetActive(false); //Desactiva generador
        GeneradorMagos.SetActive(false); //Desactiva generador
        nivelEnCurso = false;
        tiempoRestante = 0;
        DestruirObjetosPorCapa("Enemigo");
        DestruirObjetosPorCapa("Caja");
        DestruirObjetosPorCapa("Calavera");
        panelTienda.SetActive(true); // Mostrar tienda

        GameObject jugador = GameObject.FindGameObjectWithTag("Jugador");
        jugador.transform.position = Vector2.zero; // Coloca al jugador en (0,0,0)
    }

    public void IniciarSiguienteNivel()
    {
        nivelActual++;
        tiempoRestante = 20 + (nivelActual - 1) * 5; // Se suma 5 segundos por nivel
        nivelEnCurso = true;
        panelTienda.SetActive(false); // Ocultar tienda
        GeneradorCajas.SetActive(true);
        GeneradorOrcos.SetActive(true);
        GeneradorMagos.SetActive(true);
        ActualizarUI();
    }

    void DestruirObjetosPorCapa(string nombreCapa)
    {
        int capa = LayerMask.NameToLayer(nombreCapa);
        GameObject[] objetos = GameObject.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (GameObject obj in objetos)
        {
            if (obj.layer == capa)
            {
                Destroy(obj);
            }
        }
    }

    void ActualizarUI()
    {
        textoNivel.text = "Nivel " + nivelActual;
        textoTiempo.text = Mathf.Ceil(tiempoRestante).ToString();
    }
}
