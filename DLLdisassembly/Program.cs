using System;
using System.IO;
using System.Reflection;

namespace DLLdisassembly
{
    internal class DisassemblyClass
    {
        public static void Main(string[] args)
        {
            // Подгружаем все пути сборок с расширением .dll
            var filePaths = Directory.GetFiles(@"C:\DEV\Assemblies", "*.dll");
            ShowAssemblyInfo(filePaths);
        }
        private static void ShowAssemblyInfo(string[] filePaths)
        {
            // Пробегаемся по сборкам
            foreach (var filePath in filePaths)
            {
                try
                {
                    // Подгружаем сборку
                    var readedAssembly = Assembly.LoadFile(filePath);
                    // Пробегаемся по типам сборки
                    foreach (var type in readedAssembly.GetTypes())
                    {
                        // Если тип является классом, то обрабатываем события
                        if (type.IsClass)
                        {
                            // Кодировка для красоты
                            Console.OutputEncoding = System.Text.Encoding.UTF8;
                            // Вывод названия класса
                            Console.WriteLine("\u2022 " + type.Name);
                            // Получаем методы, которые являются членами экземпляра, публичные и не только, унаследованые не учитываем
                            var methodInfo = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                            // Пробегаемся по методам чтобы их вывести в консоль
                            foreach (var mi in methodInfo)
                            {
                                Console.WriteLine("\t\u25E6 " + mi.Name);
                            }    
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.ReadLine();
        }
    }
}