using System;
using System.Collections.Generic;
using System.Text;

namespace App1.MenuItems
{
    public class MasterPageItem
    {
        public string Title
        {
            get;
            set;
        }
        public string Icon
        {
            get;
            set;
        }
        public Type TargetType
        {
            get;
            set;
        }

        public Xamarin.Forms.Color colour
        {
            get;
            set;
        }

        public Xamarin.Forms.Color gapColour
        {
            get;
            set;
        }
    }

}
