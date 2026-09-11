# Campus Equipment Borrowing System

An Architecture desktop application for managing equipment borrowing at a university lab

## Solution Structure
CampusBorrowing/
  ─ CampusBorrowing.sln
  ─ src/
    ─ Domain/ # Core business entities, no dependencies
    ─ Application/ # Business logic, repository interfaces
    ─ Infrastructure/ # In-memory repository implementations
    ─ ConsoleApp/ # Entry point, demonstrates the system 
  -README.md

  
Domain has zero dependencies. Application depends only on Domain. Infrastructure depends on both Application and Domain. ConsoleApp composes everything together. Dependencies always point inward — outer layers know about inner layers, never the reverse.

## Part A: System Analysis

### Actors

**Student** — expects to check equipment availability, request to borrow available equipment, and return equipment when finished, receiving clear success or failure feedback at each step.

### Use Cases

**Use Case: Borrow Equipment**
- Primary Actor: Student
- Preconditions: Student is currently allowed to borrow equipment (not restricted)
- Main Action: Student requests to borrow a specific piece of equipment
- Expected Result: Student successfully borrows the equipment; a new borrowing record is created with status Active
- Possible Failure: Student is not currently allowed to borrow; equipment does not exist; equipment is unavailable; student has reached the maximum number of active borrowings

**Use Case: Return Equipment**
- Primary Actor: Student
- Preconditions: An active (unreturned) borrowing record exists for this equipment
- Main Action: Student returns the equipment
- Expected Result: The borrowing is marked as Returned and the equipment becomes available again
- Possible Failure: Equipment does not exist; borrowing has already been returned; no borrowing was ever made for this equipment/student pair

**Use Case: Check Equipment Availability**
- Primary Actor: Student
- Preconditions: Equipment exists in the system
- Main Action: Student checks whether a piece of equipment is currently available
- Expected Result: Student receives accurate information on whether the equipment is currently available
- Possible Failure: Equipment does not exist

### Domain Concepts

**Student**
- Holds: Id, Name, IsAllowedToBorrow
- Does not hold its own borrowing count — that is calculated by querying active Borrowing records, avoiding duplicated/inconsistent state
- Not responsible for: checking its own eligibility against other students' data, counting borrowings, or any cross-object business rule

**Equipment**
- Holds: Id, Name, IsAvailable (Availability)
- Not responsible for: knowing who currently has it borrowed or tracking its own borrowing history — that belongs to Borrowing

**Borrowing**
- Holds: Id, StudentId, EquipmentId, DateBorrowed, ExpectedReturnDate, Status (Active/Returned)
- References Student and Equipment by Id rather than holding the full objects, keeping it lightweight and consistent with how a real database would model the relationship
- Not responsible for: deciding whether a new borrowing is allowed — that decision spans multiple objects and belongs in the Application layer (BorrowEquipmentService)

## How to Run

```bash
dotnet run --project src/ConsoleApp/CampusBorrowing.Console.csproj
```

This demonstrates:
1. A successful borrowing
2. A failed borrowing attempt (equipment already unavailable)
3. A failed borrowing attempt (equipment does not exist)

## Why This Structure Supports Future Desktop UI Work

Because all business rules live in the Application layer and are accessed only through `BorrowEquipmentService`, a future graphical interface (e.g., Avalonia) can call the exact same service without duplicating or reimplementing any borrowing logic. The UI layer would only need to handle presentation concerns — displaying data and collecting input — while Domain and Application remain completely unaware that a UI framework exists. Swapping the in-memory repositories for a real database later would similarly only require changes in the Infrastructure layer.
