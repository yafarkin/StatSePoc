using System.Data;

namespace DataService.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}