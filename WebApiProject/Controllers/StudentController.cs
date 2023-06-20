using BLL.Controllers;
using BOL.ModelDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiProject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApiProject.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentBll _studentBll;
        private readonly JWTSettings _jwtsettings;
        private readonly ILogger<StudentController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StudentController(ILogger<StudentController> logger,IOptions<JWTSettings> options, IWebHostEnvironment hostEnvironment)
        {
            _studentBll = new StudentBll();
            _logger = logger;
            _jwtsettings = options.Value;
            _hostEnvironment = hostEnvironment;
        }


        #region get 
        [HttpGet]       
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [HttpPost]
        public ActionResult SaveStudent([FromBody] StudentDto studentDto)
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
                if (_studentBll.DeleteStudentBll(id))
                {
                    return Ok();
                }
            }
            return NoContent();
        }
        #endregion

        #region update
        [HttpPut]
        public ActionResult UpdateStudent(int id,[FromBody]StudentDto studentDto) 
        {
            _logger.LogInformation("Inside UpdateStudent ");
            if (id != studentDto.StudentId)
            {
                return BadRequest();
            }
            else
            {
                return Ok(_studentBll.UpdateStudentBll(id,studentDto));
            }            
        }
        #endregion

        #region Account

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public ActionResult Authenticate([FromBody] StudentDto studentDto)
        {
            var isStudentDtoFound = _studentBll.GetSinglelStudentByUsernameAndEmailBll(studentDto);
            if (isStudentDtoFound.StudentId == 0)
                return Unauthorized();
            var tokenhandler = new JwtSecurityTokenHandler();
            var jsonObj = GetApplicationJson();
            // Parse the JSON string into a C# object      
            var tokenkey = Encoding.UTF8.GetBytes(jsonObj.ToString());
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    { new Claim(ClaimTypes.Name, studentDto.StudentId.ToString()), new Claim(ClaimTypes.Role,"Admin")
                    }
               ),
                Expires = DateTime.Now.AddSeconds(180),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256),

            };
            var token = tokenhandler.CreateToken(tokenDescription);
            string finalToken = tokenhandler.WriteToken(token);
            return Ok(finalToken);
        }

        #endregion

        [HttpGet("GetApplicationJson")]
        public string GetApplicationJson()
        {
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "appsettings.json");     
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }

            string jsonContent = System.IO.File.ReadAllText(filePath);
            // return Content(jsonContent, "application/json");
            return jsonContent;
        }




    }
}
