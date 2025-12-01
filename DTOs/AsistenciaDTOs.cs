namespace relojChecadorAPI;

public class AsistenciaTablaDTOs
{
    public string usuario { get; set; } = string.Empty;
    public string area { get; set; } = string.Empty;
    public string movimiento { get; set; } = string.Empty;
    public decimal latitud { get; set; }
    public decimal longitud { get; set; }
    public decimal distanciaCentro { get; set; }
    public ulong dentroZona { get; set; }
    public DateTime fechaHora { get; set; }
}


public class AsistenciaToDashboard
{
    public string usuario { get; set; } = string.Empty;
    public string area { get; set; } = string.Empty;
    public string movimiento { get; set; } = string.Empty;
    public ulong dentroZona { get; set; }
    public DateTime fechaHora { get; set; }
}

public class AsistenciaUsuarioToDatabase
{
    public ulong idUsuario {get; set;}
    public ulong idArea {get; set;}
    public ulong idMovimiento {get; set;}
    public decimal latitud { get; set; }
    public decimal longitud { get; set; }
}

public class AsistenciaCrearDto
{
    public long idUsuario { get; set; }
    public long idArea { get; set; }
    public long idMovimiento { get; set; }
    public decimal latitud { get; set; }
    public decimal longitud { get; set; }
    public ulong dentroZona { get; set; }
    public DateTime fechaHora { get; set; }
}

public class AreaDto
{
    public decimal latitudCentral { get; set; }
    public decimal longitudCentral { get; set; }
    public int radio { get; set; }
}

public class LastRegisterDTO
{
    public int idUsuario {get; set;}
}
