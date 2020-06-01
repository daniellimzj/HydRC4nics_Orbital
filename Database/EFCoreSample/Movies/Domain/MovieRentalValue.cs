namespace EFCoreSample.Movies.Domain
{
    public class MovieRentalValue
    {
        public MovieRentalValue(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; }
        public string Address { get; }
    }
}