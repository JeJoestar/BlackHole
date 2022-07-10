namespace BlackHole.DAL
{
    public class User : BaseEntity
    {
        public string Mail { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}