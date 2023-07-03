using Furion.JsonSerialization;
using Hx.Gateway.Application.Options.Ocelot;
using Hx.Gateway.Application.Services.GlobalConfiguration;
using Hx.Gateway.Application.Services.Projects;
using Hx.Gateway.Application.Services.Routes.Dtos;

namespace Hx.Gateway.Application.Services.Routes
{
    public class RouteService : IDynamicApiController, ITransient
    {
        private readonly GlobalConfigurationService _globalConfigurationService;
        private readonly ProjectService _projectService;
        private readonly SqlSugarRepository<TgRoute> _tgRouteRep;
        public RouteService(SqlSugarRepository<TgRoute> tgRouteRep, 
            GlobalConfigurationService globalConfigurationService, 
            ProjectService projectService)
        {
            _tgRouteRep = tgRouteRep;
            _globalConfigurationService = globalConfigurationService;
            _projectService = projectService;
        }

        #region 查询

        /// <summary>
        /// 预览路由信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRoutePreviewAsync()
        {
            var ocelotRoot = new OcelotRoot();
            // 获取全局变量
            var globalConfiguration = await _globalConfigurationService.GetGlobalConfigurationAsync();
            if (globalConfiguration != null && globalConfiguration.Status == StatusEnum.Enable)
            {
                ocelotRoot.GlobalConfiguration = globalConfiguration.Adapt<OcelotGlobalConfigurationNode>();
            }
            var projects = await _projectService.GetProjectList();
            if (projects.Count == 0)
            {
                return "{}";
            }
            var allRoutes = projects.SelectMany(r => r.Routes);
            var configRoutes = allRoutes.Adapt<List<OcelotRouteNode>>();
            ocelotRoot.Routes = configRoutes;
            return JSON.Serialize(ocelotRoot);
        }


        /// <summary>
        /// 查询路由具体信息
        /// </summary>
        /// <param name="projectIds">项目Id集合</param>
        /// <returns></returns>
        public async Task<List<OcelotRouteNode>> GetRouteByProjectIdsAsync(List<long> projectIds)
        {
            var route = await _tgRouteRep.Context.Queryable<TgRoute>().With(SqlWith.NoLock)
                 .Includes(o => o.DownstreamHostAndPorts)
                 .Includes(o => o.RouteProperties)
                 .Where(o => projectIds.Contains(o.ProjectId) && o.Status ==  StatusEnum.Enable)
                 .ToListAsync();
            return route.Adapt<List<OcelotRouteNode>>();
        }

        /// <summary>
        /// 分页查询项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SqlSugarPagedList<PageRouteOutput>> GetPageRouteAsync([FromQuery]PageRouteInput input)
        {
            return await _tgRouteRep.Context.Queryable<TgRoute>()
              .WhereIF(!string.IsNullOrWhiteSpace(input.UpstreamPathTemplate), o => o.UpstreamPathTemplate.Contains(input.UpstreamPathTemplate))
              .WhereIF(!string.IsNullOrWhiteSpace(input.DownstreamPathTemplate), o => o.DownstreamPathTemplate.Contains(input.DownstreamPathTemplate))
              .WhereIF(!string.IsNullOrWhiteSpace(input.RequestIdKey), o => o.RequestIdKey.Contains(input.RequestIdKey))
              .WhereIF(input.Status.HasValue, o => o.Status == input.Status)
              .Where(o => o.ProjectId == input.ProjectId)
              .Select<PageRouteOutput>()
              .OrderByDescending(o => o.Id)
              .ToPagedListAsync(input.Page, input.PageSize);
        }

        /// <summary>
        /// 查询路由具体信息
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public async Task<DetailRouteOutput> GetRouteAsync(long routeId)
        {
            var route = await _tgRouteRep.Context.Queryable<TgRoute>()
                 .Includes(o => o.DownstreamHostAndPorts)
                 .Includes(o => o.RouteProperties)
                 .Where(o => o.Id == routeId)
                 .FirstAsync();
            return route.Adapt<DetailRouteOutput>();
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增路由信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<long> AddRouteAsync(AddRouteInput request)
        {
            var route = request.Adapt<TgRoute>();
            var insertResult = await _tgRouteRep.Context.InsertNav(route)
                .Include(o => o.DownstreamHostAndPorts)
                .Include(o => o.RouteProperties)
                .ExecuteCommandAsync();
            if (insertResult)
            {
                var now = DateTime.Now;
                await _tgRouteRep.Context.Updateable<TgProject>()
                    .SetColumns(o => o.UpdateTime == now)
                    .Where(o => o.Id == request.ProjectId)
                    .ExecuteCommandAsync();
                return route.Id;
            }
            else
            {
                throw Oops.Oh(GatewayErrorCodeEnum.INSERT_PROJECT_FAIL).StatusCode((int)GatewayErrorCodeEnum.INSERT_PROJECT_FAIL);
            }
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 更新路由信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<long> UpdateRouteAsync(UpdateRouteInput request)
        {
            var route = request.Adapt<TgRoute>();
            var result = await _tgRouteRep.Context.DeleteNav(route)
                .Include(o => o.DownstreamHostAndPorts)
                .Include(o => o.RouteProperties)
                .ExecuteCommandAsync();
            if (result)
            {
                var now = DateTime.Now;
                await _tgRouteRep.Context.Updateable<TgProject>()
                    .SetColumns(o => o.UpdateTime == now)
                    .Where(o => o.Id == request.ProjectId)
                    .ExecuteCommandAsync();
                return await AddRouteAsync(request);
            }
            else
            {
                throw Oops.Oh(GatewayErrorCodeEnum.UPDATE_PROJECT_FAIL).StatusCode((int)GatewayErrorCodeEnum.UPDATE_PROJECT_FAIL);
            }
        }

        /// <summary>
        /// 启用或禁用项目信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public async Task<string> PatchProjectAsync(int projectId, StatusEnum status)
        {
            var result = await _tgRouteRep.Context.Updateable<TgProject>()
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
                throw Oops.Oh(errorCode).StatusCode((int)errorCode);
            }
        }

        /// <summary>
        /// 启用或禁用路由信息
        /// </summary>
        /// <param name="routeId">路由Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public async Task<string> PatchRouteStatusAsync(long routeId, StatusEnum status)
        {
            var result = await _tgRouteRep.Context.Updateable<TgRoute>()
                .SetColumns(it => new TgRoute() 
                { 
                    Status = status
                })
                .Where(it => it.Id == routeId)
                .ExecuteCommandHasChangeAsync();
            if (result)
            {
                return $"{status.GetDescription()}路由成功";
            }
            else
            {
                var errorCode = status == StatusEnum.Enable 
                    ? GatewayErrorCodeEnum.ENABLE_ROUTE_FAIL 
                    : GatewayErrorCodeEnum.DISABLE_ROUTE_FAIL;
                throw Oops.Oh(errorCode).StatusCode((int)errorCode);
            }
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除路由信息
        /// </summary>
        /// <param name="routeId">路由Id</param>
        /// <returns></returns>
        public async Task<string> DeleteRouteAsync(int routeId)
        {
            var result = await _tgRouteRep.Context.DeleteNav<TgRoute>(it => it.Id == routeId)
                .Include(o => o.DownstreamHostAndPorts)
                .Include(o => o.RouteProperties)
                .ExecuteCommandAsync();
            if (result)
            {
                return $"删除路由成功";
            }
            else
            {
                throw Oops.Oh(GatewayErrorCodeEnum.DELETE_ROUTE_FAIL).StatusCode((int)GatewayErrorCodeEnum.DELETE_ROUTE_FAIL);
            }
        }

        #endregion
    }
}