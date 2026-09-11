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

## Reflection

**1. Where does the dependency rule actually show up here?**
Domain has zero refs. Application only refs Domain. Infra + Desktop ref Application/Domain, never the reverse. So UI or storage changes can't break the core.

**2. What'd it take to swap in a real DB instead of in-memory?**
Only touch Infrastructure — new repo classes using EF Core/SQLite, then update the DI registration. Domain/Application/Desktop don't change since they only know about the interfaces.

**3. Why put business rules in Application instead of the ViewModel?**
So they're not tied to Avalonia specifically. If there was ever a second UI (web, CLI) the rule already lives in one place instead of getting copy-pasted and drifting out of sync.

**4. Hardest part?**
Getting the nav panels to actually toggle — the `!IsEquipmentViewActive` negated binding wasn't resolving right. Fixed it by just using two separate bools instead of relying on the `!`.

**5. Known limitation?**
Borrowing IDs come from `new Random().Next(1000, 999999)` so uniqueness not guaranteed.

**6. What would you improve with more time?**
Actual separate views/viewmodels for Equipment and Borrowings instead of one ViewModel toggling visibility, plus unit tests around the two services to lock the rules.

## Status

Core stuff (borrow, return, nav, DI, MVVM, styling) all working end to end.

## Author

Lawrence ("Law")
