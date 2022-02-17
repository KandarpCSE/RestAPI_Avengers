
namespace SuperHeroAPI.Repositories
{
    public interface ISuperHeroRepo
    {
        Task<SuperHero> AddNewHero(SuperHero newHero);
        Task<string> DeleteHero(int id);
        Task<List<SuperHero>> GetAllHero();
        Task<SuperHero> GetHero(int id);
        Task<SuperHero> UpdateOurHero(SuperHero UpdatedHero, int id);
        Task UploadHeroImage(IFormFile file, int id);
    }
}