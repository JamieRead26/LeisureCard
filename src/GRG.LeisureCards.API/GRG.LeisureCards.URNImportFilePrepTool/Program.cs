using System;
using System.IO;

namespace GRG.LeisureCards.URNImportFilePrepTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: URNImportFilePrepTool.exe {importfile} {outputfile} {tenantKey} {ref}");
            }

            if (File.Exists(args[1]))
                File.Delete(args[1]);

            Console.WriteLine("reading source file {0} writing output {1}", args[0], args[1]);
            using (var reader = new StreamReader(File.OpenRead(args[0])))
            using (var fileWriter = new StreamWriter(File.OpenWrite(args[1])))
            {
                fileWriter.WriteLine(
                    "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"",
                    "Code", "UploadedDate", "Reference", "RenewalPeriodMonths", "TenantKey");

                var count = 0;
                var uploadLoadDate = DateTime.Now.ToShortDateString();
                reader.ReadLine();
                var line = string.Empty;
                while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
                {
                    count++;
                    fileWriter.WriteLine("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"",
                        line, uploadLoadDate, args[3], "12", args[2]);
                }

                Console.WriteLine("Process complete for {0} URNS", count);
            }

#if DEBUG
            Console.ReadLine();
#endif

        }
    }
}
