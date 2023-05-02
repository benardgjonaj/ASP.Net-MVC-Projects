using MovieStoreApp.Models.DAL;


namespace MovieStoreApp.Models.BLL
{
    public class CategoriesManager
    {

        public static List<DTO.Category> GetCategories()
        {
            List<Category> dbcategory = Category.GetCategories();
            List<DTO.Category> result = new List<DTO.Category>();
            foreach (var category in dbcategory)
            {
                if (category.IsDeleted)
                    continue;
                result.Add(new DTO.Category
                {
                    Id =category.Id,
                    Name=category.Name,
                    Description=category.Description
                 

                });
            }
            return result;
        }
        public static DTO.Category GetCategorById(int id) {
            Category dbCategory = Category.GetCategoryById(id);
            if (dbCategory != null)
            {
                return new DTO.Category
                {
                    Id=dbCategory.Id,
                    Name=dbCategory.Name,
                    Description=dbCategory.Description
                };
            }
            return null; ;
        }
        public static DTO.Category AddNewCategory(DTO.Category model) {
            var existscategoryByName = DAL.Category.GetByName(model.Name);
            if (existscategoryByName != null)
            {

                throw new Exception("Already exist a category by this name: ");
            }
            DAL.Category categoryToInsert = new DAL.Category
            {
              Name=model.Name,
              Description=model.Description
            };


            bool insertResult = Category.Insert(categoryToInsert);


            if (insertResult)
            {
                return model;
            }

            else
            {
                return null;
            }
        }
        public static DTO.Category EditCategory(int id, DTO.Category model)
        {
            DAL.Category category = DAL.Category.GetCategoryById(id);


            if (category != null)
            {
                var existscategoryByName= DAL.Category.GetByName(model.Name);
                if (existscategoryByName != null && existscategoryByName.Id != id)
                {
                    throw new Exception("Already exist a category by this name: ");
                }
                category.Name=model.Name; 
                category.Description=model.Description;

                bool updated = Category.Update(category);

                if (updated)
                {

                    return new DTO.Category
                    {
                        Id = category.Id,
                        Name=category.Name,
                        Description=category.Description
                    };
                }
            }
            return null;
        }
        public static void DeleteCategory(int id)
        {
            bool result = Category.DeleteCategory(id);
            if (result)
                MovieCategory.DeleteCategory(id);
            
        }

        public static DTO.Category GetCategoriesWithMovies()
        {
            var categories = CategoriesManager.GetCategories();
            var model = new DTO.Category();
            foreach (var category in categories)
            {
                var movies =MovieCategoriesManager.GetMoviesByCategoryId(category.Id);
                model.CategoriesWithMovies.Add(new DTO.MovieCategory
                {
                    Category = category,
                    Movies = movies
                });
            }
            return model;

        }
        public static List<DTO.CategoryViewModel> GetCategoriesViewModel()
        {
            List<Category> dbcategory = Category.GetCategories();
            List<DTO.CategoryViewModel> result = new List<DTO.CategoryViewModel>();
            foreach (var category in dbcategory)
            {
                if (category.IsDeleted)
                    continue;
                result.Add(new DTO.CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,


                }) ;
            }
            return result;
        }
        public static List<DTO.CategoryViewModel> GetCategoriesByMovieId(int movieId)
        {
            var categories = Category.GetCategoriesByMovieId(movieId);
            var dtoCategories = new List<DTO.CategoryViewModel>();
            foreach (var category in categories)
            {
                var dtoCategory = new DTO.CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name
                };
                dtoCategories.Add(dtoCategory);
            }
            return dtoCategories;
        }
    }
}
