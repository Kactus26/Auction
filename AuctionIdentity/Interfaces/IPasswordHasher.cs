namespace AuctionIdentity.Interfaces
{
    public interface IPasswordHasher
    {
        public string GeneratePassword(string password);
        public bool Verify(string password, string hashedPassword);
    }
}
