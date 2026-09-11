namespace CampusBorrowing.Domain;

public class Borrowing
{
    public int Id { get;}
    public int StudentId{ get;}
    public int EquipmentId { get;}
    public DateTime BorrowDate { get; private set; }
    public DateTime ReturnDate { get; private set; }
    public BorrowingStatus Status { get; private set; }
    public Borrowing(int id, int studentId, int equipmentId, DateTime borrowDate, DateTime returnDate)
    {
        Id = id;
        StudentId = studentId;
        EquipmentId = equipmentId;
        BorrowDate = borrowDate;
        ReturnDate = returnDate;
        Status = BorrowingStatus.Active;
    }
    public void Return() => Status = BorrowingStatus.Returned;
}
