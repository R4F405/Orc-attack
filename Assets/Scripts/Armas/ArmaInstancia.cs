/// <summary>
/// Representa una instancia concreta de un arma en el inventario del jugador,
/// con su tipo (DatosArma) y nivel de rareza actual.
/// </summary>
[System.Serializable]
public class ArmaInstancia
{
    public DatosArma datos;
    public int nivel;

    public ArmaInstancia(DatosArma datos, int nivel = 1)
    {
        this.datos = datos;
        this.nivel = UnityEngine.Mathf.Clamp(nivel, NivelArma.NIVEL_MIN, NivelArma.NIVEL_MAX);
    }

    public int Danio => datos.ObtenerDanio(nivel);
    public float Recarga => datos.ObtenerRecarga(nivel);
    public int Critico => datos.ObtenerCritico(nivel);
    public int RoboVida => datos.ObtenerRoboVida(nivel);
    public string Nombre => datos.nombre;
    public UnityEngine.Sprite Icono => datos.icono;
    public TipoArma Tipo => datos.tipo;
    public int Precio => datos.ObtenerPrecio(nivel);
    public bool PuedeMejorar => NivelArma.PuedeMejorar(nivel);

    /// <summary>
    /// Comprueba si otra instancia es del mismo tipo de arma y mismo nivel (fusionable).
    /// </summary>
    public bool MismoTipoYNivel(ArmaInstancia otra)
    {
        if (otra == null || nivel != otra.nivel) return false;
        if (datos == null || otra.datos == null) return false;

        // Misma referencia de ScriptableObject (caso normal).
        if (datos == otra.datos) return true;

        // Fallback por si hubiera duplicados de asset con el mismo contenido.
        return datos.nombre == otra.datos.nombre
               && datos.tipo == otra.datos.tipo
               && datos.danioBase == otra.datos.danioBase
               && datos.prefab == otra.datos.prefab;
    }
}
