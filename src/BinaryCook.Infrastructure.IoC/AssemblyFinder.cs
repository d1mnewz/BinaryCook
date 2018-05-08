using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BinaryCook.Infrastructure.IoC
{
    public class AssemblyFinder
    {
        public static IEnumerable<Assembly> FindAssembliesFromCurrentDomain(bool includeExeFiles = false) =>
            FindAssemblies(AppDomain.CurrentDomain.BaseDirectory, includeExeFiles);
        
        public static IEnumerable<Assembly> FindAssembliesFromCurrentDomain(Func<Assembly, bool> filter, bool includeExeFiles = false) =>
            FindAssemblies(filter, AppDomain.CurrentDomain.BaseDirectory, includeExeFiles);

        public static IEnumerable<Assembly> FindAssemblies(Func<Assembly, bool> filter, string assemblyPath, bool includeExeFiles = false) =>
            FindAssemblies(assemblyPath, includeExeFiles).Where(filter);

        public static IEnumerable<Assembly> FindAssemblies(string assemblyPath, bool includeExeFiles = false)
        {
            var dllFiles = Directory.EnumerateFiles(assemblyPath, "*.dll", SearchOption.AllDirectories).ToList();
            var files = dllFiles;

            if (includeExeFiles)
            {
                var exeFiles = Directory.EnumerateFiles(assemblyPath, "*.exe", SearchOption.AllDirectories);
                files = dllFiles.Concat(exeFiles).ToList();
            }

            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                Assembly assembly = null;

                try
                {
                    assembly = Assembly.Load(new AssemblyName(name));
                }
                catch (Exception)
                {
                    try
                    {
                        assembly = Assembly.LoadFrom(file);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (assembly != null)
                    yield return assembly;
            }
        }
    }
}