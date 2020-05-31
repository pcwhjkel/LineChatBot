using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBfStoreData {
    Task<int> CommitAsync ();
}