namespace IBP2.StudentResultManagementSystemMVC.Models
{
    public class CreateResultRequest
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public double Marks { get; set; }
    }
}
