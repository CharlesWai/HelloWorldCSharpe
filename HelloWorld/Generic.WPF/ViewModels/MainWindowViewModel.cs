using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.WPF.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
		private string? _tips = "Hello World!";

		public string? Tips
		{
			get { return _tips; }
			set {SetProperty(ref _tips , value); }
		}

	}
}
