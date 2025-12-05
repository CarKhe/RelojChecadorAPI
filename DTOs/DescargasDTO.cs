namespace relojChecadorAPI;

public class AsistenciasFechasDTO
{
    public DateTime fechaInicio { get; set; } 
    public DateTime fechaFin { get; set; } 
}

public class AsistenciaExcelDTO
{
    public long idUsuario { get; set; }
    public string nombreEmpleado { get; set; } = string.Empty;
    public List<DetalleAsistenciaExcelDTO> detalleAsistencias { get; set; } = new();

}

public class DetalleAsistenciaExcelDTO
{
    public string area { get; set; } = string.Empty;
    public string movimiento { get; set; } = string.Empty;
    public DateTime fechaHora { get; set; }
}
