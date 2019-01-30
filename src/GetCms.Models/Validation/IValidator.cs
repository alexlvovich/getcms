using GetCms.Models.General;

namespace GetCms.Models.Validation
{
    public interface IValidator<T>
    {
        Result Validate(T entity);
    }
}
