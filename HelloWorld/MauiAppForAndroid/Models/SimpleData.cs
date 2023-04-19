using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppForAndroid.Models
{
    public class SimpleData :ObservableObject
    {
        public string DtNow { get; set; }
        public string Message { get; set; }
        public string HexMessage { get; set; }
    }
}
