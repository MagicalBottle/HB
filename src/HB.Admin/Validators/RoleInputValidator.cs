using FluentValidation;
using HB.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Admin.Validators
{
    public class RoleInputValidator : HBAdminBaseValidator<RoleInput>
    {
        public RoleInputValidator()
        {
            RuleFor(c => c.RoleName).NotEmpty().WithMessage("角色名称不能为空");
            //RuleFor(c => c.RoleStatus).NotEmpty().WithMessage("角色状态不能为空");
            //RuleFor(c => c.RoleRemark).NotEmpty().WithMessage("密码不能为空");
            //RuleFor(c => c.AdminIds).NotEmpty().WithMessage("必须分配一个角色");
        }
    }
}
