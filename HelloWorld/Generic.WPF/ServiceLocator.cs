using Generic.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.WPF
{
    public class ServiceLocator
    {
        private IServiceProvider? _serviceProvider;
        public ServiceLocator()
        {          
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<MainWindowViewModel>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        public MainWindowViewModel?  MainWindow => _serviceProvider?.GetService<MainWindowViewModel>();
    }
}
