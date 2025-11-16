namespace relojChecadorAPI;

public class MensajesDB : IMensajesDB
{
    List<string> ESTADOS = new List<string>
    {
        "CREADO",
        "SUSPENDIDO",
        "ELIMINADO",
        "MODIFICADO"
    };

    public string LlaveForaneaNoExiste(string entidad)
    {
        return $"El/La {entidad}, no esta registrada.";
    }

    public string MensajeNoEncontrado(string entidad)
    {
        return $"EL/LA {entidad}, no se encuentra en la base de datos.";
    }

    public string MensajeModificarDB(string entidad, int type)
    {
        return $"EL/LA {entidad}, fue {ESTADOS[type]} correctamente.";
    }

   
}
