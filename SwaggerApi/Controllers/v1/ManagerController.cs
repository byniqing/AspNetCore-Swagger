using Microsoft.AspNetCore.Mvc;
using SwaggerApi.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace SwaggerApi.Controllers.v1
{
    /// <summary>
    /// 管理部门信息--V1
    /// </summary>
    [Produces("application/json")] //声明控制器的操作支持 application/json 的响应内容类型
    [ApiExplorerSettings(GroupName = "v1")] //版本分组
    //[Route("api/v{api-version:apiVersion}/[controller]/[action]")]
    //[Route("api/v1/[controller]/[action]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly Context _context;
        public ManagerController(Context context)
        {
            _context = context;

        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("/api/v1/Manager/Students")]
        //跟上面效果一样
        //[CustomRoute(ApiVersions.v1, "GetStudents")]
        public IEnumerable<Student> GetStudents()
        {
            List<Student> stu = new List<Student> {
            new Student
            {
                Id = 1,
                UserName = "cnblogs-v1",
                Age = 18
            }};
            return stu;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        [HttpGet("{id}/{name}")]
        public Student QueryOrder(int id, string name)
        {
            var model = new Student
            {

                UserName = name,
                Id = id

            };
            return model;
        }
    }
}