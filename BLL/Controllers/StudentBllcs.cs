using BOL.ModelDtos;
using DAL.Student; 

namespace BLL.Controllers
{
    public class StudentBll
    {
        private readonly StudentDal _studentDal;
        public StudentBll()
        {
            _studentDal = new StudentDal();
        }
        public List<StudentDto> GetAllStudentsBll()
        {
            return (_studentDal.GetallStudents());
        }
        public StudentDto GetSingleStudentBll(int StudentId)
        {
            return (_studentDal.GetSinglelStudent(StudentId));
        }
        public StudentDto GetSinglelStudentByUsernameAndEmailBll(StudentDto studentDto)
        {
            return (_studentDal.GetSinglelStudentByUsernameAndEmail(studentDto));
        }
        public bool DeleteStudentBll(int StudentId)
        {
            return (_studentDal.DeleteStudent(StudentId));
        }
        public bool SaveStudentBll(StudentDto studentDtoFromUser)
        {
            studentDtoFromUser.StudentId = 0;
            return (_studentDal.SaveStudentDal(studentDtoFromUser));
        }
        public bool UpdateStudentBll(int id ,StudentDto studentDtoFromUser)
        {
            return (_studentDal.UpdateStudentDal(id,studentDtoFromUser));
        }

    }
}
