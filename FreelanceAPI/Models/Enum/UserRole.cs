namespace FreelanceMarketplace.API.Enums
{
    /// <summary>
    /// Static role name constants used for seeding and [Authorize(Roles = ...)] checks.
    /// </summary>
    public enum UserRole
    {
        Admin,
        Seller,
        Client
    }
}
