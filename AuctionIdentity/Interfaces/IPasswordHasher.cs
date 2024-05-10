namespace AuctionIdentity.Interfaces
{
    internal interface IPasswordHasher
    {
        public string GenereatePassword(string password);
        public bool Verify(string password, string hashedPassword);
    }
}
