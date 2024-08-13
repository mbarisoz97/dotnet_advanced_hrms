namespace Ehrms.Shared.TestHepers;

public static class PortNumberProvider
{
    private static HashSet<int> _portsInUse = new();

    public static int GetPortNumber()
    {
        lock (_portsInUse)
        {
            int port;
            int count = 0;
            do
            {
                port = Random.Shared.Next(1024, 49151);
                if (!_portsInUse.Contains(port))
                {
                    _portsInUse.Add(port);
                    return port;
                }
            } while (++count < 10);

            throw new Exception("Could not find available port.");
        }
    }

    public static void ReleasePortNumber(int port)
    {
        lock (_portsInUse)
        {
            _portsInUse.Remove(port);
        }
    }
}