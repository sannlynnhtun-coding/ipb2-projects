namespace IBP2.StudentResultManagementSystemMVC.Models
{
    public class UpdateStudentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool? IsDelete { get; set; }
    }
}
