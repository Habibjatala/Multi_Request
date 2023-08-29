namespace Multi_Request.Dtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public long Phone { get; set; }

    }
}
