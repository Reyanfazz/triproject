namespace certificate.Models
{
    public class studentRepo
    {
        public readonly studentContext _context;    

        public studentRepo(studentContext context) 
        {
            _context = context;
        }

        public void Astudent(student student) 
        {
           _context.students.Add(student);
            _context.SaveChanges();
        }
    }
}
