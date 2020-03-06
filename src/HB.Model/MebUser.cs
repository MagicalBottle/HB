

using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.IO;
using System.Linq;
using System.Text;

namespace HB.Models
{
    [Table("Meb_User")]
   public  class MebUser : EditorEntity
    {

        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 登录账号名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码 密文
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        [ExplicitKey]
        public string AccessToken { get; set; }

    }
}
