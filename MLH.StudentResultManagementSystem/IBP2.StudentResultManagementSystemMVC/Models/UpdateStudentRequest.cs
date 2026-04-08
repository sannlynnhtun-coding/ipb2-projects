namespace IBP2.StudentResultManagementSystemMVC.Models
{
    public class UpdateStudentRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
