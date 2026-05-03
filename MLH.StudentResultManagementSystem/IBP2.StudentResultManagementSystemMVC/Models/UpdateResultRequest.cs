namespace IBP2.StudentResultManagementSystemMVC.Models
{
    public class UpdateResultRequest
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public double Marks { get; set; }
    }
}
