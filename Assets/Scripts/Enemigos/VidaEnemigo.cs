using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    public int saludMaxima = 100;
    public AudioClip sonidoMuerte;

    private int saludActual;
    private AudioSource audioSource;
    private DropCalaveras dropObjeto; // Referencia al script DropObjeto
    private BarraExperiencia barraExp;

    private void Start()
    {
        saludActual = saludMaxima;
        dropObjeto = GetComponent<DropCalaveras>(); // Obtener el script DropObjeto si está presente
        barraExp = FindAnyObjectByType<BarraExperiencia>(); // Busca la barra en la escena
        // Obtener o crear AudioSource para reproducir sonidos
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void RecibirDaño(int cantidad)
    {
        saludActual -= cantidad;
        if (saludActual <= 0f)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        // Desactivar visuales
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;
        // Desactivar colisiones
        foreach (Collider c in GetComponents<Collider>())
            c.enabled = false;
        foreach (Collider2D c2d in GetComponents<Collider2D>())
            c2d.enabled = false;
        // Desactivar scripts de movimiento
        var movCol = GetComponent<MovimientoEnemigoColision>();
        if (movCol != null) movCol.enabled = false;
        var movDist = GetComponent<MovimientoEnemigoDistancia>();
        if (movDist != null) movDist.enabled = false;

        // Reproducir sonido de muerte antes de destruir
        if (sonidoMuerte != null)
        {
            // Usar el nuevo método estático para reproducir el sonido
            ExtensionesAudio.ReproducirEnPosicion(sonidoMuerte, transform.position, 1.0f, TipoAudio.Efectos);
        }

        // Llamar a SoltarObjeto si el enemigo tiene el script DropObjeto
        if (dropObjeto != null)
        {
            dropObjeto.SoltarObjeto();
        }
        //Aumenta la experiencia si esta bien configurado
        if (barraExp != null)
        {
            barraExp.GanarExperiencia(); // Sumar experiencia al morir
        }

        // Destruir el objeto inmediatamente, ya que el sonido continuará reproduciéndose
        Destroy(gameObject);
    }

    public int ObtenerSalud()
    {
        return saludActual;
    }

    public int ObtenerSaludMaxima()
    {
        return saludMaxima;
    }
    
    // Método para reiniciar la salud actual al valor máximo
    public void ReiniciarSalud()
    {
        saludActual = saludMaxima;
    }
}
