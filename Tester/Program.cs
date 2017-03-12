using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWindowOperator;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("OSVersion: {0}", Environment.OSVersion);
            IWindowOperator swo = WindowOperatorFactory.CreateWindowOperator();
            string[] array = new string[10];
            TopLevelWindow[] windows = swo.GetTopLevelWindow();
            foreach(TopLevelWindow tlw in windows)
            {
                Console.WriteLine(tlw.ToString());
            }
            Console.ReadKey();

            Console.WriteLine("\n\nBefore moving\n");
            TopLevelWindow[] beforewindows = swo.GetTopLevelWindow();
            foreach (TopLevelWindow tlw in beforewindows)
            {
                Console.WriteLine(tlw.ToString());
            }

            swo.ApplyLayoutConfiguration(windows);

            Console.WriteLine("\n\nAfter moving\n");
            TopLevelWindow[] afterwindows = swo.GetTopLevelWindow();
            foreach (TopLevelWindow tlw in afterwindows)
            {
                Console.WriteLine(tlw.ToString());
            }
            */
            WindowsManager.WindowManager.Instance.Test();
            /*
            IWindowOperator swo = WindowOperatorActivator.CreateWindowOperator();
            MappedLayout layout = swo.GetCurrentMappedLayout();
            foreach (MappedWindow tlw in layout.Mapping)
            {
                Console.WriteLine(tlw.Configuration.ToString()+"|"+tlw.Handle);
            }
            
            //Console.ReadKey();
            //swo.ApplyMappedLayout(layout);
            /*
            Console.WriteLine("\n\nBefore moving\n");
            MappedLayout layout2 = swo.GetCurrentMappedLayout();
            foreach (MappedWindow tlw in layout2.Mapping)
            {
                Console.WriteLine(tlw.Configuration.ToString() + "|" + tlw.Handle);
            }

            swo.ApplyMappedLayout(layout);
            Console.WriteLine("\n\nAfter\n");
            MappedLayout layout3 = swo.GetCurrentMappedLayout();
            foreach (MappedWindow tlw in layout3.Mapping)
            {
                Console.WriteLine(tlw.Configuration.ToString() + "|" + tlw.Handle);
            }
            */
        }
    }
}
