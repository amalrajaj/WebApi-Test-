/***********************************************************************
   Program Name             : StudentBll.cs
   Purpose                  :  Business Logic Layer for  Student
   Creation Date            : 1-7-2023
   Created By               : Amalraj 
   Last Modified By         : 
   Modification Date        : 
   Change Request/Bug Nos   :
/***********************************************************************/
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
            studentDtoFromUser.StudentId = 0;// For saving Id must be zero
            return (_studentDal.SaveStudentDal(studentDtoFromUser));
        }
        public bool UpdateStudentBll(int id ,StudentDto studentDtoFromUser)
        {
            return (_studentDal.UpdateStudentDal(id,studentDtoFromUser));
        }

    }
}
