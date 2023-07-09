namespace Hx.Gateway.Application.Services.Projects;
public class AddProjectInput
{
    /// <summary>
    /// 项目名称
    ///</summary>
    [Required(ErrorMessage = "项目名称不能为空")]
    public string Name { get; set; }

    /// <summary>
    /// 排序字段
    ///</summary>
    public int SortIndex { get; set; }
}

