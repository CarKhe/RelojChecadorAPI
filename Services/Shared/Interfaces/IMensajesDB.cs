namespace relojChecadorAPI;

public interface IMensajesDB
{
    string LlaveForaneaNoExiste(string entidad);
    string MensajeNoEncontrado(string entidad);
    string MensajeModificarDB(string entidad, int type);
    
}
