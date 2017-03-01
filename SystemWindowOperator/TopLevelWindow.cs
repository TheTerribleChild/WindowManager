﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SystemWindowOperator
{
    public class TopLevelWindow
    {

        public string ApplicationName { get; private set; }
        public string Title { get; private set; }
        public int ID { get; private set; }
        public object Placement { get; private set; }
        public int Z { get; private set; }

        public TopLevelWindow(string applicationName, string title, int id, object placement, int z)
        {
            this.ApplicationName = applicationName;
            this.Title = title;
            this.ID = id;
            this.Placement = placement;
            this.Z = z;
        }

        public override string ToString()
        {
            return ApplicationName + " " + Title + " " + ID + " " + " " + Z + " ";
        }
    }
}
