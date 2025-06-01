using ExcelShSy.Core.Interfaces.Common;
using ExcelShSy.Ui.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace ExcelShSy.Ui.Factories
{
    public class SettingWindowFactory : ISettingWindowFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SettingWindowFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public SettingWindow Create()
        {
            return _serviceProvider.GetRequiredService<SettingWindow>();
        }
    }
}
