namespace IBP2.StudentResultManagementSystemMVC.Models
{
    public class CreateResultResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public double Marks { get; set; }
        public string Grade { get; set; } = null!;
    }
}
