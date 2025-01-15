namespace Hahn.Domain.Entities.ValueObjects;

public class Address
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    private Address() { }

    public Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }
    public override bool Equals(object? obj)
    {
        if (obj is not Address other) return false;
        return Street == other.Street
            && City == other.City
            && State == other.State
            && ZipCode == other.ZipCode;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, ZipCode);
    }
}

