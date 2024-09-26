namespace CommonDTO
{
    public class LotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = String.Empty;
        public double StartPrice { get; set; } = 1;
        public double CurrentPrice { get; set; } = 1;
        public DateTime DateTime { get; set; } = DateTime.Now;

    }

    public class LotWithImageDTO
    {
        public LotDTO LotInfo { get; set; }
        public byte[] Image { get; set; }
    }
}
