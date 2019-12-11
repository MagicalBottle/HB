using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace HB.Admin.Validators
{
    public abstract class HBAdminBaseValidator<T> : AbstractValidator<T> where T : class
    {
    }
}
