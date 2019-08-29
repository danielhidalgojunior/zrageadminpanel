using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.BZip2;

namespace Bz2Helper
{
    public static class Bz2Handler
    {
        public static string Compress(string filename)
        {
            throw new NotImplementedException();
        }

        public static string Decompress(string filename, string location)
        {
            FileInfo zipFileName = new FileInfo(filename);
            using (FileStream fileToDecompressAsStream = zipFileName.OpenRead())
            {
                string decompressedFileName = Path.Combine(location, Path.GetFileName(filename)).Replace(".bz2", "");
                using (FileStream decompressedStream = File.Create(decompressedFileName))
                {
                    try
                    {
                        BZip2.Decompress(fileToDecompressAsStream, decompressedStream, true);
                        return decompressedFileName;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                    }
                }
            }
        }
    }
}
