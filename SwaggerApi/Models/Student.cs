namespace SwaggerApi.Models
{
    public class Student
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        /// <example>示例</example>
        public string UserName { get; set; }
        /// <summary>
        /// 用户名年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 性别
        /// <remarks>
        /// 0：男
        /// 1：女
        /// 2：其他
        /// </remarks>
        /// </summary>

        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        /// <summary>
        /// 男
        /// </summary>
        M = 0,
        /// <summary>
        /// 女
        /// </summary>
        F = 1,
        /// <summary>
        /// 其他
        /// </summary>
        O = 2
    }

}
