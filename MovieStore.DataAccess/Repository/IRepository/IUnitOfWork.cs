using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        MovieRepository Movie { get; }
        GenreRepository Genre { get; }

        void Save();
    }
}
