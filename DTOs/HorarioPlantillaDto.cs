namespace relojChecadorAPI;

public class HorarioPlantillaCrearDto
{
    public string nombre { get; set; } = string.Empty;
    public List<DetalleHorarioPlantillaCrearDto>  detalleHorarioPlantillaCrear { get; set; } = new();
}

public class HorarioPlantillaTablaDto
{
    public int idHorarioPlantilla { get; set; }
    public string nombre { get; set; } = string.Empty;
    public ulong activo {get; set; }
    public List<DetalleHorarioPlantillaTablaDto>  detalleHorarioPlantillaTabla { get; set; } = new();
}

public class DetalleHorarioPlantillaCrearDto
{
    public long idHorarioPlantilla { get; set; } 
    public long idMovimiento {get; set; }
    public int diaSemana { get; set; }
    public TimeOnly hora { get; set; }
    public int margenAntes { get; set; }
    public int margenDespues { get; set; }
    public ulong laboral { get; set; }

}

public class DetalleHorarioPlantillaTablaDto
{
    public long idHorarioPlantilla { get; set; } 
    public string movimiento {get; set; } = string.Empty;
    public int diaSemana { get; set; }
    public TimeOnly hora { get; set; }
    public int margenAntes { get; set; }
    public int margenDespues { get; set; }
    public ulong laboral { get; set; }
    public ulong activo { get; set; }

}
