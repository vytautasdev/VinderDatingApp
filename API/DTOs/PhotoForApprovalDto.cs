namespace API.DTOs
{
    public class PhotoForApprovalDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Username { get; set; }
        public bool IsApproved { get; set; }
    }
}

//  dotnet ef migrations add PostgresInitial -o Data/Migrations
