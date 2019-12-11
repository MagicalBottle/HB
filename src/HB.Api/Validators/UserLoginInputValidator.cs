using FluentValidation;
using HB.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Validators
{
    public class UserLoginInputValidator : HBApiBaseValidator<UserLoginInput>
    {
        public UserLoginInputValidator()
        {
            RuleFor(c => c.UserName).NotEmpty().WithMessage("登录名不能为空");
            RuleFor(c => c.Password).NotEmpty().WithMessage("密码不能为空");
            //RuleFor(c => c.Captcha).NotEmpty().WithMessage("验证码不能为空");
        }
    }
}
