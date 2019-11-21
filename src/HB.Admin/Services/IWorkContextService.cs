using HB.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Admin.Services
{
    public interface IWorkContextService
    {
        SysAdmin Admin { get; set; }

        HttpContext HttpContext { get; }
    }
}
