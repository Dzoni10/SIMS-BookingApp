namespace BookingApp.Serializer
{
    public interface ISerializable
    {
        string[] ToCSV();
        void FromCSV(string[] values);
        public int Id { get; set; }
    }
}
