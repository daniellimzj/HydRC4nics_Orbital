namespace EFCoreSample.Libraries.Domain
{
    public class LibraryValue
    {
        public LibraryValue(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; }
        public string Address { get; }
    }
}