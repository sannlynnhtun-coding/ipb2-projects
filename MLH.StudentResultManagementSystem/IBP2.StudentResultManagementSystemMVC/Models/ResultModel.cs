namespace IBP2.StudentResultManagementSystemMVC.Models
{
    public class ResultModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public double Marks { get; set; }
        public string Grade { get; set; } = null!;
        public virtual StudentModel Student { get; set; } = null!;
        public virtual SubjectModel Subject { get; set; } = null!;
    }
}
