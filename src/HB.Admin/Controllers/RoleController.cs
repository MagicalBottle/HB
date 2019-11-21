using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HB.Admin.Attributes;
using HB.Admin.Configuration;
using HB.Admin.Models;
using HB.Admin.Services;
using HB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HB.Admin.Controllers
{

    public class RoleController : AdminBaseController
    {

        private readonly IRoleService _roleService;
        private readonly IWorkContextService _context;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService,
            IWorkContextService context,
            IMapper mapper)
        {
           _roleService = roleService;
            _context = context;
            _mapper = mapper;
        }

        [AdminAuthorize(Policy = HBPermissionKeys.RoleView)]
        public IActionResult Index()
        {
            return View();
        }


        [AdminAuthorize(Policy = HBPermissionKeys.RoleView)]
        public IActionResult List(RoleQueryParamInput param)
        {
            PagedList<SysRole> role = null;
            var result = new PagedListReponseOutPut<SysRole>();
            try
            {
                role = _roleService.GetRoles(param);

                result.Rows = role;
            }
            catch (Exception ex)
            {
                result.Status = ReutnStatus.Error;
                result.Message = "Error";
            }
            return new JsonResult(JsonConvert.SerializeObject(result, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffff" }));
        }



        [AdminAuthorize(Policy = HBPermissionKeys.RoleView)]
        public IActionResult GetAllRoles()
        {
            List<SysRole> roles = null;
            var result = new ListReponseOutPut<SelectOutPut>();
            try
            {
                roles = _roleService.GetAllRoles();

                List<SelectOutPut> rows = _mapper.Map<List<SysRole>, List<SelectOutPut>>(roles);
                result.Rows = rows;
            }
            catch (Exception ex)
            {
                result.Status = ReutnStatus.Error;
                result.Code = "get_data_error";
                result.Message = "Error";
            }
            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        [AdminAuthorize(Policy = HBPermissionKeys.RoleAdd)]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [AdminAuthorize(Policy = HBPermissionKeys.RoleAdd)]
        [HttpPost]
        public IActionResult Add(RoleInput param)
        {
            var response = new ReponseOutPut();
            response.Code = "role_add_success";
            response.Message = "新增角色成功";
            if (!ModelState.IsValid)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "param_vaild_error";

                var errorProperty = ModelState.Values.First(m => m.ValidationState == ModelValidationState.Invalid);
                response.Message = errorProperty.Errors.First().ErrorMessage;//验证不通过的 //全局配置一个验证不通过就不在验证了，只存在一个错误信息

                return new JsonResult(JsonConvert.SerializeObject(response));
            }

            // 检查用户名是否重复
            var isExistUserName = _roleService.ExistRoleName(param.RoleName);
            if (isExistUserName)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "rolename_is_exist";
                response.Message = "角色名称已经存在";
                return new JsonResult(JsonConvert.SerializeObject(response));
            }

            SysRole role = _mapper.Map<RoleInput, SysRole>(param);

            role.CreateBy = _context.Admin.Id;
            role.CreatebyName = _context.Admin.UserName;
            role.CreateDate = DateTime.Now;
            role.LastUpdateBy = role.CreateBy;
            role.LastUpdateByName = role.CreatebyName;
            role.LastUpdateDate = role.CreateDate;

            var result = _roleService.AddRole(role, param.AdminIds);
            if (result < 0)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "role_add_error";
                response.Message = "新增角色失败";
            }

            return new JsonResult(JsonConvert.SerializeObject(response));
        }


        [AdminAuthorize(Policy = HBPermissionKeys.RoleEdit)]
        public IActionResult Edit(int id)
        {
            var role = _roleService.GetRoleById(id);
            return View(role);
        }

        [AdminAuthorize(Policy = HBPermissionKeys.RoleEdit)]
        [HttpPost]
        public IActionResult Edit(RoleInput param)
        {
            var response = new ReponseOutPut();
            response.Code = "role_edit_success";
            response.Message = "编辑角色成功";
            if (!ModelState.IsValid)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "param_vaild_error";

                var errorProperty = ModelState.Values.First(m => m.ValidationState == ModelValidationState.Invalid);
                response.Message = errorProperty.Errors.First().ErrorMessage;//验证不通过的 //全局配置一个验证不通过就不在验证了，只存在一个错误信息

                return new JsonResult(JsonConvert.SerializeObject(response));
            }

            // 检查用户名是否重复
            var isExistUserName = _roleService.ExistRoleName(param.RoleName, param.Id);
            if (isExistUserName)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "rolename_is_exist";
                response.Message = "角色名称已经存在";
                return new JsonResult(JsonConvert.SerializeObject(response));
            }

            SysRole role = _mapper.Map<RoleInput, SysRole>(param);

            role.LastUpdateBy = _context.Admin.Id;
            role.LastUpdateByName = _context.Admin.UserName;
            role.LastUpdateDate = DateTime.Now;

            var result = _roleService.UpdateRole(role, param.AdminIds);
            if (result < 0)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "role_edit_error";
                response.Message = "编辑角色失败";
            }

            return new JsonResult(JsonConvert.SerializeObject(response));
        }

        [AdminAuthorize(Policy = HBPermissionKeys.RoleDelete)]
        public IActionResult Delete(int id)
        {
            var result = _roleService.DeleteRoleWithAdminRole(id);

            var response = new ReponseOutPut();
            response.Code = "role_delete_success";
            response.Message = "角色删除成功";
            if (result < 0)
            {
                response.Code = "role_delete_error";
                response.Message = "角色删除失败";
            }
            return new JsonResult(JsonConvert.SerializeObject(response));
        }

        [AdminAuthorize(Policy = HBPermissionKeys.RoleEdit)]
        public IActionResult Permission(int id)
        {
            var model = _roleService.GetRoleWithMenus(id);
            return View(model);
        }


        [AdminAuthorize(Policy = HBPermissionKeys.RoleEdit)]
        [HttpPost]
        public IActionResult Permission(int id, List<int> menuIds)
        {
            var response = new ReponseOutPut();
            response.Code = "success";
            response.Message = "分配权限成功";

            if (menuIds == null || menuIds.Count() <= 0)
            {
                return new JsonResult(JsonConvert.SerializeObject(response));
            }

            var role = new SysRole();
            role.Id = id;
            role.LastUpdateBy = _context.Admin.Id;
            role.LastUpdateByName = _context.Admin.UserName;
            role.LastUpdateDate = DateTime.Now;

            var result = _roleService.UpdatePermission(role, menuIds);
            if (result < 0)
            {
                response.Status = ReutnStatus.Error;
                response.Code = "error";
                response.Message = "分配权限失败";
            }
            return new JsonResult(JsonConvert.SerializeObject(response));
        }
    }
}