using Ems.BusinessLogic.Abstract;
using Ems.DataAccessLayer.Abstract;

namespace Ems.BusinessLogic.Concrete;
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<IDictionary<int, string>> GetCategoriesDictionaryAsync()
    {
        return await _categoryRepository.GetDictionaryAsync(x => x.Id, x => x.CategoryName);
    }
}
