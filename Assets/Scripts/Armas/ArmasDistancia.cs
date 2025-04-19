using UnityEngine;

public class ArmasDistancia : MonoBehaviour
{
    public GameObject balaPrefab; // Prefab de la bala

    public int danioBase = 0; // Daño base de la bala (Solo se modifica desde el inspector)
    [HideInInspector] public int danio = 0; // Daño de la bala


    public float recargaBase = 0f; // Tiempo de recarga base entre disparos (Solo se modifica desde el inspector)
    [HideInInspector]public float recarga = 0f; // Tiempo de recarga entre disparos

    public int probabilidadCritico = 0; //Probabilidad de critico 
    public float velocidadBala = 0f; // Velocidad de la bala
    public float alcance = 0f;
    public int probabilidadRobarVida = 0; // Probabilidad de robar 1 de vida tras golpe en %
    public LayerMask capaEnemigos; // Capa de los enemigos
    public LayerMask capaCajas; // Nueva capa para detectar cajas

    private Collider2D colliderJugador; // Referencia al collider del jugador
    private float tiempoSiguienteDisparo = 0f;
    private bool esCritico = false;
    private int danioCritico;

    private void Start()
    {
        danio = danioBase; // Inicializar el daño con el valor base
        recarga = recargaBase; // Inicializar la recarga con el valor base

        // Aplicar mejoras persistentes
        GestorMejorasArmas gestorMejoras = GestorMejorasArmas.instancia;
        if (gestorMejoras != null)
        {
            AumentarDanioPorPocentaje(gestorMejoras.ObtenerAumentoDanioPorcentaje());
            AumentarProbabilidadCritico(gestorMejoras.ObtenerAumentoProbabilidadCritico());
            AumentarProbabilidadRobarVida(gestorMejoras.ObtenerAumentoProbabilidadRobarVida());
            DisminuirRecargaPorPocentaje(gestorMejoras.ObtenerDisminucionRecargaPorcentaje());
            
            // Debug para verificar las mejoras aplicadas
            Debug.Log("Arma Distancia inicializada: Aplicadas mejoras del GestorMejorasArmas");
        }

        if (colliderJugador == null)
        {
            colliderJugador = GameObject.FindWithTag("Jugador")?.GetComponent<Collider2D>();
        }
    }

    private void Update()
    {
        if (Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        Collider2D[] objetivos = Physics2D.OverlapCircleAll(transform.position, alcance, capaEnemigos | capaCajas);
        if (objetivos.Length > 0)
        {
            Transform objetivo = objetivos[0].transform;

            GameObject bala = Instantiate(balaPrefab, ObtenerPuntoDisparo(), Quaternion.identity);
            Bala scriptBala = bala.GetComponent<Bala>();
            if (scriptBala != null)
            {
                ProbabilidadCritico();
                if (esCritico) 
                {
                    scriptBala.ConfigurarBala(danioCritico, velocidadBala, capaEnemigos, capaCajas, colliderJugador, objetivo,probabilidadRobarVida);
                }
                else 
                {
                    scriptBala.ConfigurarBala(danio, velocidadBala, capaEnemigos, capaCajas, colliderJugador, objetivo,probabilidadRobarVida);
                }
                esCritico = false;
            }

            tiempoSiguienteDisparo = Time.time + recarga;
        }
    }

    private Vector2 ObtenerPuntoDisparo()
    {
        return new Vector2(transform.position.x, transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2));
    }

    private void ProbabilidadCritico() 
    {
        int probabilidad = Random.Range(0, 100);
        if (probabilidad < probabilidadCritico)
        {
            danioCritico = danio * 2;
            esCritico = true;
        }   
    }

    public void AumentarProbabilidadRobarVida (int cantidad) 
    {
        probabilidadRobarVida += cantidad;
    }

    public void AumentarDanioPorPocentaje(int porcentaje)
    {
        // Corregido para evitar la división entera que resultaría en 0 con porcentajes < 100
        float porcentajeDecimal = porcentaje / 100f;
        int aumento = Mathf.RoundToInt(danioBase * porcentajeDecimal);
        danio += aumento;
        
        // Debug para verificar que se está aplicando el daño correctamente
        Debug.Log("Arma Distancia: Daño base = " + danioBase + ", Porcentaje = " + porcentaje + "%, Aumento = " + aumento + ", Daño final = " + danio);
    }

    public void DisminuirRecargaPorPocentaje(int porcentaje)
    {
        // Corregido para evitar la división entera que resultaría en 0 con porcentajes < 100
        float porcentajeDecimal = porcentaje / 100f;
        float disminucion = recargaBase * porcentajeDecimal;
        recarga -= disminucion;
        
        // Asegurarse de que la recarga no sea menor que un valor mínimo
        if (recarga < 0.1f) recarga = 0.1f;
        
        // Redondear a 2 decimales para mayor precisión
        recarga = Mathf.Round(recarga * 100f) / 100f;
        
        // Debug para verificar que se está aplicando la recarga correctamente
        Debug.Log("Arma Distancia: Recarga base = " + recargaBase + ", Porcentaje = " + porcentaje + "%, Disminución = " + disminucion + ", Recarga final = " + recarga);
    }

     public void AumentarProbabilidadCritico(int cantidad)
    {
       probabilidadCritico = probabilidadCritico + cantidad;     
    }
}
