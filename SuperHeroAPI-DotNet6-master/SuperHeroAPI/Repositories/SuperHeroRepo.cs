
using Microsoft.Extensions.Options;
using SuperHeroAPI.optionClass;
using TinifyAPI;

namespace SuperHeroAPI.Repositories
{
    public class SuperHeroRepo : ISuperHeroRepo
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        private readonly IOptions<SuperHeroOption> op;

        public SuperHeroRepo(DataContext context, IConfiguration configuration, IOptions<SuperHeroOption> op)
        {
            this.context = context;
            this.configuration = configuration;
            this.op = op;
        }

        public async Task<List<SuperHero>> GetAllHero()
        {
            return await context.SuperHeroes.ToListAsync();
        }

        public async Task<SuperHero> GetHero(int id)
        {
            return await context.SuperHeroes.FindAsync(id);
        }

        public async Task<SuperHero> AddNewHero(SuperHero newHero)
        {

            context.SuperHeroes.Add(newHero);
            await context.SaveChangesAsync();
            return newHero;
        }

        public async Task<SuperHero> UpdateOurHero(SuperHero UpdatedHero, int id)
        {
            var ourHero = await context.SuperHeroes.FindAsync(id);
            if(ourHero != null)
            {
                ourHero.CharacterName = UpdatedHero.CharacterName;
                ourHero.RealName = UpdatedHero.RealName;
                ourHero.Description = UpdatedHero.Description;
                ourHero.Power = UpdatedHero.Power;
                await context.SaveChangesAsync();
                return ourHero;
            }
            else
            {
                return null;
            }   
        }

        public async Task<string> DeleteHero(int id)
        {
            var ourHero = await context.SuperHeroes.FindAsync(id);
            if(ourHero == null)
            {
                return null;
            }
            context.SuperHeroes.Remove(ourHero);
            await context.SaveChangesAsync();
            return "Deleted Successfully";
        }

        public async Task UploadHeroImage(IFormFile file, int id)
        {
            if (!Directory.Exists(op.Value.UploadedFolderPath))
            {
                Directory.CreateDirectory(op.Value.UploadedFolderPath);
            }
            if (!Directory.Exists(op.Value.TrashFolderPath))
            {
                Directory.CreateDirectory(op.Value.TrashFolderPath);
            }
            var hero = await context.SuperHeroes.FindAsync(id);

            Tinify.Key = op.Value.Tinify;
            var temp = op.Value.TrashFolderPath + file.FileName;
            var original = op.Value.UploadedFolderPath + file.FileName;

            using (FileStream fs = System.IO.File.Create(original))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            using (FileStream fs = System.IO.File.Create(temp))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            var source = Tinify.FromFile(temp);
            var resized = source.Resize(new
            {
                method = "cover",
                width = 500,
                height = 500
            });
            await resized.ToFile(original);
            hero.ImageURl = original;
            await context.SaveChangesAsync();

           
        }
    }
}
