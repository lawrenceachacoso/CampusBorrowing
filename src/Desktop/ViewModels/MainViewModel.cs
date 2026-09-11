using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CampusBorrowing.Application.Interfaces;
using CampusBorrowing.Application.Services;
using CampusBorrowing.Domain;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusBorrowing.Desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly BorrowEquipmentService _borrowService;
    private readonly ReturnEquipmentService _returnService;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IBorrowingRepository _borrowingRepository;

    public ObservableCollection<Equipment> EquipmentList { get; } = new();
    public ObservableCollection<Borrowing> ActiveBorrowings { get; } = new();

    [ObservableProperty]
    private Equipment? _selectedEquipment;

    [ObservableProperty]
    private Borrowing? _selectedBorrowing;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private int _studentIdInput = 1;
    [ObservableProperty]
    private bool _isEquipmentViewActive = true;

    [ObservableProperty]
    private bool _isBorrowingsViewActive = false;

    [RelayCommand]
    private void ShowEquipment()
    {
        IsEquipmentViewActive = true;
        IsBorrowingsViewActive = false;
    }

    [RelayCommand]
    private void ShowBorrowings()
    {
        IsEquipmentViewActive = false;
        IsBorrowingsViewActive = true;
    }


    public MainViewModel(
        BorrowEquipmentService borrowService,
        ReturnEquipmentService returnService,
        IEquipmentRepository equipmentRepository,
        IBorrowingRepository borrowingRepository)
    {
        _borrowService = borrowService;
        _returnService = returnService;
        _equipmentRepository = equipmentRepository;
        _borrowingRepository = borrowingRepository;

        _ = LoadEquipmentAsync();
    }

    private async Task LoadEquipmentAsync()
    {
        var equipment = await _equipmentRepository.GetAllAsync();
        EquipmentList.Clear();
        foreach (var item in equipment)
            EquipmentList.Add(item);
    }

    [RelayCommand]
    private async Task BorrowAsync()
    {
        if (SelectedEquipment is null)
        {
            StatusMessage = "Please select a piece of equipment first.";
            return;
        }

        var result = await _borrowService.BorrowAsync(
            studentId: StudentIdInput,
            equipmentId: SelectedEquipment.Id,
            expectedReturnDate: DateTime.Now.AddDays(7));

        if (result.Success)
        {
            StatusMessage = $"Borrowed successfully. Borrowing ID: {result.Borrowing!.Id}";
            await LoadEquipmentAsync();
            await RefreshActiveBorrowingsAsync();
        }
        else
        {
            StatusMessage = $"Failed to borrow: {result.FailureReason}";
        }
    }

    [RelayCommand]
    private async Task ReturnAsync()
    {
        if (SelectedBorrowing is null)
        {
            StatusMessage = "Please select an active borrowing first.";
            return;
        }

        var result = await _returnService.ReturnAsync(SelectedBorrowing.Id);

        if (result.Success)
        {
            StatusMessage = $"Returned successfully. Borrowing ID: {result.Borrowing!.Id}";
            await LoadEquipmentAsync();
            await RefreshActiveBorrowingsAsync();
        }
        else
        {
            StatusMessage = $"Failed to return: {result.FailureReason}";
        }
    }

    private async Task RefreshActiveBorrowingsAsync()
    {
        var active = await _borrowingRepository.GetActiveBorrowingByStudentAsync(StudentIdInput);
        ActiveBorrowings.Clear();
        foreach (var b in active)
            ActiveBorrowings.Add(b);
    }
}