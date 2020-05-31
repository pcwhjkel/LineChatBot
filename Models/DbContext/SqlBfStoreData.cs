using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class SqlBfStoreData : IBfStoreData {

    private BfStoreDbContext _db { get; set; }

    public SqlBfStoreData (BfStoreDbContext db) {
        _db = db;
    }

    public async Task<int> CommitAsync () {
        var code = await _db.SaveChangesAsync ();
        return code;
    }
}