using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwaggerApi.Models;
using SwaggerApi.SwaggerHelper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SwaggerApi.SwaggerHelper.CustomApiVersion;

namespace SwaggerApi.Controllers.v2
{
    /// <summary>
    /// 管理部门信息--V2
    /// </summary>
    [Produces("application/json")] //声明控制器的操作支持 application/json 的响应内容类型
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v2")] //版本分组
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly Context _context;
        public ManagerController(Context context)
        {
            _context = context;
            //初始化数据
            if (!_context.studentItem.Any())
            {
                _context.studentItem.AddRange(new Student
                {
                    Id = 1,
                    UserName = "cnblogs-v2",
                    Age = 18
                },
                new Student
                {
                    Id = 2,
                    UserName = "csdn-v2",
                    Age = 28
                });
                _context.SaveChanges();
            }
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [CustomRoute(ApiVersions.v2, "GetStudents")]
        public IEnumerable<Student> GetStudents()
        {
            List<Student> stu = new List<Student> {
            new Student
            {
                Id = 1,
                UserName = "cnblogs-v2",
                Age = 18
            }};
            return stu;

            //return await _context.studentItem.ToListAsync();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var stu = await _context.studentItem.FindAsync(id);
            if (stu == null)
            {
                return NotFound();
            }
            return Ok(stu);
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <remarks>
        /// 请求示例：
        ///     POST /CreateAsync
        ///     {
        ///        "id": 0,
        ///         "userName": "string",
        ///         "age": 0
        ///     }
        ///
        /// </remarks>
        /// <param name="sutdent"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Student>> CreateAsync(Student sutdent)
        {
            _context.studentItem.Add(sutdent);
            await _context.SaveChangesAsync();

            //添加成功后，返回添加成功的项
            return CreatedAtAction(nameof(GetStudent), new { id = sutdent.Id }, sutdent);
        }
    }
}