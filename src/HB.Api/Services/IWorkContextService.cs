using HB.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Api.Services
{
   public interface IWorkContextService
    {
        MebUser User { get; set; }

        HttpContext HttpContext { get; }
    }
}
