using FastFood.Services.Models.Categories;

namespace FastFood.Services.Contracts
{
    public interface ICategoryService
    {
        Task Add(CreateCategoryDto categoryDto);

        Task <ICollection<ListCategoryDto>> GetAll();
    }
}
