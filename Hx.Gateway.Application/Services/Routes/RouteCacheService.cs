
namespace Hx.Gateway.Application.Services.Routes
{
    public class RouteCacheService : ITransient
    {
        public readonly SqlSugarScopeProvider _sqlserverDB;
        public RouteCacheService(ISqlSugarClient db)
        {
            _sqlserverDB = db.AsTenant().GetConnectionScope(DatabaseConfigIdConst.OcelotSettings);
        }
    }
}