using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace MaliciousMetadataWriter
{
    public class MetadataHelper
    {
        public static string WriteMetadata(string filePath, string author, string description)
        {
            try
            {
                string command = $"exiftool -overwrite_original -Author='{author}' -Description='{description}' '{filePath}'";

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{command}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine($"Command Output:\n{output}");
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine($"Command Error:\n{error}");
                    return $"Error: {error}";
                }

                return "Metadata updated successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return $"Failed to update metadata: {ex.Message}";
            }
        }
    }

   [Serializable]
    public class ProcessWrapper : ISerializable
    {
        public string FileName { get; set; }
        public string Arguments { get; set; }

        public ProcessWrapper() { }

        protected ProcessWrapper(SerializationInfo info, StreamingContext context)
        {
            FileName = info.GetString(nameof(FileName));
            Arguments = info.GetString(nameof(Arguments));

            Execute();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(FileName), FileName);
            info.AddValue(nameof(Arguments), Arguments);
        }

        public void Execute()
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = FileName,
                            Arguments = Arguments,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };

                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    Console.WriteLine($"Output:\n{output}");
                    Console.WriteLine($"Error:\n{error}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Execution failed: {ex.Message}");
                }
            }
        }

        public byte[] Serialize()
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, this);
                return memoryStream.ToArray();
            }
        }
    }
}
