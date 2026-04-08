namespace IBP2.StudentResultManagementSystemMVC.Models
{
    public class UpdateSubjectRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Credits { get; set; }
    }
}
