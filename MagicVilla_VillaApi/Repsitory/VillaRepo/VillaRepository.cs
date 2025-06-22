using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Repsitory.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaApi.Repsitory
{
    public class VillaRepository : Repository<Villa>, IVillaRepo
    {
        public ApplicationDBContext _db { get; }
        public VillaRepository(ApplicationDBContext db):base(db)
        {
            this._db = db;
            
        }


        public async Task<Villa> Update(Villa villa)
        {
            villa.UpdateDate = DateTime.Now;
            _db.Villas.Update(villa);
            await _db.SaveChangesAsync();
            return villa;
        }
    }
}