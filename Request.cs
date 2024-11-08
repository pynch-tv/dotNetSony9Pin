namespace lathoub;

public class Request(string s)
{
    private readonly string _s = s + "\n";

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _s;
    }
}
