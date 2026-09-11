namespace CampusBorrowing.Domain;

public class Equipment
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public bool Availability { get; private set; }

    public Equipment(int id, string name, bool availability = true)
    {
        Id = id;
        Name = name;
        Availability = availability;
    }
    public void Borrowed() => Availability = false;
    public void Available() => Availability = true;
}
