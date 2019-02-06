using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EFAutoRepository : IAutoRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Auto> Autos
        {
            get { return context.Autos; }
        }

        public void SaveAuto(Auto auto)
        {
            if(auto.Id == 0)
            {
                context.Autos.Add(auto);
            }
            else
            {
                Auto dbEntry = context.Autos.Find(auto.Id);
                if(dbEntry != null)
                {
                    dbEntry.Name = auto.Name;
                    dbEntry.Manufacturer = auto.Manufacturer;
                    dbEntry.Description = auto.Description;
                    dbEntry.Type = auto.Type;
                    dbEntry.Price = auto.Price;
                    dbEntry.ImageData = auto.ImageData;
                    dbEntry.ImageMimeType = auto.ImageMimeType;
                }
            }
            context.SaveChanges();
        }
        public Auto DeleteAuto(int Id)
        {
            Auto dbEntry = context.Autos.Find(Id);
            if (dbEntry != null)
            {
                context.Autos.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

    }
}
