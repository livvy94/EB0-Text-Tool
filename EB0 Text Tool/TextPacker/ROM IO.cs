using System.IO;

namespace TextPacker
{
    class ROM_IO
    {
        internal static string FileExists(string romPath)
        {
            //Check as a full path
            if (File.Exists(romPath))
                return romPath;

            //Check as [Current Directory]/romPath
            var longPath = Path.Combine(Directory.GetCurrentDirectory(), romPath);
            if (File.Exists(longPath))
                return longPath;

            return "ERROR! Couldn't find either " + romPath + " or " + longPath;
        }
    }
}
