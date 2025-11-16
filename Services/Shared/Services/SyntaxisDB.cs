namespace relojChecadorAPI;

public class SyntaxisDB:ISyntaxisDB
{
    public string StringUpper(string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return texto;

        return texto.ToUpper();
    }

}
