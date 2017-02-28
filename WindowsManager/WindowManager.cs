using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManager
{
    public class WindowManager
    {
        private static WindowManager instance;

        public static WindowManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new WindowManager();
                return instance;
            }
        }

        private WindowManager()
        {
            
        }
    }
}
