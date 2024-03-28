using System;
using System.IO;
using System.Reflection;

namespace ConfigGen
{ 
    ///<summary>Extracts embedded files.</summary>
    internal static partial class xExtractor
    {
        internal static void SaveResourceToDisk(String filePath, String assemblyResourcePath, Assembly assembly)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath was null");
            }

            if (assemblyResourcePath == null)
            {
                throw new ArgumentNullException("assemblyResourcePath was null");
            }

            if (assembly == null)
            {
                throw new ArgumentNullException("assembly was null");
            }

            (String path, String fileName) = ValidatingPathSplit(ref filePath);

            ValidateResource(ref assemblyResourcePath, ref assembly);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using FileStream fileStream = File.Create($"{path}\\{fileName}");

            assembly.GetManifestResourceStream(assemblyResourcePath).CopyTo(fileStream);
        }

        internal static String[] GetResourceNames(Assembly assembly, Boolean stripeTopLevelNamespace = false)
        {
            return GetResourceNames(ref assembly, stripeTopLevelNamespace);
        }

        //

        private static (String path, String fileName) ValidatingPathSplit(ref String filePath)
        {
            String path = "";
            String fileName = "";

            String[] pathParts = filePath.Split('\\');

            if (pathParts.Length < 2)
            {
                throw new ArgumentException("input was not a filePath?");
            }

            for (UInt16 i = 0; i < pathParts.Length - 1; ++i)
            {
                if (i < pathParts.Length - 2)
                {
                    path += pathParts[i] + "\\";
                }
                else
                {
                    path += pathParts[i];
                }
            }

            fileName = pathParts[pathParts.Length - 1];

            return (path, fileName);
        }

        private static void ValidateResource(ref String assemblyResourcePath, ref Assembly assembly)
        {
            String[] resourceNames = GetResourceNames(ref assembly);

            for (UInt32 i = 0; i < resourceNames.Length; ++i)
            {
                if (resourceNames[i] == assemblyResourcePath)
                {
                    return;
                }
            }

            throw new InvalidDataException("resource not found in assembly");
        }

        private static String[] GetResourceNames(ref Assembly assembly, Boolean stripeTopLevelNamespace = false)
        {
            String[] names = assembly.GetManifestResourceNames();

            if (stripeTopLevelNamespace)
            {
                for (UInt32 i = 0; i < names.Length; i++)
                {
                    names[i] = names[i].Split(new Char[] { '.' }, 2, StringSplitOptions.None)[1];
                }
            }

            return names;
        }
    }
}