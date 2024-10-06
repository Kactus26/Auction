namespace CommonDTO
{
    public class OfferPrice
    {
        public int LotId {  get; set; }
        public double Price { get; set; }
    }

    public class LotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = String.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int StartPrice { get; set; }

    }

    public class LotWithImageDTO
    {
        public LotDTO LotInfo { get; set; }
        public byte[] Image { get; set; }
    }

    public class OffersDTO
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
    }
}
