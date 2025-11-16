namespace relojChecadorAPI;

public class AreasTablaDTOs
{
    public string nombre { get; set; } = string.Empty;
    public string descripcion { get; set; } = string.Empty;
    public decimal centroLat { get; set; }
    public decimal centroLon { get; set; }
    public int radio { get; set; }
    public DateTime fechaCreacion { get; set; }
    public ulong activo { get; set; }
}

public class AreasCrearDTOs
{
    public string nombre { get; set; } = string.Empty;
    public string descripcion { get; set; } = string.Empty;
    public decimal centroLat { get; set; }
    public decimal centroLon { get; set; }
    public int radio { get; set; }
}
