namespace Simple_FrontEnd.Models.Account
{
    public class User
    {
        public int UserId { get; set; }
        public int PartId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UtypeId { get; set; }
        public bool Enabled { get; set; }
        public string AccessToken { get; set; }
    }
}