using CommunityToolkit.Mvvm.ComponentModel;

namespace CampusBorrowing.Desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
}