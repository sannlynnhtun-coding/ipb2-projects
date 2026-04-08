namespace IBP2.StudentResultManagementSystemMVC.Models
{
    public class UpdateSubjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Credits { get; set; }
        public bool IsDelete { get; set; }
    }
}
