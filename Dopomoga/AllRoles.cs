namespace Store444.Dopomoga
{
    public static class AllRoles
    {
        public const string Admin = "admin";
        public const string User = "user";
        public const string Shipper = "shipper";

        public static ICollection<string> roles = new[] { Admin, User, Shipper };



    }
}
