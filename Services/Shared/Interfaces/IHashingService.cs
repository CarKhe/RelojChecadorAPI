namespace relojChecadorAPI;

public interface IHashingService
{
    string Hash(string input);
    bool Verify(string input, string storedHash);
}
