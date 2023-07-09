using Hx.Gateway.Application.Services.Projects;
using Hx.Gateway.Application.Services.Routes;
using Hx.Sdk.Entity;
using Hx.Sdk.Extensions;
using Hx.Sdk.Sqlsugar;

namespace Hx.Gateway.Application.Services
{
    public class ProjectService :  ITransientDependency
    {
        private readonly SqlSugarRepository<TgProject> _tgProjectRep;
        public ProjectService(SqlSugarRepository<TgProject> tgProjectRep)
        {
            _tgProjectRep = tgProjectRep;
        }

        #region 查询

        /// <summary>
        /// 查询项目信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<TgProject>> GetProjectList()
        {
            return await _tgProjectRep.AsQueryable()
                .Includes(o => o.Routes, route => route.DownstreamHostAndPorts)
                .Includes(o => o.Routes, route => route.RouteProperties)
                .OrderByDescending(o => o.SortIndex)
                .Where(o => o.Status == StatusEnum.Enable)
                .ToListAsync();
        }

        /// <summary>
        /// 查询所有可用的项目
        /// </summary>
        /// <returns></returns>
        public async Task<List<long>> GetAllEnabledProjectIdsAsync()
        {
            return await _tgProjectRep.AsQueryable()
                .Where(o => o.Status == StatusEnum.Enable)
                .Select(o => o.Id)
                .ToListAsync();
        }

        /// <summary>
        /// 分页查询项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedListResult<TgProject>> GetPageProjectAsync([FromQuery]PageProjectInput input)
        {
            return await _tgProjectRep.AsQueryable()
                .WhereIF(input.Status.HasValue, o => o.Status == input.Status)
                .OrderByDescending(o => o.CreateTime)
                .ToPagedListAsync(input.Page, input.PageSize);
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增项目信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> AddProjectAsync(AddProjectInput request)
        {
            var Project = request.Adapt<TgProject>();
            var result = await _tgProjectRep.InsertAsync(Project) > 0;
            if (result)
            {
                return "新增项目成功";
            }
            else
            {
                throw new UserFriendlyException("新增项目失败", (int)GatewayErrorCodeEnum.INSERT_PROJECT_FAIL);
            }
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 更新项目信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> UpdateProjectAsync(UpdateProjectInput request)
        {
            var project = request.Adapt<TgProject>();
            var result = await _tgProjectRep.Context.Updateable(project)
                    .UpdateColumns(u => new
                    {
                        u.Name,
                        u.SortIndex
                    }).ExecuteCommandHasChangeAsync();
            if (result)
            {
                return "编辑项目成功";
            }
            else
            {
                throw new UserFriendlyException("编辑项目失败", (int)GatewayErrorCodeEnum.UPDATE_PROJECT_FAIL);
            }
        }

        /// <summary>
        /// 启用或禁用项目信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public async Task<string> PatchProjectAsync(long projectId, StatusEnum status)
        {
            var result = await _tgProjectRep.Context.Updateable<TgProject>()
                .SetColumns(it => new TgProject()
                {
                    Status = status,
                    UpdateTime = DateTime.Now
                })
                .Where(it => it.Id == projectId)
                .ExecuteCommandHasChangeAsync();
            if (result)
            {
                return $"{status.GetDescription()}项目成功";
            }
            else
            {
                var errorCode = status == StatusEnum.Enable
                    ? GatewayErrorCodeEnum.ENABLE_PROJECT_FAIL
                    : GatewayErrorCodeEnum.DISABLE_PROJECT_FAIL;
                throw new UserFriendlyException("禁用/启用项目失败", (int)errorCode);
            }
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除项目信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public async Task<string> DeleteProjectAsync(int projectId)
        {
            var result = await _tgProjectRep.Context.Deleteable<TgProject>()
                .Where(it => it.Id == projectId)
                .ExecuteCommandHasChangeAsync();
            if (result)
            {
                return $"删除项目成功";
            }
            else
            {
                throw new UserFriendlyException("删除项目失败", (int)GatewayErrorCodeEnum.DELETE_PROJECT_FAIL);
            }
        }

        #endregion
    }
}