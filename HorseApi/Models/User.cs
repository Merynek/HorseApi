namespace HorseApi.Models
{
    public class User
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string password { get; set; }
        public string refreshToken { get; set; }
    }
}
