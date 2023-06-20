using AutoMapper;
using BOL.ModelDtos;
using DAL.Models;
using DAL.Common;
using Microsoft.EntityFrameworkCore;

namespace DAL.Student
{
    public class StudentDal
    {
        private readonly DemoDbContext _demoDbContext;   

        public StudentDal( )
        {
            _demoDbContext = new DemoDbContext( );
           
        }
        #region Get Functions
        public List<StudentDto> GetallStudents()
        {
            var StudentDtoList = new List<StudentDto>();
            var StudentsList = _demoDbContext.StudentDbTables.ToList().FindAll(x=>x.Email!=null); 
            foreach (var Students in StudentsList)                   
              {
               var StudentDto = new StudentDto();                          
               CopyToDtoFromModel(StudentDto, Students);               
               StudentDtoList.Add(StudentDto);
              }           
            return StudentDtoList;
        }


        #region logincheck
        public StudentDto GetSinglelStudentByUsernameAndEmail(StudentDto studentDto)
        {
            var StudentDto = new StudentDto();
            var student = _demoDbContext.StudentDbTables.ToList().FirstOrDefault(x => x.StudentName == studentDto.StudentName && x.Email == studentDto.Email);
            if(null != student)
            CopyToDtoFromModel(StudentDto, student);
            return StudentDto;
        }
        #endregion

        public StudentDto GetSinglelStudent(int StudentId)
        {
            var StudentDto = new StudentDto();
            var student = _demoDbContext.StudentDbTables.ToList().FirstOrDefault(x => x.StudentId == StudentId);
            CopyToDtoFromModel(StudentDto, student);
            return StudentDto;
        }
        #endregion

        #region Delete
        public bool DeleteStudent(int StudentId)
        {
            var rbet = false;
            var student = _demoDbContext.StudentDbTables.FirstOrDefault(student => student.StudentId == StudentId);
            if (student != null)
            {
                try
                {
                    _demoDbContext.StudentDbTables.Remove(student);
                    _demoDbContext.SaveChanges();
                    rbet = true;
                }
                catch
                {
                    rbet = false;
                }

            }
            else
                rbet = false;
            return rbet;
        }
        #endregion

        #region Save Functions
        public bool SaveStudentDal (StudentDto studentDtoFromUser)
        {
            bool rbet = false;
            var StudentDbTableObject = new StudentDbTable();
            if (null != StudentDbTableObject)
            {
                CopyToModelFromDto(StudentDbTableObject, studentDtoFromUser);
                try
                {
                    _demoDbContext.StudentDbTables.Add(StudentDbTableObject);
                    _demoDbContext.SaveChanges();
                    rbet = true;
                }
                catch
                {
                    rbet = false;
                }
            }
            return rbet;
        }

        #endregion Save Functions

        #region Update function
        public bool UpdateStudentDal(int id,StudentDto studentDtoFromUser)
        {
            bool rbet = false;        
            try
            {
                var StudentObjectFromDatabase = _demoDbContext.StudentDbTables.FirstOrDefault(student => student.StudentId == id);
                if (null != StudentObjectFromDatabase)
                {
                    CopyToModelFromDto(StudentObjectFromDatabase, studentDtoFromUser);
                    _demoDbContext.SaveChangesAsync();
                    rbet = true;
                }
            }
            catch
            {
                rbet = false;
            }
            return rbet;
        }
        #endregion


        #region Copyfunction
        private StudentDto CopyToDtoMapper(StudentDto destination, StudentDbTable source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentDto, StudentDbTable>().ForMember(c => c.Stunthobbies, option => option.Ignore()));
            var mapper = new Mapper(config);
            destination = mapper.Map<StudentDto>(source);
            return destination;
        }   
        private void CopyToModelMapper(StudentDbTable destination, StudentDto source)
        {
            var configformodel = new MapperConfiguration(cfg => cfg.CreateMap<StudentDto, StudentDbTable>().ForMember(c => c.Stunthobbies, option => option.Ignore()));
            var mapperformodel = new Mapper(configformodel);
            destination = mapperformodel.Map<StudentDbTable>(source);
        }
        private void CopyToModelFromDto(StudentDbTable destination, StudentDto source)
        {
            destination.StudentId = source.StudentId;
            destination.StudentName = source.StudentName;
            destination.Email = source.Email;
            destination.Dob = source.Dob;
            destination.Class = source.Class;
        }
        private void CopyToDtoFromModel(StudentDto destination, StudentDbTable source)
        {
            destination.StudentId = source.StudentId;
            destination.StudentName = source.StudentName;
            destination.Email = source.Email;
            destination.Dob = source.Dob;
            destination.Class = source.Class;
        }



        #endregion
    }
}
