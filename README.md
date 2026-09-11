# Campus Equipment Borrowing System

Desktop app for tracking campus equipment borrowing. Built with .NET 8 + Avalonia (MVVM), using Clean Architecture.

## Structure

```
src/
├── Domain/          entities + business rules, no deps
├── Application/     use cases, service + repo interfaces
├── Infrastructure/  in-memory repos
└── Desktop/         Avalonia UI (views + viewmodels)
```

Deps only point inward: Desktop → Application → Domain. Infrastructure implements Application's repo interfaces but Domain/Application never know it exists.

## Layers

**Domain** — `Student`, `Equipment`, `Borrowing`, `BorrowingStatus`. No refs to anything else.

**Application** — `IStudentRepository`, `IEquipmentRepository`, `IBorrowingRepository` (interfaces only), plus `BorrowEquipmentService` and `ReturnEquipmentService` which hold the actual business rules.

**Infrastructure** — in-memory implementations of the repo interfaces, seeded with sample data.

**Desktop** — MVVM via CommunityToolkit. `[ObservableProperty]` + `[RelayCommand]` handle the binding/commands. DI is set up in `App.axaml.cs` — ViewModel just takes services in its constructor, doesn't care what's behind the interfaces.

## Navigation

Sidebar with Equipment / Active Borrowings buttons. Toggles which panel shows via two bools (`IsEquipmentViewActive`, `IsBorrowingsViewActive`) bound to `IsVisible` on each panel. Same ViewModel instance the whole time so state doesn't reset when switching.

## Borrow flow

1. Enter student ID, pick equipment, hit Borrow
2. `BorrowEquipmentService` checks: student valid → equipment available → under max active borrowings
3. Pass → new `Borrowing` created, equipment marked unavailable, lists refresh
4. Fail → nothing changes, status message says why

## Return flow

1. Pick a borrowing from Active Borrowings, hit Return
2. `ReturnEquipmentService` checks it exists and isn't already returned
3. Marks it returned, frees up the equipment, lists refresh

## Validation

Presentation-level stuff (no student entered, nothing selected) is handled in the ViewModel — quick early-return + status message, no service call. Actual business rules (equipment unavailable, student not eligible, max borrowings, already returned) live in the Application services. ViewModel just shows whatever comes back.

## Styling

Custom styles in `MainWindow.axaml` — accent color + hover state on buttons, separate style for nav buttons, heading + status text styles, consistent spacing. Not just default Avalonia look anymore.

## Running it

Needs .NET 8 SDK.

```bash
git clone <repo-url>
cd CampusBorrowing
dotnet build
dotnet run --project src/Desktop/CampusBorrowing.Desktop.csproj
```


BSIT-3D
lawrence andre l. achacoso
