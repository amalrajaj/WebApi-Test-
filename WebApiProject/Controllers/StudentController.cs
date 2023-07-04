/***********************************************************************
   Program Name             : StudentController.cs
   Purpose                  : API Controller for Student
   Creation Date            : 1-7-2023
   Created By               : Amalraj 
   Last Modified By         : 
   Modification Date        : 
   Change Request/Bug Nos   :
/***********************************************************************/
using BLL.Controllers;
using BOL.ModelDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using BOL.AccountDto;

namespace WebApiProject.Controllers
{
   // [Route("Secure")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentBll _studentBll;  
        private readonly ILogger<StudentController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _config;

        //Constructor
        public StudentController(IConfiguration config, ILogger<StudentController> logger, IWebHostEnvironment hostEnvironment)
        {
            _studentBll = new StudentBll();
            _logger = logger;       
            _hostEnvironment = hostEnvironment;
            _config = config;
        }

        #region get       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<List<StudentDto>> GetAllStudent()
        {
            _logger.LogInformation("Inside Get all");
            return Ok(_studentBll.GetAllStudentsBll());
        }
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<StudentDto> GetSingleStudent(int id)
        {
            _logger.LogInformation("Inside Get GetSingle ");
            return Ok(_studentBll.GetSingleStudentBll(id));
        }
        #endregion get 

        #region save       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("Save")]
        public ActionResult SaveStudent([FromBody]  StudentDto studentDto)
        {
            _logger.LogInformation("Inside SaveStudent ");
            if (ModelState.IsValid)
            {
                if (studentDto != null)
                {
                    var isSuccess = (_studentBll.SaveStudentBll(studentDto));
                    return new StatusCodeResult(isSuccess ? 200 : 500);
                }
                else
                {

                    return BadRequest();
                }
            }
            else
            {

                return BadRequest(ModelState);
            }
        }
        #endregion save

        #region delete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult StudentDelete(int id)
        {
            
            _logger.LogInformation("Inside StudentDelete ");
            if (id == 0)
            {
                return BadRequest();
            }
            else
            {
                var isSuccess = _studentBll.DeleteStudentBll(id);
                return new StatusCodeResult(isSuccess ? 200 : 500);
              
            }            
        }
        #endregion

        #region update
        [HttpPut]       
        [Route("{id:int}")]
        public ActionResult UpdateStudent(int id, [FromBody] StudentDto studentDto)
        {
            _logger.LogInformation("Inside UpdateStudent ");
            if (id != studentDto.StudentId)
            {
                return BadRequest();
            }
            else
            {
                return Ok(_studentBll.UpdateStudentBll(id, studentDto));
            }
        }
        #endregion

        #region Authetication   

        [AllowAnonymous]      
        [HttpPost("Login")]
        public ActionResult Login([FromBody] StudentDto studentDto)
        {
            var user = Authenticate(studentDto);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(ConvertStringToJson(token));
            }
            return NotFound("user not found");
        }

        // To generate token
        private string GenerateToken(StudentDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));  // get that saved in appsettings.json
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);// security algoritham is HmacSha256
            var claims = new[]// Add Claims
            {
                new Claim(ClaimTypes.NameIdentifier,user.StudentName),
                new Claim(ClaimTypes.Role,"Admin") // default set role = adimn..in this project no user type table thats why
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: credentials);
         return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //To authenticate user
        private StudentDto Authenticate(StudentDto studentDto)
        {
            var isStudentDtoFound = _studentBll.GetSinglelStudentByUsernameAndEmailBll(studentDto);         
            if (isStudentDtoFound.StudentId != 0)
            {
                return isStudentDtoFound;
            }
            else
            return null;// if true return dto else return null
        }


        // copy token into an Dto Object
        public static TokenDto ConvertStringToJson(string jsonString)
        {
            TokenDto tokenDto = new TokenDto();
            tokenDto.token = jsonString;
            return tokenDto;
        }

        #endregion
    }



}

