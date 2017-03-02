using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SystemWindowOperator
{
    public static class WindowOperatorFactory
    {
        private static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            // TODO: Argument validation
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IWindowOperator CreateWindowOperator()
        {
            string[] sourcePackDLLs = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "WindowOperators"), "*.dll");

            foreach (string dllName in sourcePackDLLs)
            {
                Assembly assembly = Assembly.LoadFrom(dllName);

                if (assembly == null)
                    continue;

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IWindowOperator).IsAssignableFrom(type))
                    {
                        return Activator.CreateInstance(type) as IWindowOperator;
                    }
                        
                }
            }
            return null;
        }
    }
}
