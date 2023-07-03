namespace Hx.Gateway.Application.Services.Projects;

public class UpdateProjectInput : AddProjectInput
{
    /// <summary>
    /// 项目Id
    ///</summary>
    [Required]
    public long Id { get; set; }
}
