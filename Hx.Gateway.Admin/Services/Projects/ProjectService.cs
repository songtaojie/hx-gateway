using Hx.Gateway.Admin.Services.Projects.Dto;
using Hx.Gateway.Application.Services.Projects;
using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;

namespace Hx.Gateway.Admin.Services
{
    public class ProjectService
    {
        private readonly ISqlSugarRepository<TgProject> _tgProjectRep;
        public ProjectService(ISqlSugarRepository<TgProject> tgProjectRep)
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
        public async Task<List<Guid>> GetAllEnabledProjectIdsAsync()
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
        public async Task<PagedListResult<PageProjectOutput>> GetPageProjectAsync(PageProjectInput input)
        {
            return await _tgProjectRep.AsQueryable()
                .WhereIF(input.Status.HasValue, o => o.Status == input.Status)
                .OrderByDescending(o => o.CreateTime)
                .Select<PageProjectOutput>()
                .ToPagedListAsync(input.Page, input.PageSize);
        }

        #endregion

        #region 新增
        /// <summary>
        /// 新增项目信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> SaveProjectAsync(SaveProjectInput request)
        {
            if (request.Id.HasValue && request.Id != Guid.Empty)
            {
                var project = new TgProject
                {
                    Id = request.Id.Value,
                    Code = request.Code,
                    Name = request.Name,
                    Status = request.Status
                };
                return await _tgProjectRep.UpdateAsync(project) > 0;
            }
            else
            {
                var project = new TgProject
                {
                    Code = request.Code,
                    Name = request.Name,
                    Status = request.Status
                };
                return await _tgProjectRep.InsertAsync(project) > 0;
            }
        }
        #endregion
    }
}