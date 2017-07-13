using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FitConnectApp.Models
{
    public class DragMessage
    {
        public int Order { get; set; }
        public Guid Id { get; set; }
    }

    public class DropMessage : DragMessage
    {
        
    }
}