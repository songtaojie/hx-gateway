using Hx.Gateway.Admin.Services.Projects.Dto;
using Hx.Gateway.Application.Services.Projects;
using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;

namespace Hx.Gateway.Admin.Services
{
    public class ProjectService
    {
        private readonly ISqlSugarRepository<TgProject> _rep;
        public ProjectService(ISqlSugarRepository<TgProject> rep)
        {
            _rep = rep;
        }

        #region 查询

        /// <summary>
        /// 查询项目信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<TgProject>> GetProjectList()
        {
            return await _rep.AsQueryable()
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
            return await _rep.AsQueryable()
                .Where(o => o.Status == StatusEnum.Enable)
                .Select(o => o.Id)
                .ToListAsync();
        }

        /// <summary>
        /// 分页查询项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedListResult<PageProjectOutput>> GetPageAsync(PageProjectInput input)
        {
            return await _rep.AsQueryable()
                .WhereIF(input.Status.HasValue, o => o.Status == input.Status)
                .WhereIF(!string.IsNullOrEmpty(input.Name),o => o.Name.Contains(input.Name))
                .OrderByDescending(o => o.CreateTime)
                .Select<PageProjectOutput>()
                .ToPagedListAsync(input.Page, input.PageSize);
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AddProjectInput input)
        {
            var isExist = await _rep.AnyAsync(u => u.Code == input.Code);
            if (isExist) throw new UserFriendlyException($"已存在编号为【{input.Code}】的项目");
            isExist = await _rep.AnyAsync(u => u.Name == input.Name);
            if (isExist) throw new UserFriendlyException($"已存在名称为【{input.Name}】的项目");
            var project = new TgProject
            {
                SortIndex = input.SortIndex,
                Code = input.Code,
                Name = input.Name,
                Status = input.Status
            };
            return await _rep.InsertAsync(project) > 0;
        }


        /// <summary>
        /// 编辑项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(UpdateProjectInput input)
        {
           var entity = await _rep.AsQueryable()
                .Where(u => u.Id == input.Id)
                .FirstAsync();
            if (entity == null)
                throw new UserFriendlyException("项目信息不存在");
            var isExist = await _rep.AnyAsync(u => u.Code == input.Code && u.Id != input.Id);
            if (isExist) throw new UserFriendlyException($"已存在编号为【{input.Code}】的项目");
            isExist = await _rep.AnyAsync(u => u.Name == input.Name && u.Id != input.Id);
            if (isExist) throw new UserFriendlyException($"已存在名称为【{input.Name}】的项目");
            entity.Code = input.Code;
            entity.Name = input.Name;
            entity.Status = input.Status;
            entity.UpdateTime = DateTime.Now;
            var result =await _rep.Context.Updateable(entity)
                .UpdateColumns(u => new { u.Code, u.Name,u.Status, u.UpdateTime })
                .ExecuteCommandAsync();
            return result > 0;
        }

        /// <summary>
        ///删除项目信息
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(DeleteProjectInput input)
        {
            return await _rep.DeleteAsync(input.Id) > 0;
        }
        #endregion
    }
}