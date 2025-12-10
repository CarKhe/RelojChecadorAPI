namespace relojChecadorAPI;

public class AsistenciasFechasDTO
{
    public DateTime fechaInicio { get; set; } 
    public DateTime fechaFin { get; set; } 
}

public class AsistenciaExcelUSuarioDTO
{
    public long idUsuario { get; set; }
    public string nombreEmpleado { get; set; } = string.Empty;

}

public class AssistenciaExcelSearchDateDTO()
{
    public long idUsuario { get; set; }
    public AsistenciasFechasDTO fechas { get; set; } = new();
    
}


public class DetalleAsistenciaExcelDTO
{
    public string area { get; set; } = string.Empty;
    public string movimiento { get; set; } = string.Empty;
    public DateTime fechaHora { get; set; }
}


public class ResultadoHorasDia
{
    public TimeSpan HorasTrabajadas { get; set; }
    public TimeSpan HorasDescanso { get; set; }
}