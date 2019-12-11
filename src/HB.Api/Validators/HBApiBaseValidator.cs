using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Validators
{
    public abstract class HBApiBaseValidator<T> : AbstractValidator<T> where T : class
    {
    }
}
