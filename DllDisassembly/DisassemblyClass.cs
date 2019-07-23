using System;
using System.IO;
using System.Reflection;

namespace DllDisassembly
{
    internal class DisassemblyClass
    {
        public static void Main(string[] args)
        {
            // Подгружаем все пути сборок с расширением .dll
            var filePaths = Directory.GetFiles(@"C:\DEV\Assemblies", "*.dll"); 
            var assemblyInfo = ShowAssemblyInfo(filePaths);
            // Кодировка для красоты
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine(assemblyInfo);
            Console.ReadLine();
        }

        private static string ShowAssemblyInfo(string[] filePaths)
        {
            string assemblyInfo = "";
            // Пробегаемся по сборкам
            foreach (var filePath in filePaths)
            {
                if (CheckDLL(filePath) != true) continue;
                // Подгружаем сборку
                var readedAssembly = Assembly.LoadFile(filePath);
                // Пробегаемся по типам сборки
                foreach (var type in readedAssembly.GetTypes())
                {
                    // Если тип является классом, то обрабатываем события
                    if (type.IsClass)
                    {
                        // Вывод названия класса
                        assemblyInfo += "\u2022 " + type.Name + "\n";
                        // Получаем методы, которые являются членами экземпляра, публичные и не только,
                        // унаследованые не учитываем
                        var methodInfo = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic |
                                                         BindingFlags.Public | BindingFlags.DeclaredOnly);
                        // Пробегаемся по методам чтобы их вывести в консоль
                        foreach (var mi in methodInfo)
                        {
                            if (mi.IsFamily || mi.IsPublic)
                            {
                                assemblyInfo += "\t\u25E6 " + mi.Name + "\n";
                            }
                        }
                    }
                }
            }
            return assemblyInfo;
        }

        private static bool CheckDLL(string filePath)
        { 
            try
            {
                var assembly = Assembly.LoadFile(filePath);
                return assembly != null;
            }
            catch
            {
                return false;
            }
        }
    }
}