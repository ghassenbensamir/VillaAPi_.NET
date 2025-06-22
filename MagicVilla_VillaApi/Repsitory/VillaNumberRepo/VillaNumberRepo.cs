using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models.VillaNumber;
using MagicVilla_VillaApi.Repsitory.IRepository;

namespace MagicVilla_VillaApi.Repsitory.VillaNumberRepo
{
    public class VillaNumberRepo : Repository<VillaNumber>, IVillaNumberRepo
    {
        public ApplicationDBContext _db { get; }
        public VillaNumberRepo(ApplicationDBContext db) : base(db)
        {
            this._db = db;

        }
        

        public async Task<VillaNumber> Update(VillaNumber villaNo)
        {
            villaNo.UpdateDate = DateTime.Now;
            _db.VillaNumbers.Update(villaNo);
            await _db.SaveChangesAsync();
            return villaNo;
        }
    }
}