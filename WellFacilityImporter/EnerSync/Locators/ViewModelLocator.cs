using CommunityToolkit.Mvvm.DependencyInjection;
using EnerSync.ViewModels;

namespace EnerSync.Locators
{
    public class ViewModelLocator
    {
#pragma warning disable CA1822 // Mark members as static (false positive), this property is late time bound
        public MainViewModel MainViewModel => Ioc.Default.GetService<MainViewModel>() ?? null!;
        public WellsFilterViewModel WellsFilterViewModel => Ioc.Default.GetService<WellsFilterViewModel>() ?? null!;
        public FacilitiesFilterViewModel FacilitiesFilterViewModel => Ioc.Default.GetService<FacilitiesFilterViewModel>() ?? null!;
#pragma warning restore CA1822 // Mark members as static
    }
}
